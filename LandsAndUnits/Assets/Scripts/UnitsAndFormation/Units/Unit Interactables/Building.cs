using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;
using DG.Tweening;
public class Building : UnitInteractable, ISaveable
{
    [SerializeField] protected bool _isLoaded;
    [HideInInspector] public Buildingcomponents _components;
    [HideInInspector] public List<OccupancyInformation> _occupancyInformation = new List<OccupancyInformation>();
    [HideInInspector] public List<StockpileInformation> _stockpiles = new List<StockpileInformation>();
    [HideInInspector] public List<StockpileInformation> _producedStockStockpiles = new List<StockpileInformation>();

    public delegate void UnitOpensMenu(UnitInteractable x);
    public static event UnitOpensMenu OnUnitMenuOpen;

    public List<Unit> _unitsInsideBuilding = new List<Unit>();
    protected TooltipTrigger _tooltip;

    protected ObjectWarningType _currentWarningType;
    protected GameObject _instantiatedWarningSymbol;
    protected int _stockCount = 0;
    [HideInInspector] public bool _isShipWreck = false;

    new protected virtual void Awake()
    {
        _targetTransform = this.transform;
        base.Awake();
        _tooltip = GetComponentInChildren<TooltipTrigger>();
        if(_tooltip == null)
            _tooltip = GetComponent<TooltipTrigger>();

        _components = GetComponent<Buildingcomponents>();
        DemolitionButton.OnStartDemolition += OnDemolitionActivate;
        CancelButton.OnCancelDemolitionButtonClick += OnDemolitionDeactivate;

        if (_isLoaded)
        {
            SetBuildingParamiters();
        }
    }

    private void SetBuildingParamiters()
    {
        _gridSize = _components._interactableInformation._size;
        _components._fence.SetActive(false);
        _components._unValidModel.SetActive(false);
        _components._model.SetActive(true);
        _components._constructionCollider.enabled = false;
        _components._buildingCollider.enabled = true;
        _components._model.transform.localPosition = new Vector3(0, 0, 0);
        base.OnPlacement();
    }

    public virtual bool RequestEnter(Unit unit, out OccupancyType type)
    {
        Debug.Log(unit + " Requested enter to: " + this);
        foreach (OccupancyInformation c in _occupancyInformation)
        {
            if (c._occupancyType != OccupancyType.Visitor && c._occupants.Contains(unit))
            {
                _unitsInsideBuilding.Add(unit);
                Debug.Log(c._occupancyType + " " + unit + " granted enter to: " + this);
                type = c._occupancyType;
                return true;
            }

            if (c._occupancyType == OccupancyType.Visitor)
            {
                if (c._occupants.Count < c._max)
                {
                    c._occupants.Add(unit);
                    _unitsInsideBuilding.Add(unit);
                    Debug.Log(c._occupancyType + " " + unit + " Granted enter to: " + this);
                    type = c._occupancyType;
                    return true;
                }
            }
        }

        Debug.Log(unit + " Denied enter to: " + this);
        type = OccupancyType.None;
        return false;
    }

    public virtual void ExitBuilding(Unit unit)
    {
        foreach (OccupancyInformation c in _occupancyInformation)
        {
            if (c._occupancyType == OccupancyType.Visitor)
            {
                c._occupants.Remove(unit);
            }
        }

        if (_unitsInsideBuilding.Contains(unit))
        {
            _unitsInsideBuilding.Remove(unit);
        }
    }

    public virtual void AssignOccupant(Unit unit, OccupancyType occupancyType)
    {
        foreach (OccupancyInformation information in _occupancyInformation)
        {
            if (information._occupancyType == occupancyType)
            {
                information.AddOccupant(unit);
            }
        }
    }

    protected virtual void ChangeWarningSymbol(ObjectWarningType warning)
    {
        if (_instantiatedWarningSymbol != null)
            Destroy(_instantiatedWarningSymbol);

        _currentWarningType = warning;
        _instantiatedWarningSymbol = Instantiate(WarningDatabase.GetPrefab(_currentWarningType), this.transform);
    }

    protected virtual void CreateOccupancyInformation()
    {

    }

    public virtual void UpdateOccupancyInformation()
    {

    }

