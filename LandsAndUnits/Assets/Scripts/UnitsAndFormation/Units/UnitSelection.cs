    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormationUI;

namespace UnitsAndFormation
{
    public class UnitSelection : MonoBehaviour
    {
        private static UnitSelection instance = null;
        public static UnitSelection Instance
        {
            get {
                return instance;
            }
        }

        public delegate void SelectGroupWithSingleUnit(Unit unit);
        /// <summary>
        /// Call to activate units UI icon. | UnitGroupUIManager.cs
        /// </summary>
        /// <param name="unit"></param>
        public static event SelectGroupWithSingleUnit OnSelectGroupWithSingleUnit;

        public delegate void UnSelectAll(bool UnitOnly);
        public static event UnSelectAll OnUnSelectAll;

        public delegate void UnSelectUnitInformation();
        public static event UnSelectUnitInformation OnUnSelectUnitInformation;

        private GroupManager _groupManager;
        private UnitAdministrator _unitAdministrator;
        private Camera _mainCamera;
        private GameManager _gameManager;

        //Selectionbox
        public RectTransform _selectionBox;
        private Vector2 _startPosition;
        private bool isHoveringGroupIcon;

        [SerializeField]
        private LayerMask _unitLayerMask;

        private Unit _hoveredUnit;
        private Unit _selectedUnit;

        private List<Unit> _UnitsInSelectionBox = new List<Unit>();

        public UnitInteractable _hoveredUnitInteractable;

        private bool _cancelInput;

        private void Awake()
        {
            instance = this;
            if (instance != null && instance != this)
                Destroy(this.gameObject);

            InputManager.OnStartMousepress += OnMouseStart;
            InputManager.OnMousePress += OnMousePress;
            InputManager.OnMouseRelease += OnMouseRelease;
            InputManager.OnKeypress += OnCancelInput;

            Unit.OnUnitHover += OnUnitHover;
            Unit.OnUnitClick += OnUnitClick;
            Unit.OnUnitExit += OnUnitExit;
            Unit.OnUnitWantsToBeUnselected += UnselectSingleUnit;

            TroopIcon.OnTroopSelection += OnTroopSelection;
        }

        private void Start()
        {
            _groupManager = GroupManager.Instance;
            _unitAdministrator = UnitAdministrator.Instance;
            _mainCamera = Camera.main;
            _gameManager = GameManager._instance;
        }

        public void UnSelectAllUnitOutlines(int count)
        {
            OnUnSelectAll?.Invoke(true);

            if(count != 1)
                OnUnSelectUnitInformation?.Invoke();
        }

        private void OnUnitHover(Unit unit)
        {
            _hoveredUnit = unit;
        }

        private void OnUnitClick(Unit clickedUnit)
        {
            //First click
            if (_selectedUnit == null || _selectedUnit != clickedUnit)
            {
                UnitGroup u = _groupManager.GetGroupFromSingleUnit(clickedUnit);

                //For group with one unit.
                if (u != null && u._units.Count == 1)
                {
                    _selectedUnit = clickedUnit;
                    OnTroopSelection(u, OutlineState.Selected);
                    return;
                }
                _UnitsInSelectionBox = new List<Unit>();
                SelectUnit(u);
                InputManager.Instance.ChangeInputState(InputState.UNIT_SELECTION);
                return;
            }
            //Second click
            if (_selectedUnit == clickedUnit)
            {
                UnitGroup u = _groupManager.GetGroupFromSingleUnit(clickedUnit);
                OnTroopSelection(u, OutlineState.Selected);
                InputManager.Instance.ChangeInputState(InputState.UNIT_SELECTION);
            }
        }

        private void OnUnitExit(Unit unit)
        {
            if(_hoveredUnit == unit)
                _hoveredUnit = null;
        }

        #region Input
        private void OnMouseStart(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 0) return;
            if (_state != InputState.UNIT_MOVEMENT) return;

            //Set ready
            _cancelInput = false;

            if (!isHoveringGroupIcon && _hoveredUnitInteractable == null && _hoveredUnit == null)
            {
                _groupManager._selectedGroup = null;
                _groupManager._selectedUnits = new List<Unit>();
                _startPosition = position;
                OnUnSelectAll?.Invoke(false);
                OnUnSelectUnitInformation?.Invoke();
                _selectedUnit = null;
                InputManager.Instance.ChangeInputState(InputState.UNIT_SELECTION);
                return;
            }
        }

        private void OnMousePress(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 0) return;
            if (type != KeyCode.LeftShift) return;
            if (_state != InputState.UNIT_SELECTION) return;

            if (_cancelInput)
            {
                CancelInput();
                return;
            }

