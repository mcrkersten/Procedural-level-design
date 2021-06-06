using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnitsAndFormation;
using DG.Tweening;
public class Construction : Building, ISaveable
{
    [HideInInspector]
    public int _constructionPoints = 0;
    public bool _isBuild;
    public bool _isFilled { private set; get; }

    protected string temp_header;
    protected string temp_content;

    [SerializeField]
    private float _buildOffset;
    private ResourceInformation _resourceInformation;
    public bool _isResource;

    protected override void Awake()
    {
        _targetTransform = this.transform;
        base.Awake();
        _type = InteractableType.Construction;
        _components._construction = this;

        if(_isResource)
            _resourceInformation = GetComponent<Buildingcomponents>()._resourceInformation;
    }

    private void FixedUpdate()
    {
        if (_scheduledForDestruction)
        {
            _scheduledForDestruction = false; //To Prevent double demolition
            DestroyInteractable();
        }
    }

    public void StartConstruction()
    {
        temp_content = _tooltip.defaultContent;
        temp_header = _tooltip.header;
        _tooltip.header = "Construction";
        _tooltip.defaultContent = temp_header + "Assign unit to build!";

        _gridSize = _components._interactableInformation._size;
        _components._fence.SetActive(true);
        _components._unValidModel.SetActive(false);
        _components._model.SetActive(true);
        _components._constructionCollider.enabled = true;
        _components._buildingCollider.enabled = false;
        _components._model.transform.localPosition = new Vector3(0, _buildOffset, 0);

        ChangeWarningSymbol(ObjectWarningType.NeedsConstruction);
        CreateSliderInformation();
        CreateStockpileInformation();
        base.OnPlacement();

    }

    protected override void CreateStockpileInformation()
    {
        foreach (StorageInformation resource in _components._interactableInformation._costs)
        {
            StockpileInformation stockpile = new StockpileInformation(resource._maxStorage, resource._resourceType, false);
            _stockpiles.Add(stockpile);
        }
    }

    protected override void CreateSliderInformation()
    {
        SliderInformation x = new SliderInformation(_constructionPoints, "Progress");
        _sliderStatistics.Add(x);
    }

    public override void UpdateSliderInformation()
    {
        _sliderStatistics[0]._value = (float)_constructionPoints / (float)_components._interactableInformation._constructionPoints;
    }

    public override bool Action(int _strenght, Unit unit = default)
    {

        if (!_isBuild)
        {
            if (_isFilled)
            {
                _constructionPoints += _strenght;
                float percentage = (float)_constructionPoints / (float)_components._interactableInformation._constructionPoints;
                if (_buildOffset + (-_buildOffset * percentage) < 0)
                    _components._model.transform.DOLocalMoveY(_buildOffset + (-_buildOffset * percentage), .7f).SetEase(Ease.OutBounce).OnComplete(CompleteConstruction);
                else
                    _components._model.transform.DOLocalMoveY(0, .7f).SetEase(Ease.OutBounce).OnComplete(CompleteConstruction);
            }
            return true;
        }
        return false;
    }

    private void CheckConstruction()
    {
        foreach (StockpileInformation pile in _stockpiles)
        {
            if (pile._currentStockAmount != pile._max)
            {
                _isFilled = false;
                return;
            }
        }
        _isFilled = true;
    }

    private void CompleteConstruction()
    {
        if (_constructionPoints >= _components._interactableInformation._constructionPoints && !_isBuild)
        {
            _tooltip.defaultContent = temp_content;
            _tooltip.header = temp_header;

            DeactivateWarningSymbol();
            _components._fence.transform.DOLocalMoveY(-.5f, .7f).SetEase(Ease.OutBounce).OnComplete(DisableFence);
            _components._constructionCollider.enabled = false;
            _components._buildingCollider.enabled = true;
            _isBuild = true;

            CreateBuildingComponent(_components._interactableInformation._interactableType);
        }
    }

    private void CreateBuildingComponent(InteractableType type)
    {
        int index = GridController.Instance.GenesisField._allUnitInteractables.IndexOf(this);
        switch (type)
        {
            case InteractableType.Housing:
                House newHouse = this.gameObject.AddComponent<House>();
                newHouse._type = InteractableType.Housing;
                PopulateUnitInteractable(newHouse, .75f);
                GridController.Instance.GenesisField._allUnitInteractables[index] = newHouse;
                newHouse._cells = _cells;
                newHouse._components._interactableInformation = this._components._interactableInformation;
                newHouse.OnPlacement();
                break;
            case InteractableType.Resource:
                ResourceInteractable newResource = this.gameObject.AddComponent<ResourceInteractable>();
                newResource._type = InteractableType.Resource;
                newResource._whoCanInteract = UnitType.Harvester;
                PopulateUnitInteractable(newResource, .5f);
                newResource.SetResourceInformationAndStart(_resourceInformation);
                newResource._yeetHeight = 1f;
                GridController.Instance.GenesisField._allUnitInteractables[index] = newResource;
                newResource._cells = _cells;
                newResource.OnPlacement();
                break;
            case InteractableType.Workplace:
                Workplace newWorkplace = this.gameObject.AddComponent<Workplace>();
                newWorkplace._type = InteractableType.Workplace;
                PopulateUnitInteractable(newWorkplace, .75f);
                GridController.Instance.GenesisField._allUnitInteractables[index] = newWorkplace;
                newWorkplace._cells = _cells;
                newWorkplace._components._interactableInformation = this._components._interactableInformation;
                newWorkplace.OnPlacement();
                break;
            case InteractableType.Storage:
                Storage newStorage = this.gameObject.AddComponent<Storage>();
                newStorage._type = InteractableType.Storage;
                PopulateUnitInteractable(newStorage, .5f);
                GridController.Instance.GenesisField._allUnitInteractables[index] = newStorage;
                newStorage._cells = _cells;
                newStorage._components._interactableInformation = this._components._interactableInformation;
                newStorage.OnPlacement();
                break;
            default:
                Debug.LogError("Component creator for this type ("+ type +") has not been made or is not needed", this);
                break;
        }
        
        Destroy(this);
    }

    private void PopulateUnitInteractable(UnitInteractable interactable, float interactionDistance)
    {
        interactable._intergrationLayer = this._intergrationLayer;
        interactable._interactionRadius = interactionDistance;
        interactable._targetTransform = _components._targetPoint;
        interactable._gridSize = this._gridSize;
        _flowField.UpdateSignleIntergrationLayer(interactable._targetTransform.position, _intergrationLayer);
    }

    private void DisableFence()
    {
        _components._fence.SetActive(false);
    }

    public override void DepositInToStorage(Unit unit)
    {
        foreach (StockpileInformation storage in _stockpiles)
        {
            int fund = unit._storagCompartment.WithdrawalResource(unit._maxStorage, storage._resourceType);
            int overdraft = storage.DepositResource(fund);
            unit._storagCompartment.DepositResource(overdraft, storage._resourceType);
        }
        CheckConstruction();
    }

    public override void OnMouseEnter()
    {
        if (_gameManager._gameState != GameState.PAUSED)
        {
            _tooltip.OnMouseEnter();
            base.OnMouseEnter();
            if (_outlineState != OutlineState.Selected)
            {
                SetOutlineState(OutlineState.AboutToBeSelected);
                SendInteractableDelegate(this, OutlineState.AboutToBeSelected);
                SendCursorDelegate(CursorType.Construct);
            }
        }
    }

    public override void OnMouseExit()
    {
        _tooltip.OnMouseExit();
        base.OnMouseExit();
    }
}