    protected virtual void CreateStockpileInformation()
    {

    }

    public virtual void UpdateStockpileInformation()
    {

    }

    /// <summary>
    /// Deposit resource in to storage of building.
    /// </summary>
    /// <param name="unit"></param>
    public virtual void DepositInToStorage(Unit unit)
    {
        foreach (StockpileInformation storage in _stockpiles)
        {
            int fund = unit._storagCompartment.WithdrawalResource(unit._unitBrain._memory._resourceQuest._amount, storage._resourceType);
            Debug.Log("Deposited: " + fund + " to storage");
            int overdraft = storage.DepositResource(fund);
            unit._storagCompartment.DepositResource(overdraft, storage._resourceType);
        }
    }

    /// <summary>
    /// Withdraw from storage of building
    /// </summary>
    /// <param name="unit"></param>
    public void WithdrawalFromStorage(Unit unit)
    {
        foreach (StockpileInformation storage in _stockpiles)
        {
            if (unit._unitBrain._memory._resourceQuest._resourceType == storage._resourceType)
            {
                int fund = storage.WithdrawalResource(unit._unitBrain._memory._resourceQuest._amount);
                Debug.Log("Withdrawn: " + fund + " from storage");
                int overdraft = unit._storagCompartment.DepositResource(fund, unit._unitBrain._memory._resourceQuest._resourceType);
                storage.DepositResource(overdraft);
            }
        }
    }

    /// <summary>
    /// Withdraw from produced storage of building
    /// </summary>
    /// <param name="unit"></param>
    /// <returns>If withdrawal was succesfull</returns>
    public virtual bool WithdrawalFromProducedStorage(Unit unit)
    {
        int fund = 0;
        foreach (StockpileInformation storage in _producedStockStockpiles)
        {
            if (unit._unitBrain._memory._resourceQuest._resourceType == storage._resourceType)
            {
                fund = storage.WithdrawalResource(unit._unitBrain._memory._resourceQuest._amount);
                Debug.Log("Withdrawn: " + fund + " from produced storage");
                int overdraft = unit._storagCompartment.DepositResource(fund, unit._unitBrain._memory._resourceQuest._resourceType);
                storage.DepositResource(overdraft);
            }
        }

        if(fund != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void DeactivateWarningSymbol()
    {
        if (_instantiatedWarningSymbol != null)
            Destroy(_instantiatedWarningSymbol);
    }

    public override void OnMouseEnter()
    {
        if (InputManager.Instance?._inputState == InputState.MENU_HOVER)
            return;

        if (_gameManager._gameState != GameState.PAUSED)
        {
            base.OnMouseEnter();
            if (DemolitionSystem._demolitionActive)
                SendCursorDelegate(CursorType.Demolition);

            if (_outlineState != OutlineState.Selected)
            {
                SetOutlineState(OutlineState.AboutToBeSelected);
                SendInteractableDelegate(this, OutlineState.AboutToBeSelected);
            }
        }
    }

    public override void OnMouseUp()
    {
        if (InputManager.Instance?._inputState == InputState.MENU_HOVER)
            return;

        if (!DemolitionSystem._demolitionActive)
            base.OnMouseUp();
        else
        {
            ScheduleForDestruction();
            TipsAndGuidesManager._instance.AdvanceGuide(Guide.DEMOLISH, 2);
            TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.DEMOLISH, 1);
        }
    }

    public override bool DestroyInteractableAction()
    {
        DestroyInteractable();
        TipsAndGuidesManager._instance.AdvanceGuide(Guide.DEMOLISH, 3);
        TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.DEMOLISH, 2);
        isDemolished = true;
        return true;
    }

