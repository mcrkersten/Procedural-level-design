using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public class UnitInput : MonoBehaviour
    {
        //References
        private GroupManager _groupManager;
        private Camera _mainCamera;

        //MousePositions
        private Vector3 _startMousePosition;
        private Vector3 _currentMousePosition;

        private Vector3 _startMousePositionIn3D;
        private Vector3 _currentMousePositionIn3D;

        //Settings
        [SerializeField]
        private LayerMask _groundLayerMask;
        [SerializeField]
        private float _scale;

        //Visuals
        [SerializeField]
        private GameObject _positionVisualPrefab;
        private GameObject _base;
        [SerializeField]
        private GameObject _arrowSpritePrefab;
        [SerializeField]
        private GameObject _selectionSpritePrefab;

        private List<Vector3> _worldUnitPositions = new List<Vector3>();
        private List<Transform> _localUnitPositions = new List<Transform>();
        private SelectionSphere _selectionSphere;

        private FormationType _formationType;

        [SerializeField]
        private LayerMask _unitLayerMask;
        private bool _startedAction;

        private bool _cancelInput;



        private void Awake()
        {
            InputManager.OnStartMousepress += OnMouseStart;
            InputManager.OnMousePress += OnMousePress;
            InputManager.OnMouseRelease += OnMouseRelease;
            InputManager.OnKeypress += OnCancelInput;
        }

        private void Start()
        {
            _groupManager = GroupManager.Instance;
        }

        #region Input

        void OnMouseStart(Vector3 position, int x, KeyCode type, InputState _state)
        {
            _mainCamera = Camera.main;
            if (x != 1) return;
            if (_state != InputState.UNIT_MOVEMENT) return;
            InputManager.Instance.ChangeInputState(InputState.UNIT_SELECTION);
            _cancelInput = false;

            //If we have one of our own troops selected
            if(_groupManager._selectedUnits.Count != 0)
            {
                if (_groupManager._typeOfSelection != UnitType.Enemy || _groupManager._typeOfSelection != UnitType.Animal)
                {
                    _startMousePosition = position;
                    _startMousePositionIn3D = HelperFunctions.GetMousePositionIn3D(_startMousePosition, _groundLayerMask);

                    if (type == KeyCode.LeftShift)
                    {
                        CreateFormationVisual(FormationCreator.CreateFormation(_groupManager._selectedUnits.Count, 2f, _formationType));
                    }
                    else if (type == KeyCode.LeftControl)
                    {
                        CreateSelectionVisual();
                    }
                }
            }
        }

        void OnMousePress(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 1) return;
            if (_state != InputState.UNIT_SELECTION) return;
            if (_startedAction) return;

            //If we want to cancel our action
            if (_cancelInput)
            {
                Destroy(_base);
                return;
            }

            //If we have a previous selections of units that we own and have not started a action yet
            if(_groupManager._selectedUnits.Count != 0)
            {
                if (_groupManager._typeOfSelection != UnitType.Enemy || _groupManager._typeOfSelection != UnitType.Animal)
                {
                    _currentMousePosition = position;
                    _currentMousePositionIn3D = HelperFunctions.GetMousePositionIn3D(_currentMousePosition, _groundLayerMask);
                    if (type == KeyCode.LeftShift)
                    {
                        UpdateFormationPositions();
                    }
                    else if (type == KeyCode.LeftControl)
                    {
                        UpdateSelectionSize();
                    }
                }
            }
        }

        void OnMouseRelease(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 1) return;
            if (_state != InputState.UNIT_SELECTION) return;
            if (_startedAction) return;
            InputManager.Instance.ChangeInputState(InputState.UNIT_MOVEMENT);

            //If we want to cancel our action
            if (_cancelInput)
            {
                _cancelInput = false;
                return;
            }

            //If we have a previous selections of units that we own and have not started a action yet
            if(_groupManager._selectedUnits.Count != 0)
            {
                if (_groupManager._typeOfSelection != UnitType.Enemy || _groupManager._typeOfSelection != UnitType.Animal)
                {
                    if (type == KeyCode.LeftShift)
                    {
                        Vector3 rot = new Vector3(_base.transform.eulerAngles.x, _base.transform.eulerAngles.y + 180f, _base.transform.eulerAngles.z);
                        _groupManager.PlayerMovementInput(_startMousePositionIn3D, _worldUnitPositions, rot);
                        Destroy(_base);
                        _base = null;
                    }
                    else if (type == KeyCode.LeftControl)
                    {
                        if (_selectionSphere._selection.Count > 0)
                        {
                            _selectionSphere.SetSelectedVisual();
                            _groupManager.OnSphereSelection(_selectionSphere.transform.position, _selectionSphere._selection);
                        }
                        Destroy(_base);
                    }
                }
            }
            _startedAction = false;
        }

        void OnCancelInput(int x)
        {
            if (x != 0) return;
            _cancelInput = true;
        }
        #endregion

        #region Movement
        private void CreateFormationVisual(List<Vector2> points)
        {
            _base = new GameObject("FormationBase");
            Instantiate(_arrowSpritePrefab,
                _arrowSpritePrefab.transform.position,
                _arrowSpritePrefab.transform.rotation,
                _base.transform);

            _base.transform.position = _startMousePositionIn3D;

            _localUnitPositions = new List<Transform>();
            foreach (Vector2 p in points)
            {
                GameObject x = Instantiate(_positionVisualPrefab, 
                    _positionVisualPrefab.transform.position, 
                    _positionVisualPrefab.transform.rotation,
                    _base.transform);

                x.transform.localPosition = new Vector3(p.x, 0.1f, p.y) * _scale;
                _localUnitPositions.Add(x.transform);
            }
        }

        private void UpdateFormationPositions()
        {
            _worldUnitPositions = new List<Vector3>();
            foreach (Transform t in _localUnitPositions)
                _worldUnitPositions.Add(t.position);

            _base.transform.LookAt(_currentMousePositionIn3D, Vector3.up);
        }
        #endregion

        #region Selection
        private void CreateSelectionVisual()
        {
            _base = new GameObject("SelectionBase");
            GameObject x = Instantiate(_selectionSpritePrefab,
                _base.transform.position,
                _selectionSpritePrefab.transform.rotation,
                _base.transform);
            _base.transform.localScale = new Vector3(0, 0, 0);
            _base.transform.position = _startMousePositionIn3D;
            _selectionSphere = x.GetComponent<SelectionSphere>();
        }

        private void UpdateSelectionSize()
        {
            _base.transform.localScale = (Vector3.one * Vector3.Distance(_currentMousePositionIn3D, _startMousePositionIn3D));
        }
        #endregion

        private void OnDestroy()
        {
            InputManager.OnStartMousepress -= OnMouseStart;
            InputManager.OnMousePress -= OnMousePress;
            InputManager.OnMouseRelease -= OnMouseRelease;
        }
    }
}