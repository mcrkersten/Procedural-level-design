using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

[RequireComponent(typeof(SaveableEntity))]
public class UnitInteractable : MonoBehaviour
{
    public bool isDemolished { protected set; get; }
    public Transform _targetTransform;
    [HideInInspector] public List<SliderInformation> _sliderStatistics = new List<SliderInformation>();
    [HideInInspector] public List<Cell> _cells = new List<Cell>();
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

    [HideInInspector] public string _interactableID;

    public UnitType _whoCanInteract;
    public InteractableType _type;
    protected OutlineState _outlineState;

    protected Outline[] _outlines;
    protected FlowField _flowField;
    protected Animator _animator;
    public Vector2Int _gridSize;
    public float _interactionRadius;
    public LayerMask _layerMask;
    public bool _scheduledForDestruction;
    public bool _useGroundNormal;
    public bool _isMenuInstance;
    protected GameManager _gameManager;

    public int _intergrationLayer { set; get; }

    public delegate void UnitInteractableMouseEvent(UnitInteractable x, OutlineState state);
    public static event UnitInteractableMouseEvent OnUnitInteractableMouseEvent;

    public delegate void UnitInteractableCursorEvent(CursorType type);
    public static event UnitInteractableCursorEvent OnUnitInteractableCursorEvent;

    public delegate void UnselectInteractable(UnitInteractable x);
    public static event UnselectInteractable OnUnselectInteractable;

    public delegate void DeclareInteractable(UnitInteractable i);
    public static event DeclareInteractable OnDeclareInteractable;

    public delegate void UnDeclareInteractable(UnitInteractable i);
    public static event DeclareInteractable OnUnDeclareInteractable;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _outlines = GetComponentsInChildren<Outline>();
        UnitSelection.OnUnSelectAll += Unselect;
        FlowField.OnGenesisFieldCreated += SetCellTypesUnderObject;
        UnitInteractable.OnUnselectInteractable += OnUnselectUnitInteractable;
        SavingLoading.OnPrepareToSave += CreateID;
        UIhover.OnUiHover += OnMouseExit;

