using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnitsAndFormation;

public class BuildMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public InteractableInformation MenuItem
    {
        set 
        {
            OnItemUpdate(value);
        }
        get 
        {
            return _menuItem;
        }
    }

    public delegate void ItemSelected(InteractableInformation item);
    public static event ItemSelected OnItemSelected;

    public delegate void ActivateBuilding();
    public static event ActivateBuilding OnActivateBuilding;

    private InteractableInformation _menuItem;
    [SerializeField] private Canvas _objectCanvas;
    public List<Transform> _overlayObjects;

    public GameObject _unAssignedVariablePrefab;
    private List<GameObject> _instantiated_variablePrefabs = new List<GameObject>();

    [SerializeField] private Sprite _sizeIcon;
    [SerializeField] private Sprite _occupantIcon;
    [SerializeField] private Sprite _visitorIcon;
    [SerializeField] private Sprite _workerIcon;


    //Changes on type
    [Header("Information")]
    public TextMeshProUGUI _title;
    public TextMeshProUGUI _explanatoryText;


    [Header("VariableLayers")]
    public GameObject _variableLayer01;
    public GameObject _variableLayer02;

    [Header("Tile")]
    public Image _iconImage;

    public CornerObject _cornerObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHover();
    }

    public void OnPointerClick()
    {
        OnItemSelected?.Invoke(_menuItem);
        OnActivateBuilding?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisableHover();
    }

    private void EnableHover()
    {
        _objectCanvas.overrideSorting = true;
        _cornerObject.gameObject.SetActive(false);
        foreach (Transform item in _overlayObjects)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void DisableHover()
    {
        _objectCanvas.overrideSorting = false;
        _cornerObject.gameObject.SetActive(true);
        foreach (Transform item in _overlayObjects)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void OnItemUpdate(InteractableInformation newItem)
    {
        DestroyList();
        _cornerObject.DestroyInstantiatedObjects();

        Button x = GetComponentInChildren<Button>();
        x.onClick.AddListener(OnPointerClick);
        _menuItem = newItem;

        //Information
        _iconImage.sprite = newItem._image;
        _title.text = newItem._name;
        _explanatoryText.text = newItem._description;

        CreateSizeObject(newItem, _variableLayer01);
        CreateOccupancyObject(newItem, _variableLayer01);
        CreateVisitorObject(newItem, _variableLayer01);
        CreateStorageObject(newItem, _variableLayer01);
        CreateCostObject(newItem, _variableLayer02);
        CreateCornerOject(newItem);
    }

    private void CreateSizeObject(InteractableInformation newItem, GameObject layer)
    {
        GameObject size = Instantiate(_unAssignedVariablePrefab, layer.transform);
        VariableObject var = size.GetComponent<VariableObject>();
        var._text.text = newItem._size.x.ToString() + "x" + newItem._size.y.ToString();
        var._icon.sprite = _sizeIcon;
        var._tooltipTrigger.defaultContent = "Size";
    }

    private void CreateOccupancyObject(InteractableInformation newItem, GameObject layer)
    {
        if (newItem._occupancyCapacity > 0)
        {
            GameObject visitor = Instantiate(_unAssignedVariablePrefab, layer.transform);
            VariableObject var = visitor.GetComponent<VariableObject>();
            var._text.text = newItem._occupancyCapacity.ToString();

            if (newItem._interactableType == InteractableType.Workplace)
            {
                var._icon.sprite = _workerIcon;
                var._tooltipTrigger.defaultContent = "Space for workers";
            }
            else if (newItem._interactableType == InteractableType.Housing)
            {
                var._icon.sprite = _occupantIcon;
                var._tooltipTrigger.defaultContent = "Space for tenants";
            }
        }
    }

    private void CreateVisitorObject(InteractableInformation newItem, GameObject layer)
    {
        if (newItem._visitorCapacity > 0)
        {
            GameObject visitor = Instantiate(_unAssignedVariablePrefab, layer.transform);
            VariableObject var = visitor.GetComponent<VariableObject>();
            var._text.text = newItem._visitorCapacity.ToString();
            var._icon.sprite = _visitorIcon;
            var._tooltipTrigger.defaultContent = "Space for visitors";
        }
    }

    private void CreateStorageObject(InteractableInformation newItem, GameObject layer)
    {
        foreach (StorageInformation storage in newItem._storageTypes)
        {
            if(storage._resourceType != ResourceType.None && newItem._interactableType != InteractableType.Workplace)
            {
                GameObject xx = Instantiate(_unAssignedVariablePrefab, layer.transform);
                VariableObject var = xx.GetComponent<VariableObject>();
                var._text.text = storage._maxStorage.ToString();
                var._icon.sprite = ResourceDatabase.GetResourceIcon(storage._resourceType);
                var._tooltipTrigger.defaultContent = storage._resourceType.ToString();
            }
        }
    }

    private void CreateCostObject(InteractableInformation newItem, GameObject layer)
    {
        foreach (StorageInformation cost in newItem._costs)
        {
            GameObject xx = Instantiate(_unAssignedVariablePrefab, layer.transform);
            VariableObject var = xx.GetComponent<VariableObject>();
            var._text.text = cost._maxStorage.ToString();
            var._icon.sprite = ResourceDatabase.GetResourceIcon(cost._resourceType);
            var._tooltipTrigger.defaultContent = cost._resourceType.ToString();
        }
    }

    private void CreateCornerOject(InteractableInformation newItem)
    {
        if (newItem._interactableType != InteractableType.Resource)
        {
            _cornerObject.InstantiateIcon(BuildingTypeDatabase.GetBuildingTypeIcon(newItem._interactableType));
            if (newItem._interactableType == InteractableType.Storage)
                foreach (StorageInformation item in newItem._storageTypes)
                    _cornerObject.InstantiateIcon(ResourceDatabase.GetResourceIcon(item._resourceType));

            if (newItem._occupancyCapacity != 0)
                _cornerObject.InstantiateText(newItem._occupancyCapacity.ToString());
        }
        else
        {
            _cornerObject.InstantiateIcon(BuildingTypeDatabase.GetBuildingTypeIcon(newItem._interactableType));
            _cornerObject.InstantiateIcon(ResourceDatabase.GetResourceIcon(newItem._constructionPrefab.GetComponent<Buildingcomponents>()._resourceInformation._resourceType));
        }
    }

    private void OnDisable()
    {
        DisableHover();
    }

    private void DestroyList()
    {
        GameObject[] x = _instantiated_variablePrefabs.ToArray();
        int count = _instantiated_variablePrefabs.Count;
        for (int i = 0; i < count; i++)
            Destroy(x[i]);
        _instantiated_variablePrefabs = new List<GameObject>();
    }

    public void DisableButton()
    {
        this.gameObject.SetActive(false);
    }

    public void EnableButton()
    {
        this.gameObject.SetActive(true);
    }
}