    protected override void DestroyInteractable()
    {
        _components._buildingCollider.enabled = false;

        if(this._type != InteractableType.Construction)
        {
            foreach (StorageInformation cost in _components._interactableInformation._costs)
            {
                GameObject stack = null;
                stack = Instantiate(ResourceDatabase.GetResourceStackObject(cost._resourceType),
                    GetStackPosition(),
                    this.transform.rotation,
                    null);

                ResourceStack resourceStack = stack.GetComponent<ResourceStack>();
                resourceStack._amountOfResources = cost._maxStorage;
                resourceStack._resourceInformation = ResourceDatabase.GetResourceInformation(cost._resourceType);
            }
        }

        foreach (StockpileInformation stock in _stockpiles)
        {
            if(stock._currentStockAmount > 0)
            {
                GameObject stack = Instantiate(ResourceDatabase.GetResourceStackObject(stock._resourceType),
                    GetStackPosition(),
                    this.transform.rotation,
                    null);

                ResourceStack resourceStack = stack.GetComponent<ResourceStack>();
                resourceStack._amountOfResources = stock._currentStockAmount;
                resourceStack._resourceInformation = ResourceDatabase.GetResourceInformation(stock._resourceType);
            }
        }

        ChangeWarningSymbol(ObjectWarningType.None);
        _components._model.transform.DOMoveY(-3f, 7f).SetEase(Ease.OutCubic);
        _components._fence.transform.DOLocalMoveY(-3f, 7f).SetEase(Ease.OutCubic);
        _components._model.transform.DOLocalRotate(new Vector3(0, 0, -40f), 7.1f).SetEase(Ease.OutCubic).OnComplete(base.DestroyInteractable);
    }

    public override void ScheduleForDestruction()
    {
        base.ScheduleForDestruction();
        OnDemolitionActivate();
        if (this._type != InteractableType.Construction)
        {
            Unit[] toExit = _unitsInsideBuilding.ToArray();
            for (int i = 0; i < toExit.Length; i++)
                toExit[i]._unitBrain._behaviourModule.ExitBuilding();

            _components._fence.SetActive(true);
            _components._fence.transform.DOLocalMoveY(0f, .7f).SetEase(Ease.InBounce);
            ChangeWarningSymbol(ObjectWarningType.ScheduledForDestruction);
        }
    }

    protected Vector3 GetStackPosition()
    {
        if (_stockpiles.Count == 0 && _components._interactableInformation._costs.Count == 1)
        {
            return this.transform.position;
        }

            if (_stockCount < _cells.Count)
        {
            _stockCount++;
            return _cells[_stockCount]._worldPosition;
        }
        else
        {
            Vector3 offset = Vector3.zero;
            switch (_stockCount - _cells.Count)
            {
                case 0:
                    offset = new Vector3(.5f, 0, 0);
                    break;
                case 1:
                    offset = new Vector3(-.5f, 0, 0);
                    break;
                case 2:
                    offset = new Vector3(0, 0, .5f);
                    break;
                case 3:
                    offset = new Vector3(0, 0, -.5f);
                    break;
                default:
                    break;
            }

            _stockCount++;
            return _cells[_stockCount]._worldPosition + offset;
        }
    }

