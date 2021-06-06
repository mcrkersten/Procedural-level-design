using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitsAndFormation {
    public class BuildingPlacer : MonoBehaviour {

        private GameManager _gameManager;
        public Color _validColor;

        [SerializeField]
        private float _rotationAmount;

        private bool _isPlacing;
        [SerializeField]
        private GameObject _buildingPositioner;

        //Current building
        private GameObject _building;
        private Buildingcomponents _buildingComponents;
        private AnimationFunctions _buildingAnimations;
        private InteractableInformation _currentItem;

        //System
        private ResourceManager _resourceManager;

        [SerializeField]
        private LayerMask _layerMask;
        [SerializeField]
        private Camera _cam;
        private bool _clickStartedInCorrectGamestatus;

        //Dynamic
        private bool _isOverlappingGrid;
        private bool _isSnapped;
        private bool _toSteep;

        [SerializeField]
        private List<GameObject> _currentIntersectingObject = new List<GameObject>();

        public delegate void BuildingPlacement(CursorType type);
        public static event BuildingPlacement OnBuildingPlacement;

        private void Start()
        {
            _cam = Camera.main;
            BuildMenuButton.OnItemSelected += StartBuildingPlacement;
            CancelButton.OnCancelBuildButtonClick += CancelBuild;

            InputManager.OnStartMousepress += OnMouseStart;
            InputManager.OnMousePress += OnMousePress;
            InputManager.OnMouseRelease += OnMouseRelease;
            InputManager.OnScrollWheelMovement += OnScrollWheelMovement;

            _resourceManager = ResourceManager.Instance;
            _gameManager = GameManager._instance;
        }

        #region PlayerInput
        void OnMouseStart(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 0) return;
            if (_state != InputState.BUILDING_PLACEMENT) return;
            _clickStartedInCorrectGamestatus = true;
        }

        void OnMousePress(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 0) return;
            if (_state != InputState.BUILDING_PLACEMENT) return;
        }

        void OnMouseRelease(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 0) return;
            if (_state != InputState.BUILDING_PLACEMENT) return;
            if (_clickStartedInCorrectGamestatus)
                PlaceBuilding();
            _clickStartedInCorrectGamestatus = false;
        }

        private void OnScrollWheelMovement(float movement, KeyCode type, InputState _state)
        {
            if (type != KeyCode.LeftAlt) return;
            RotateBuilding(movement);
        }
        #endregion

        #region PlayerInput-Methods
        /// <summary>
        /// Places de building on the map.
        /// </summary>
        private void PlaceBuilding()
        {
            if (!IsOverlapping())
            {
                _buildingComponents._triggerCollider.enabled = true;
                _building.transform.parent = null;
                _buildingComponents.PopulateWorldSnapPoints();
                _building = null;

                //Spawns new object under the cursor to be placed.
                _buildingComponents._construction.StartConstruction();
                StartBuildingPlacement(_currentItem);
            }
            else
            {
                //Animation or sound
            }
        }

        private void RotateBuilding(float movement)
        {
            _buildingPositioner.transform.eulerAngles = new Vector3(0, _buildingPositioner.transform.eulerAngles.y + (_rotationAmount * movement), 0);
        }
        #endregion

        ///

        #region Trigger
        private void OnTriggerEnter(Collider other)
        {
            OnStartOverlapObject(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            OnOverlapObject(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnEndOverlapObject(other.gameObject);
        }
        #endregion

        #region Trigger-Methods
        private void OnStartOverlapObject(GameObject overlap)
        {
            //IgnoreGround
            if (!overlap.CompareTag("Ground") && !overlap.CompareTag("GridTrigger"))
            {
                _currentIntersectingObject.Add(overlap);
            }
        }

        private void OnOverlapObject(GameObject overlap)
        {
            if (overlap.CompareTag("GridTrigger"))
            {
                _isOverlappingGrid = true;
                Buildingcomponents bc = overlap.transform.root.GetComponent<Buildingcomponents>();
                SnapToPoint(bc);
                CheckReleaseFromPoint();
            }
        }

        private void OnEndOverlapObject(GameObject overlap)
        {
            //IgnoreGround
            if (!overlap.CompareTag("Ground") && !overlap.CompareTag("GridTrigger"))
                _currentIntersectingObject.Remove(overlap);

            if (overlap.CompareTag("GridTrigger"))
                _isOverlappingGrid = false;
        }

        #endregion


        private void Update()
        {
            if (_isPlacing)
            {
                SetValidityColor();
                SetToolTip();
            }
            else
            {
                _currentIntersectingObject = new List<GameObject>();
            }
            if(_buildingComponents != null)
            {
                UpdatePositionPositioner();             
            }
        }

        /// <summary>
        /// Starts building process. Updates variables and sets _buildingPositioner from Building Item.
        /// </summary>
        /// <param name="item"></param>
        private void StartBuildingPlacement(InteractableInformation item)
        {
            InputManager.Instance.ChangeInputState(InputState.BUILDING_PLACEMENT);

            //Spawn
            _currentItem = item;
            _building = Instantiate(_currentItem._constructionPrefab, Vector3.zero, Quaternion.identity, _buildingPositioner.transform);
            _building.GetComponent<UnitInteractable>()._interactableID = _currentItem._interactableID;
            _building.transform.localPosition = Vector3.zero;
            _building.transform.localRotation = Quaternion.identity;

            //Initialize
            _buildingComponents = _building.GetComponent<Buildingcomponents>();
            _buildingComponents._fence.SetActive(false);
            _buildingComponents._interactableInformation = item;
            _buildingComponents.PopulateLocalSnapPoints();
            _buildingAnimations = _building.GetComponent<AnimationFunctions>();

            //Cursor point
            OnBuildingPlacement?.Invoke(CursorType.Placement);

            _isPlacing = true;
            _isSnapped = false;
        }

        /// <summary>
        /// Sets _BuildingPositioner on 3D position of mouse on the map.
        /// </summary>
        private void UpdatePositionPositioner()
        {
            if(!_isSnapped)
            {
                Vector3 position = HelperFunctions.GetMousePositionIn3D(Input.mousePosition, _layerMask);
                if(position.y < .85f)
                    position[1] = .85f;

                _buildingPositioner.transform.position = position;


                RaycastHit ray;
                if(Physics.Raycast(_buildingPositioner.transform.position + new Vector3(0,50,0), transform.TransformDirection(Vector3.down), out ray, Mathf.Infinity, _layerMask))
                {
                    if (ray.normal.y < .85f)
                    {
                        _toSteep = true;
                    }
                    else
                    {
                        _toSteep = false;
                    }
                }
            }
        }

        private void SnapToPoint(Buildingcomponents otherBuilding)
        {
            if (_isOverlappingGrid)
            {
                Vector3 point = Vector3.zero;
                if (!_isSnapped)
                {
                    point = GetBestPointPosition(otherBuilding);
                }

                if (Vector3.Distance(point, _buildingPositioner.transform.position) < .8f)
                {
                    _isSnapped = true;
                     _buildingPositioner.transform.position = point;
                }
            }
        }

        private Vector3 GetBestPointPosition(Buildingcomponents otherBuilding)
        {
            List<Vector3> worldPoints = new List<Vector3>();
            foreach (Vector2 buildingPoint in _buildingComponents._buildingPoints)
            {
                Vector3 temp = _buildingComponents.transform.TransformPoint(new Vector3(buildingPoint.x, 0, buildingPoint.y));
                worldPoints.Add(temp);
                Debug.DrawLine(temp, new Vector3(temp.x, 5, temp.z), Color.yellow);
            }

            Vector3 closestPointOfThisBuilding = Vector3.zero;
            float smallestDistance = float.MaxValue;
            foreach (Vector3 point in worldPoints)
            {
                float tempDistance = Vector3.Distance(point, new Vector3(otherBuilding.transform.position.x, 0, otherBuilding.transform.position.z));
                if (tempDistance < smallestDistance)
                {
                    smallestDistance = tempDistance;
                    closestPointOfThisBuilding = point;
                }
            }

            Vector3 closestPointOfOtherBuilding = Vector3.zero;
            smallestDistance = float.MaxValue;
            foreach (Vector3 otherBuildingPoint in otherBuilding.WorldPositions)
            {
                float tempDistance = Vector3.Distance(otherBuildingPoint, closestPointOfThisBuilding);
                if (tempDistance < smallestDistance)
                {
                    smallestDistance = tempDistance;
                    closestPointOfOtherBuilding = otherBuildingPoint;
                }
            }
            Vector3 loc = _buildingComponents.transform.position - closestPointOfThisBuilding;
            return closestPointOfOtherBuilding + loc;
        }

        private void CheckReleaseFromPoint()
        {
            //If the building snapped to a point, the buidling is on a static position and the mouse is not. 
            //We calculate the distance between the mouse and the building.
            //If the distance is great enough, we want to un-snap from the point.
            float distOfMouse = Vector3.Distance(HelperFunctions.GetMousePositionIn3D(Input.mousePosition, _layerMask), _buildingPositioner.transform.position);
            if (distOfMouse > .65f)
            {
                _isSnapped = false;
            }
        }

        /// <summary>
        /// Sets outlineColor based on Validity of position
        /// </summary>
        private void SetValidityColor()
        {
            if (!IsOverlapping() && !_toSteep)
            {
                _buildingComponents._model.SetActive(true);
                _buildingComponents._unValidModel.SetActive(false);
            }
            else
            {
                _buildingComponents._unValidModel.SetActive(true);
                _buildingComponents._model.SetActive(false);
            }
        }

        private void SetToolTip()
        {
            if (IsOverlapping())
            {
                TooltipSystem.Show("Overlapping objects");
            }
            else
            {
                TooltipSystem.Hide();
            }
        }

        private bool IsOverlapping()
        {
            if (_currentIntersectingObject.Count == 0)
                return false;
            else
                return true;
        }

        private void CancelBuild()
        {
            InputManager.Instance.ChangeInputState(InputState.UNIT_MOVEMENT);
            Destroy(_building);
            
            _isPlacing = false;
            TooltipSystem.Hide();
            OnBuildingPlacement?.Invoke(CursorType.None);
        }

        private void OnDestroy()
        {
            BuildMenuButton.OnItemSelected -= StartBuildingPlacement;
            CancelButton.OnCancelBuildButtonClick -= CancelBuild;

            InputManager.OnStartMousepress -= OnMouseStart;
            InputManager.OnMousePress -= OnMousePress;
            InputManager.OnMouseRelease -= OnMouseRelease;
        }
    }
}
