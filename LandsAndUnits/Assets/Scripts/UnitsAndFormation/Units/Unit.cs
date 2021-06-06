using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UnitsAndFormation
{
    [RequireComponent(typeof(Outline))]
    public class Unit : MonoBehaviour, ISaveable
    {
        [SerializeField] private bool _isLoaded;
        [SerializeField] private UnitType Type;
        [SerializeField] private Transform ToolPivot;
        private string id;
        [HideInInspector] public string _ID 
        {
            get
            {
                if (id == null)
                {
                    id = GetComponent<SaveableEntity>().GenerateId();
                }
                return id;
            }
            set
            {
                GetComponent<SaveableEntity>().id = value;
                id = value;
            }
        }
        public UnitBrain _unitBrain { private set; get; }
        public TalkCloudComponent _talkCloud { private set; get; }
        public UnitType _unitType { get { return Type; } }
        public Transform _toolPivot { get { return ToolPivot; } }
        public UnitStorage _storagCompartment { private set; get; }
        public int _maxStorage = 50;

        private GameManager _gameManager;
        public float _movementSpeed { private set; get; }
        public int _searchRadius { private set; get; }
        private float _meeleeRange;
        private ushort _unitWeight;

        private LayerMask _groundLayerMask;
        private Vector3 _delayedDirection;

        private Cell _currentCellBelow;
        private GridDirection _currentDirection;

        //components
        [SerializeField] private Collider _distanceKeeperCollider; //Dirty
        [SerializeField] private GameObject _selectionSprite;

        private Collider _collider;
        private Outline _outline;
        private Rigidbody _rigidbody;
        private FlowField _flowField;

        public OutlineState _lastState { private set; get; }
        private bool _outlineOverruled;

        #region Events
        public delegate void UnitHover(Unit unit);
        public static event UnitHover OnUnitHover;

        public delegate void UnitClick(Unit unit);
        public static event UnitHover OnUnitClick;

        public delegate void UnitExit(Unit unit);
        public static event UnitExit OnUnitExit;

        public delegate void UnitWantsToBeUnselected(Unit unit);
        public static event UnitWantsToBeUnselected OnUnitWantsToBeUnselected;
        #endregion

        [SerializeField] private GameObject _ArrowTip;

        private void Awake()
        {
            _groundLayerMask = 1 << 8;
            _searchRadius = 200;
            _collider = this.GetComponent<Collider>();
            _rigidbody = this.GetComponent<Rigidbody>();
            _outline = this.GetComponent<Outline>();
            _talkCloud = this.GetComponent<TalkCloudComponent>();
            _storagCompartment = new UnitStorage(_maxStorage, this);
            _unitBrain = new UnitBrain(this);

            UnitSelection.OnUnSelectAll += UnSelectUnit;
            SavingLoading.OnPrepareToSave += GetID;
            TipsAndGuidesManager.OnTriggerSelectionArrowTip += OnTriggerSelectionArrow;
        }

        private void Start()
        {
            _unitBrain._targetInformation.SetTargetRotation(new Vector2(this.transform.position.x, this.transform.position.y));
            _gameManager = GameManager._instance;
            //Every unit get's saved in the GroupManager, this is for unit selection
            UnitAdministrator.Instance.SignupNewUnit(this);

            //Set Unit Parameters
            _movementSpeed = UnitTypeDatabase.GetMovementSpeed(Type);
            _meeleeRange = UnitTypeDatabase.GetMeeleeRange(Type);
            _unitWeight = UnitTypeDatabase.GetUnitWeight(Type);
            _flowField = GridController.Instance.GenesisField;
        }

        private void OnDestroy()
        {
            SavingLoading.OnPrepareToSave -= GetID;
            UnitSelection.OnUnSelectAll -= UnSelectUnit;
            TipsAndGuidesManager.OnTriggerSelectionArrowTip -= OnTriggerSelectionArrow;
        }

        private void OnTriggerSelectionArrow(UnitType type)
        {
            if(_unitType == type)
            {
                _ArrowTip.SetActive(true);
                _ArrowTip.transform.DOLocalMoveY(9f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
            }
            else
            {
                _ArrowTip.transform.DOKill();
                _ArrowTip.SetActive(false);
            }
        }

        public void InvokeUnselect()
        {
            OnUnitWantsToBeUnselected?.Invoke(this);
        }

        #region Movement Methods
        /// <summary>
        /// Move unit on flowfield to target. Needs to fire every frame if you want to move unit
        /// </summary>
        /// <param name="unitGroup"></param>
        /// <returns></returns>
        public bool MoveUnit(Vector3 flowfieldTarget, int intergrationLayer)
        {
            if (MoveUnitOnFlowField(flowfieldTarget, intergrationLayer))
                if (MoveUnitToTargetPosition())
                    return true;

            return false;
        }

        public bool MoveUnit()
        {
            if (MoveUnitToTargetPosition())
                return true;

            return false;
        }

        /// <returns>true if action fully performed</returns>
        private bool MoveUnitToTargetPosition()
        {
            SetUnitYposition();
            Vector3 target = new Vector3(_unitBrain._targetInformation.Position().x, transform.position.y, _unitBrain._targetInformation.Position().z);
            if (Vector3.Distance(transform.position, target) > .15f)
            {
                _collider.enabled = false;
                _distanceKeeperCollider.enabled = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

                Vector3 direction = target - transform.position;
                AddVelocityToUnit(direction);
                RotateToHeading();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <param name="flowfield"></param>
        /// <returns>true if action fully performed.</returns>
        private bool MoveUnitOnFlowField(Vector3 targetPosition, int intergrationLayer)
        {
            if (_flowField == null)
                _flowField = GridController.Instance.GenesisField;

            SetUnitYposition();

            float distanceFromUnitToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPosition.x, targetPosition.z));

            if (distanceFromUnitToTarget > (_flowField._cellRadius * 2))
            {
                _distanceKeeperCollider.enabled = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

                Cell cellBelow = _flowField.GetCellFromWorldPos(this.transform.position);
                if (cellBelow != _currentCellBelow)
                {
                    //Reset last cell
                    if (_currentCellBelow != null)
                        _currentCellBelow.OnUnitExit(_unitWeight);
                    //Set new cell
                    _currentCellBelow = cellBelow;
                    _currentCellBelow.OnUnitEnter(_unitWeight);
                    _currentDirection = _flowField.GetBestDirectionOfCell(_currentCellBelow, intergrationLayer);
                }

                if (_currentDirection != null)
                {
                    Vector3 collisionDetection = CollisionDetection();

                    Vector3 vectorDirection = new Vector3(_currentDirection.Vector.x, 0, _currentDirection.Vector.y);

                    _delayedDirection = Vector3.Lerp(_delayedDirection, vectorDirection + collisionDetection, .15f);

                    AddVelocityToUnit(_delayedDirection);

                    RotateToHeading();
                }
                return false;
            }
            return true;
        }

        private Vector3 CollisionDetection()
        {
            Vector3Int hits = Vector3Int.zero;

            //Right ray
            if (Physics.Raycast(transform.TransformPoint(Vector3.right * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, 25.0f, 0) * transform.forward, 1f))
            {
                hits[0] = 1;
                Debug.DrawRay(transform.TransformPoint(Vector3.right * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, 25.0f, 0) * transform.forward, Color.red);
            }
            else
                Debug.DrawRay(transform.TransformPoint(Vector3.right * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, 25.0f, 0) * transform.forward);

            //Forward ray
            if (Physics.Raycast(transform.TransformPoint(Vector3.forward * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, 0, 0) * transform.forward, 1f))
            {
                hits[1] = 1;
                Debug.DrawRay(transform.TransformPoint(Vector3.forward * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, 0, 0) * transform.forward, Color.red);
            }
            else
                Debug.DrawRay(transform.TransformPoint(Vector3.forward * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, 0, 0) * transform.forward);


            //Left ray
            if (Physics.Raycast(transform.TransformPoint(Vector3.left * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, -25.0f, 0) * transform.forward, 1f))
            {
                hits[2] = 1;
                Debug.DrawRay(transform.TransformPoint(Vector3.left * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, -25.0f, 0) * transform.forward, Color.red);
            }
            else
                Debug.DrawRay(transform.TransformPoint(Vector3.left * .1f) + new Vector3(0, .25f, 0), Quaternion.Euler(0, -25.0f, 0) * transform.forward);


            if (hits == new Vector3Int(1, 0, 0))
                return Quaternion.Euler(0, -45f, 0) * transform.forward;

            else if(hits == new Vector3Int(0, 1, 0))
                return Quaternion.Euler(0, 180f, 0) * transform.forward;

            else if(hits == new Vector3Int(0, 0, 1))
                return Quaternion.Euler(0, 45f, 0) * transform.forward;

            else if (hits == new Vector3Int(0, 0, 0))
                return Quaternion.Euler(0, 0, 0) * transform.forward;

            else
                return Quaternion.Euler(0, 180f, 0) * transform.forward;
        }

        private void AddVelocityToUnit(Vector3 vel)
        {
            _rigidbody.velocity = vel.normalized * _movementSpeed;
        }

        private void SetUnitYposition()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 2f, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, _groundLayerMask))
                this.transform.position = hit.point + Vector3.up * (.01f);
        }

        /// <summary>
        /// Rotates the unit towards the velocity heading
        /// </summary>
        /// <param name="direction">velocity of unit</param>
        private void RotateToHeading()
        {
            _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity.normalized);
        }

        public void StopMoving(bool withRotation)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            DOTween.To(() => _rigidbody.velocity, x => _rigidbody.velocity = x, new Vector3(0, 0, 0), .5f);
            _collider.enabled = true;

            if (withRotation)
                transform.eulerAngles = _unitBrain._targetInformation.Rotation();
        }
        #endregion

        #region Visual Methods
        public void SetOutlineState(OutlineState state, bool underrule = default)
        {
            if (state == OutlineState.AboutToBeSelected && !underrule)
            {
                _outlineOverruled = true;
            }
            else if (state == OutlineState.UnSelected && !underrule)
            {
                _outlineOverruled = false;
            }
            else if (state == OutlineState.UnSelected && underrule && _outlineOverruled)
            {
                return;
            }



            switch (state)
            {
                case OutlineState.UnSelected:
                    _lastState = state;
                    _outline.enabled = false;
                    _selectionSprite.SetActive(false);
                    break;
                case OutlineState.Selected:
                    _lastState = state;
                    _outline.OutlineColor = ColourDatabase.instance.Selection;
                    _outline.enabled = true;
                    _selectionSprite.SetActive(true);
                    break;
                case OutlineState.AboutToBeSelected:
                    _outline.OutlineColor = ColourDatabase.instance.Hover;
                    _outline.enabled = true;
                    break;
                default:
                    break;
            }
        }

        public void ReturnToLastOutlineState()
        {
            SetOutlineState(_lastState);
        }
        #endregion

        #region Death Methods
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Projectile"))
            {
                ProjectileDetailsReferencer p = collision.transform.GetComponent<ProjectileDetailsReferencer>();
                if (p.details._source != Type)
                {
                    DamageUnit(p.details._damage);
                    Destroy(p.gameObject);
                }
            }
        }

        private void DamageUnit(int amount)
        {
            _unitBrain._health.ReduceHealth(amount);
            if (_unitBrain._health._health < 0)
                KillUnit();
        }

        private void KillUnit()
        {
            UnitAdministrator.Instance.RemoveUnitFromGame(this);
            Destroy(gameObject);
        }
        #endregion

        #region Mouse interaction methods
        private void OnMouseEnter()
        {
            if (_gameManager._gameState != GameState.PAUSED && _gameManager._gameState != GameState.CUTSCENE)
            {
                OnUnitHover?.Invoke(this);
                if (_lastState != OutlineState.Selected)
                {
                    SetOutlineState(OutlineState.AboutToBeSelected, true);
                }
            }
        }

        public void OnMouseDown()
        {
            if (_gameManager._gameState != GameState.PAUSED && _gameManager._gameState != GameState.CUTSCENE)
            {
                OnUnitClick?.Invoke(this);
                if (_ArrowTip.activeSelf)
                    TipsAndGuidesManager._instance.SelectedUnit();
            }
        }

        public void GetID()
        {
            string x = _ID;
        }

        private void OnMouseExit()
        {
            OnUnitExit?.Invoke(this);
            if (_lastState != OutlineState.Selected)
            {
                SetOutlineState(OutlineState.UnSelected, true);
            }
        }

        private void UnSelectUnit(bool notUsed)
        {
            SetOutlineState(OutlineState.UnSelected);
        }
        #endregion

        #region SaveData
        public object CaptureState()
        {
            SaveData data = new SaveData();
            data.GetGeneralData(this);
            data.GetHealthData(_unitBrain._health);
            data.GetMemoryData(_unitBrain._memory);
            data.GetResourceRequestData(_unitBrain._memory);
            data.GetStorageData(_storagCompartment);
            return data;
        }
        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            _unitBrain._health.LoadHealth(saveData._healthData._health, 
                saveData._healthData._energy, 
                saveData._healthData._hunger);

            _unitBrain._memory.LoadMemory(saveData._memoryData._homeID,
                saveData._memoryData._workplaceID,
                saveData._memoryData._lastInteractedID,
                saveData._memoryData._behavourState,
                saveData._memoryData._hasFreeWill);

            if (saveData._hasResourceRequest)
                _unitBrain._memory._resourceQuest = new ResourceQuest((ResourceType)saveData._resourceQuestData._resourceType,
                    saveData._resourceQuestData._amount,
                    saveData._resourceQuestData._withdrawalResource,
                    BuildBuildingsLibrary.Instance.GetWorkplaceWithID(saveData._resourceQuestData._taskBuildingID));

            for (int i = 0; i < saveData._storageData.Count; i++)
            {
                Debug.Log("Restore");
                this._unitBrain.VerifyStorageOfUnit((ResourceType)saveData._storageData[i]._type);
                this._storagCompartment.DepositResource(saveData._storageData[i]._amount, (ResourceType)saveData._storageData[i]._type);
            }

            _unitBrain._targetInformation.SetTargetPosition(new Vector3(saveData._generalData._targetPositionX,
                saveData._generalData._targetPositionY,
                saveData._generalData._targetPositionZ));

            _unitBrain._targetInformation.SetTargetRotation(new Vector3(saveData._generalData._targetRotationX,
                saveData._generalData._targetRotationY,
                saveData._generalData._targetRotationZ));
        }

        [System.Serializable] private struct SaveData
        {
            public GeneralData _generalData;
            public HealthData _healthData;
            public MemoryData _memoryData;
            public ResourceQuestData _resourceQuestData;
            public List<StorageData> _storageData;
            public bool _hasResourceRequest;

            public void GetGeneralData(Unit unit)
            {
                _generalData = new GeneralData
                {
                    _unitType = (int)unit._unitType,

                    _targetPositionX = unit._unitBrain._targetInformation.Position().x,
                    _targetPositionY = unit._unitBrain._targetInformation.Position().y,
                    _targetPositionZ = unit._unitBrain._targetInformation.Position().z,

                    _targetRotationX = unit._unitBrain._targetInformation.Rotation().x,
                    _targetRotationY = unit._unitBrain._targetInformation.Rotation().y,
                    _targetRotationZ = unit._unitBrain._targetInformation.Rotation().z,
                };
            }

            public void GetHealthData(Health unitHealth)
            {
                _healthData = new HealthData
                {
                    _health = unitHealth._health,
                    _energy = unitHealth._fineEnergy,
                    _hunger = unitHealth._fineHunger,
                };
            }

            public void GetMemoryData(Memory unitMemory)
            {
                MemoryData data = new MemoryData();
                if (unitMemory._home != null)
                    data._homeID = unitMemory._home._ID;
                else
                    data._homeID = "";

                if (unitMemory._workplaceBuilding != null)
                    data._workplaceID = unitMemory._workplaceBuilding._ID;
                else
                    data._workplaceID = "";

                if (unitMemory._lastInteractedResource != null)
                    data._lastInteractedID = unitMemory._lastInteractedResource._ID;
                else
                    data._lastInteractedID = "";

                data._behavourState = (int)unitMemory._behaviourState;
                data._hasFreeWill = unitMemory._hasFreeWill;
            }

            public void GetResourceRequestData(Memory unitMemory)
            {
                if(unitMemory._resourceQuest != null)
                {
                    _hasResourceRequest = true;
                    ResourceQuestData data = new ResourceQuestData();
                    data._resourceType = (int)unitMemory._resourceQuest._resourceType;
                    data._amount = unitMemory._resourceQuest._amount;
                    data._taskBuildingID = unitMemory._resourceQuest._taskGivingBuilding._ID;
                    data._onReturn = unitMemory._resourceQuest._onReturn;
                }
                else
                {
                    _hasResourceRequest = false;
                }
            }

            public void GetStorageData(UnitStorage storageInformation)
            {
                _storageData = new List<StorageData>();
                foreach (StockpileInformation item in storageInformation._storage)
                {
                    StorageData storage = new StorageData
                    {
                        _amount = item._currentStockAmount,
                        _max = item._max,
                        _type = (int)item._resourceType
                    };
                    _storageData.Add(storage);
                }
            }

            [System.Serializable] public struct GeneralData
            {
                public int _unitType;
                #region Targets
                public float _targetPositionX;
                public float _targetPositionY;
                public float _targetPositionZ;

                public float _targetRotationX;
                public float _targetRotationY;
                public float _targetRotationZ;
                #endregion
            }
            [System.Serializable] public struct HealthData
            {
                public int _health;
                public float _energy;
                public float _hunger;
            }
            [System.Serializable] public struct MemoryData
            {
                public string _homeID;
                public string _workplaceID;
                public string _lastInteractedID;
                public int _behavourState;
                public bool _hasFreeWill;
            }
            [System.Serializable] public struct ResourceQuestData
            {
                public bool _withdrawalResource;
                public int _resourceType;
                public int _amount;
                public string _taskBuildingID;
                public bool _onReturn;
            }
            [System.Serializable] public struct StorageData
            {
                public int _amount;
                public int _max;
                public int _type;
            }
        }
        #endregion

        public void DropResource(int amount, ResourceType type, bool toMouse)
        {
            _unitBrain._behaviourModule._animationController.TriggerResourceThrow();
            _storagCompartment.WithdrawalResource(amount, type);
            Vector3 position = this.transform.position;
            Quaternion rotation = this.transform.rotation;

            if (toMouse)
            {
                Vector3 mouseWorldSpace = HelperFunctions.GetMousePositionIn3D(Input.mousePosition, _groundLayerMask);
                if (position.y < .85f)
                    position[1] = .85f;

                Vector3 dir = mouseWorldSpace - this.transform.position;
                Debug.Log(mouseWorldSpace);
                float distance = Vector3.Distance(this.transform.position, mouseWorldSpace);

                position = this.transform.position + dir * .4f;
            }

            StartCoroutine(DropResourceDelay(amount, type, position, rotation, 1f));
        }

        private IEnumerator DropResourceDelay(int amount, ResourceType type, Vector3 position, Quaternion rotation, float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject stack = Instantiate(ResourceDatabase.GetResourceStackObject(type),
                position,
                rotation,
                null);

            ResourceStack resourceStack = stack.GetComponent<ResourceStack>();
            resourceStack._amountOfResources = amount;
            resourceStack._resourceInformation = ResourceDatabase.GetResourceInformation(type);
        }
    }

    public class UnitStorage 
    {
        private Unit _unit;
        private List<StockpileInformation> storage;
        private int maxStorage;
        public IReadOnlyCollection<StockpileInformation> _storage 
        { 
            get 
            {
                return storage;
            } 
        }

        public UnitStorage(int maxStorage, Unit unit)
        {
            _unit = unit;
            this.maxStorage = maxStorage;
            storage = new List<StockpileInformation>();
        }

        public int DepositResource(int amount, ResourceType type)
        {
            if (amount == 0) return 0;

            foreach (StockpileInformation s in storage)
            {
                if(s._resourceType == type)
                {
                    int l = s.DepositResource(amount);
                    Debug.Log("Deposit: " + amount +" " + type +" to unit | Overdraft: " + l);
                    return l;
                }
            }
            Debug.Log("Failed to deposit: " + amount +" "+ type + " to Unit");
            return amount;
        }

        public int WithdrawalResource(int amount, ResourceType type)
        {
            foreach (StockpileInformation s in storage)
            {
                if (s._resourceType == type)
                {
                    int w = s.WithdrawalResource(amount);
                    Debug.Log("Withdrawn: " + w + " "+ type +" from unit");
                    return w;
                }
            }
            Debug.Log("Failed to withdrawal: " + type + " from Unit");
            return 0;
        }

        public void CreateNewStorage(ResourceType type)
        {
            storage.Add(new StockpileInformation(maxStorage, type, false));
        }

        public void OnFullStorage(ResourceInformation information)
        {
            switch (_unit._unitType)
            {
                case UnitType.Harvester:
                    //Find storage for resource
                    _unit._unitBrain._behaviourModule.StoreHarvestedResource(information._resourceType);
                    _unit._unitBrain._targetInformation.ResetTargetTransform();
                    break;
                case UnitType.Builder:
                    break;
                case UnitType.Worker:
                    break;
                default:
                    break;
            }
        }

        public StockpileInformation GetMostNutritionalStockFromUnitStorage()
        {
            int nutritionalValue = 0;
            StockpileInformation mostNutritionalStock = null;
            foreach (StockpileInformation item in _storage)
            {
                ResourceInformation x = ResourceDatabase.GetResourceInformation(item._resourceType);
                if (x._consumable && x._nutritionalValue > nutritionalValue)
                {
                    if (item._currentStockAmount > 0)
                    {
                        nutritionalValue = x._nutritionalValue;
                        mostNutritionalStock = item;
                    }
                }
            }
            return mostNutritionalStock;
        }
    }
}