        if(_type != InteractableType.Construction)
            OnDeclareInteractable?.Invoke(this);

    }

    protected virtual void Start()
    {
        _gameManager = GameManager._instance;
    }

    public void SetOutlineState(OutlineState state)
    {
        if (UnitSelection.Instance != null)
        {
            switch (state)
            {
                case OutlineState.UnSelected:
                    for (int i = 0; i < _outlines.Length; i++)
                    {
                        _outlines[i].enabled = false;
                    }
                    _outlineState = state;
                    break;
                case OutlineState.Selected:
                    for (int i = 0; i < _outlines.Length; i++)
                    {
                        _outlines[i].OutlineColor = ColourDatabase.instance.Selection;
                        _outlines[i].enabled = true;
                    }
                    _outlineState = state;
                    break;
                case OutlineState.AboutToBeSelected:
                    for (int i = 0; i < _outlines.Length; i++)
                    {
                        _outlines[i].OutlineColor = ColourDatabase.instance.Hover;
                        _outlines[i].enabled = true;
                    }
                    _outlineState = state;
                    break;
                default:
                    break;
            }
        }
    }
    protected void SendInteractableDelegate(UnitInteractable u, OutlineState state)
    {
        if(InputManager.Instance?._inputState != InputState.MENU_HOVER)
            OnUnitInteractableMouseEvent?.Invoke(u, state);
    }
    protected void SendCursorDelegate(CursorType u)
    {
        if (InputManager.Instance?._inputState != InputState.MENU_HOVER)
            OnUnitInteractableCursorEvent?.Invoke(u);
    }
    public virtual void OnPlacement()
    {
        GetCellsUndeObject();
        SetCellTypesUnderObject();

        if (this as Building)
            LevelBuilding();

        if(_type == InteractableType.Construction)
            OnDeclareInteractable?.Invoke(this);
    }
    public virtual bool Action(int strenght, Unit unit = default)
    {
        return true;
    }
    public virtual bool DestroyInteractableAction()
    {
        isDemolished = true;
        return true;
    }

    public virtual void ScheduleForDestruction()
    {
        _scheduledForDestruction = true;
    }

    protected virtual void DestroyInteractable()
    {
        Destroy(this.gameObject);
    }

    private void GetCellsUndeObject()
    {
        if (_flowField == null)
            _flowField = GridController.Instance.GenesisField;

        if (_gridSize.x == 1)
            _cells.Add(_flowField.GetCellFromWorldPos(this.transform.position));


        if (!_cells.Any())
            for (float x = .5f - (_gridSize.x / 2f); x < .5f + (_gridSize.x / 2f); x++)
                for (float y = .5f - (_gridSize.y / 2f); y < .5f + (_gridSize.y / 2f); y++)
                    _cells.Add(_flowField.GetCellFromWorldPos(this.transform.position + new Vector3(x, 0, y)));
    }
    private void SetCellTypesUnderObject()
    {
        if (!_isMenuInstance)
        {
            _flowField._allUnitInteractables.Add(this);
            _flowField.UpdateCells(_cells, CellType.unpassable);
        }
    }
    protected virtual void LevelBuilding()
    {
        if (!((Building)this)._isShipWreck)
        {
            //MapGenerator.Instance.UpdateMap(cells, EditType.Smoothing);
            RaycastHit ray;
            if (Physics.Raycast(transform.position + new Vector3(0, 50, 0), transform.TransformDirection(Vector3.down), out ray, Mathf.Infinity, _layerMask))
            {
                Vector3 position = ray.point;
                if (position.y < 1f)
                    position[1] = 1f;

                this.transform.position = position;
            }
        }
    }
    protected virtual void CreateSliderInformation()
    {

    }
    public virtual void UpdateSliderInformation()
    {

    }
    protected void Unselect(bool UnitUnselect)
    {
        if(!UnitUnselect)
            SetOutlineState(OutlineState.UnSelected);
    }
    public void OnUnselectUnitInteractable(UnitInteractable i)
    {
        if(i != this)
        {
            Unselect(false);
        }
    }
    protected virtual void OnDestroy()
    {
        if (!_isMenuInstance && GridController.Instance != null)
        {
            OnUnDeclareInteractable?.Invoke(this);
            _flowField = GridController.Instance.GenesisField;
            _flowField._allUnitInteractables.Remove(this);
            _flowField.ResetCells(_cells);
        }

        UnitSelection.OnUnSelectAll -= Unselect;
        FlowField.OnGenesisFieldCreated -= SetCellTypesUnderObject;
        UnitInteractable.OnUnselectInteractable -= OnUnselectUnitInteractable;
        SavingLoading.OnPrepareToSave -= CreateID;
        UIhover.OnUiHover -= OnMouseExit;
    }

    public virtual void OnMouseEnter()
    {
        if (InputManager.Instance?._inputState == InputState.MENU_HOVER)
            return;

        if (_gameManager._gameState != GameState.PAUSED && _gameManager._gameState != GameState.CUTSCENE)
            if (UnitSelection.Instance != null)
                UnitSelection.Instance._hoveredUnitInteractable = this;
    }
    public virtual void OnMouseUp()
    {
        if (InputManager.Instance?._inputState == InputState.MENU_HOVER)
            return;

        if (_gameManager._gameState != GameState.PAUSED && _gameManager._gameState != GameState.CUTSCENE)
        {
            OnUnselectInteractable?.Invoke(this);
            SetOutlineState(OutlineState.Selected);
            SendInteractableDelegate(this, OutlineState.Selected);
        }
    }
    public virtual void OnMouseExit()
    {
        if (_outlineState != OutlineState.Selected)
        {
            SetOutlineState(OutlineState.UnSelected);
            SendInteractableDelegate(this, OutlineState.UnSelected);
        }
        SendCursorDelegate(CursorType.None);

        if(UnitSelection.Instance != null)
            UnitSelection.Instance._hoveredUnitInteractable = null;
    }
    public void CreateID()
    {
        string x = _ID;
    }
}

public class SliderInformation
{
    public float _value;
    public string _title;

    public SliderInformation(float value, string title)
    {
        _value = value;
        _title = title;
    }
}