            float d = Vector3.Distance(position, _startPosition);
            if (!isHoveringGroupIcon && d > .1f)
            {
                _selectedUnit = null;
                _hoveredUnit = null;
                UpdateSelectionBox(position);
                WhileCreateSelectionBox();
            }
        }

        private void OnMouseRelease(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x != 0) return;
            if (_state != InputState.UNIT_SELECTION) return;
            InputManager.Instance.ChangeInputState(InputState.UNIT_MOVEMENT);
            if (type != KeyCode.LeftShift) return;

            if (_cancelInput)
            {
                _cancelInput = false;
                return;
            }

            float d = Vector3.Distance(position, _startPosition);
            if (!isHoveringGroupIcon && d > .1f)
                OnReleaseSelectionBox();
        }

        private void OnCancelInput(int x)
        {
            if (x != 0) return;
            _cancelInput = true;
        }
        #endregion

        private void SelectUnit(UnitGroup unitGroup)
        {
            OnUnSelectAll?.Invoke(false);

            if (_hoveredUnit != _selectedUnit)
            {
                //Set single unit as selected Unit
                _selectedUnit = _hoveredUnit;
                _groupManager._selectedUnits = new List<Unit>();
                _groupManager._selectedUnits.Add(_selectedUnit);

                if(unitGroup != null)
                {
                    foreach (Unit u in unitGroup._units)
                    {
                        u.SetOutlineState(OutlineState.AboutToBeSelected);
                    }
                }

                _selectedUnit.SetOutlineState(OutlineState.Selected);
            }
            else
            {
                //UI CALL - 
                OnSelectGroupWithSingleUnit?.Invoke(_selectedUnit);
            }
        }

        private void UnselectSingleUnit(Unit unit)
        {
            if (_groupManager._selectedGroup != null)
            {
                if (_groupManager._selectedGroup._units.Contains(unit))
                {
                    if(_groupManager._selectedGroup._units.Count == 1)
                    {
                        _groupManager._selectedGroup = null;
                        _groupManager._selectedUnits = new List<Unit>();
                        OnUnSelectAll?.Invoke(false);
                        OnUnSelectUnitInformation?.Invoke();
                        _selectedUnit = null;
                        InputManager.Instance.ChangeInputState(InputState.UNIT_SELECTION);
                    }
                }
            }
        }

        #region SelectionBoxVisuals
        // Called when creating selection box
        private void UpdateSelectionBox(Vector2 currentMousePosition)
        {
            if (!_selectionBox.gameObject.activeSelf)
                _selectionBox.gameObject.SetActive(true);

            float width = currentMousePosition.x - _startPosition.x;
            float height = currentMousePosition.y - _startPosition.y;

            _selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            _selectionBox.anchoredPosition = _startPosition + new Vector2(width / 2, height / 2);
        }

        private void WhileCreateSelectionBox()
        {
            List<Unit> selectedUnits;
            selectedUnits = GetUnitsInSelectionBox();

            foreach (Unit unit in selectedUnits)
                unit.SetOutlineState(OutlineState.AboutToBeSelected);
        }

        private void OnReleaseSelectionBox()
        {
            _selectionBox.gameObject.SetActive(false);
            _groupManager._selectedUnits = new List<Unit>();
            List<Unit> selectedUnits;
            selectedUnits = GetUnitsInSelectionBox();

            _groupManager._selectedUnits.AddRange(selectedUnits);
            foreach (Unit unit in selectedUnits)
                unit.SetOutlineState(OutlineState.Selected);
            _groupManager.OnUnitSelection();
        }

        private List<Unit> GetUnitsInSelectionBox()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            Vector2 min = _selectionBox.anchoredPosition - (_selectionBox.sizeDelta / 2);
            Vector2 max = _selectionBox.anchoredPosition + (_selectionBox.sizeDelta / 2);

            foreach (Unit unit in _unitAdministrator._units)
            {
                Vector3 screenPosition = _mainCamera.WorldToScreenPoint(unit.transform.position);
                if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
                {
                    //On first selected Unit, check what type
                    if(_UnitsInSelectionBox.Count == 0)
                    {
                        _groupManager._typeOfSelection = unit._unitType;
                    }

                    //if unit matches selection type
                    if (unit._unitType == _groupManager._typeOfSelection && !_UnitsInSelectionBox.Contains(unit))
                        _UnitsInSelectionBox.Add(unit);
                }
                else
                {
                    if (_UnitsInSelectionBox.Contains(unit))
                    {
                        _UnitsInSelectionBox.Remove(unit);
                    }

                    unit.SetOutlineState(OutlineState.UnSelected);
                }
            }

            return _UnitsInSelectionBox;
        }

        private void CancelInput()
        {
            foreach (Unit unit in _UnitsInSelectionBox)
            {
                unit.SetOutlineState(OutlineState.UnSelected);
            }

            _selectionBox.gameObject.SetActive(false);
            _groupManager._selectedUnits = new List<Unit>();
        }

        #endregion

        #region GroupElementVisuals

        private void OnTroopSelection(UnitGroup group, OutlineState state, bool afterSelection = default)
        {
            if(group._units.Count != 1)
                OnUnSelectUnitInformation?.Invoke();

            switch (state)
            {
                case OutlineState.UnSelected:
                    isHoveringGroupIcon = false;
                    if (!afterSelection)
                    {
                        if (!_groupManager._selectedUnits.Contains(group._units[0]))
                            foreach (Unit u in group._units)
                                u.SetOutlineState(state);
                        else
                            foreach (Unit u in group._units)
                                u.SetOutlineState(OutlineState.Selected);
                    }
                break;

                case OutlineState.Selected:
                    foreach (Unit u in _groupManager._selectedUnits)
                        u.SetOutlineState(OutlineState.UnSelected);

                    _groupManager._selectedUnits = new List<Unit>();
                    _groupManager._selectedGroup = group;

                    if(group != null)
                    {
                        _groupManager._selectedUnits.AddRange(group._units);
                        foreach (Unit u in group._units)
                            u.SetOutlineState(state);
                    }
                break;

                case OutlineState.AboutToBeSelected:
                    isHoveringGroupIcon = true;
                    foreach (Unit u in group._units)
                        u.SetOutlineState(state);
                break;

                default:
                    break;
            }
        }
        #endregion

        private void OnDestroy()
        {
            InputManager.OnStartMousepress -= OnMouseStart;
            InputManager.OnMousePress -= OnMousePress;
            InputManager.OnMouseRelease -= OnMouseRelease;

            GroupElement.OnGroupElementUISelection -= OnTroopSelection;
        }
    }
}