    private void OnDemolitionActivate()
    {
        _components._model.SetActive(false);
        _components._unValidModel.SetActive(true);
        if (!_scheduledForDestruction)
        {
            int numOfChildren = _components._unValidModel.transform.childCount;
            for (int i = 0; i < numOfChildren; i++)
            {
                Renderer[] children;
                children = _components._unValidModel.transform.GetChild(i).GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in children)
                {
                    var mats = new Material[rend.materials.Length];
                    for (var j = 0; j < rend.materials.Length; j++)
                    {
                        mats[j] = ColourDatabase.instance.NotScheduledForDemolition;
                    }
                    rend.materials = mats;
                }
            }
        }
        else
        {
            int numOfChildren = _components._unValidModel.transform.childCount;
            for (int i = 0; i < numOfChildren; i++)
            {
                Renderer[] children;
                children = _components._unValidModel.transform.GetChild(i).GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in children)
                {
                    var mats = new Material[rend.materials.Length];
                    for (var j = 0; j < rend.materials.Length; j++)
                    {
                        mats[j] = ColourDatabase.instance.ScheduledForDemolition;
                    }
                    rend.materials = mats;
                }
            }
        }
    }

    private void OnDemolitionDeactivate()
    {
        _components._model.SetActive(true);
        _components._unValidModel.SetActive(false);
    }

    protected override void OnDestroy()
    {
        DemolitionButton.OnStartDemolition -= OnDemolitionActivate;
        CancelButton.OnCancelDemolitionButtonClick -= OnDemolitionDeactivate;
        base.OnDestroy();
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.GetUnitData(_occupancyInformation, _unitsInsideBuilding);
        data.GetStockpileData(_stockpiles);
        return data;
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        Building building = this;

        #region Restore UnitData
        for (int i = 0; i < saveData._unitData.Count; i++)
        {
            List<Unit> _units = new List<Unit>();
            Unit unit = UnitAdministrator.Instance._units.SingleOrDefault(u => u._ID == saveData._unitData[i]._unitID);
            building.AssignOccupant(unit, (OccupancyType)saveData._unitData[i]._occupantType);
            if (saveData._unitData[i]._isInside)
                unit.gameObject.SetActive(false);
        }
        #endregion

        #region Restore stockpileData
        for (int i = 0; i < saveData._stockpileData.Count; i++)
        {
            int amount = saveData._stockpileData[i]._amount;
            ResourceType type = (ResourceType)saveData._stockpileData[i]._type;

            if (!saveData._stockpileData[i]._isProduced)
            {
                foreach (StockpileInformation stockpile in _stockpiles)
                {
                    if (stockpile._resourceType == type)
                        stockpile.DepositResource(amount);
                }
            }
            else
            {
                foreach (StockpileInformation stockpile in _producedStockStockpiles)
                {
                    if (stockpile._resourceType == type)
                        stockpile.DepositResource(amount);
                }
            }
        }
        #endregion
    }

    [System.Serializable] private struct SaveData
    {
        #region UnitData
        public List<UnitData> _unitData;
        public void GetUnitData(List<OccupancyInformation> occupancy, List<Unit> inside)
        {
            _unitData = new List<UnitData>();
            foreach (OccupancyInformation information in occupancy)
            {
                foreach (Unit u in information._occupants)
                {
                    UnitData data = new UnitData();
                    data._unitID = u._ID;
                    data._occupantType = (int)information._occupancyType;
                    if (inside.Contains(u)) { data._isInside = true; }
                    else { data._isInside = false; }
                    _unitData.Add(data);
                }
            }
        }
        [System.Serializable] public struct UnitData
        {
            public string _unitID;
            public bool _isInside;
            public int _occupantType;
        }
        #endregion

        #region StockpileData
        public List<StockpileData> _stockpileData;
        public void GetStockpileData(List<StockpileInformation> stockpiles)
        {
            _stockpileData = new List<StockpileData>();
            foreach (StockpileInformation pile in stockpiles)
            {
                StockpileData data = new StockpileData
                {
                    _type = (int)pile._resourceType,
                    _amount = pile._currentStockAmount,
                    _isProduced = pile._isProduced
                };
                _stockpileData.Add(data);
            }
        }
        [System.Serializable] public struct StockpileData
        {
            public int _type;
            public int _amount;
            public bool _isProduced;
        }
        #endregion
    }
}

public class OccupancyInformation
{
    public int _max;
    public OccupancyType _occupancyType;
    public List<Unit> _occupants = new List<Unit>();

    public OccupancyInformation(int maxCapacity, OccupancyType title)
    {
        _max = maxCapacity;
        _occupancyType = title;
    }

    public void AddOccupant(Unit unit)
    {
        if (_occupants.Count < _max)
        {
            if (!_occupants.Contains(unit))
            {
                _occupants.Add(unit);
            }
        }
    }

    public void RemoveOccupant(Unit unit)
    {
        if (_occupants.Contains(unit))
        {
            _occupants.Remove(unit);
        }
    }
}
public class StockpileInformation
{
    public int _currentStockAmount { private set; get; }
    public int _max;
    public ResourceType _resourceType;
    public bool _isProduced;


    public StockpileInformation(int maxCapacity, ResourceType type, bool isProduced)
    {
        _max = maxCapacity;
        _resourceType = type;
        _isProduced = isProduced;
    }

    public int DepositResource(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(_currentStockAmount + 1 <= _max)
                _currentStockAmount += 1;
            else
                return amount - i;
        }
        return 0;
    }

    public bool StockCheck(int amount)
    {
        if(_currentStockAmount >= amount)
            return true;
        
        return false;
    }

    public int WithdrawalResource(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_currentStockAmount > 0)
                _currentStockAmount -= 1;
            else
                return i;
        }
        return amount;
    }
}
