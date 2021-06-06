using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnitsAndFormation;
public class OccupantObjectCreator : MonoBehaviour
{
    public TextMeshProUGUI _title;

    [Header("Empty/Unassigned Sprites")]
    public Sprite _unassignedOccupant;
    public Sprite _emptyVisitor;

    [Header("Filled/Occupied Sprites")]
    public Sprite _occupiedTenant;
    public Sprite _occupiedWorker;
    public Sprite _filledVisitor;

    [Header("Occupied but not inside building Sprites")]
    public Sprite _occupiedButOutsideBuilding;

    [Header("Assigned Prefabs")]
    public GameObject _unassignedOccupantButton; //For visitors, we remove the button component.
    public GameObject _assignedOccupantButton;

    private int _buttonIndex = 0;

    [HideInInspector] public Building _building;

    [HideInInspector] public List<OccupantObject> _instantiated_OccupantObjects = new List<OccupantObject>();
    [HideInInspector] public OccupancyInformation _occupancyInformation;


    public delegate void PlayerAssignNewOccupant(Building buildingForOccupant, OccupancyType type);
    public static event PlayerAssignNewOccupant OnPlayerAssignesNewOccupant;

    public void PlayerAssignedNewOccupant(OccupancyType type)
    {
        OnPlayerAssignesNewOccupant?.Invoke(_building, type);
    }

    private void OnEnable()
    {
        _buttonIndex = 0;
    }

    public void OnOccupantClick(int number, OccupancyType type)
    {
        Unit occupant;
        foreach (OccupancyInformation information in _building._occupancyInformation)
        {
            if(type == information._occupancyType)
            {
                occupant = information._occupants[number];
                occupant.OnMouseDown();

                //If the unit-gameobject is active, move the camera.
                if (occupant.gameObject.activeSelf)
                    CameraController._instance.CenterCameraOnObject(occupant.gameObject);
            }
        }
    }

    public void Populate()
    {
        int count = _occupancyInformation._occupants.Count;
        for (int i = 0; i < count; i++)
            InstantiateFilledSpot(_occupancyInformation._occupants[i], _occupancyInformation._occupancyType);

        int leftovers = _occupancyInformation._max - count;
        for (int i = 0; i < leftovers; i++)
            InstantiateEmptySpot(_occupancyInformation._occupancyType);
    }

    private void InstantiateEmptySpot(OccupancyType type)
    {
        GameObject x = Instantiate(_unassignedOccupantButton, this.transform);
        OccupantObject instantiate_object = x.GetComponent<OccupantObject>();
        switch (type)
        {
            case OccupancyType.Tenant:
                instantiate_object._icon.sprite = _unassignedOccupant;
                instantiate_object._button.onClick.AddListener(delegate { PlayerAssignedNewOccupant(OccupancyType.Tenant); });
                instantiate_object._tooltipTrigger.defaultContent = "Assign tenant";
                x.name = "EmptyTenantPosition";
                break;
            case OccupancyType.Visitor:
                instantiate_object._icon.sprite = _emptyVisitor;
                instantiate_object._button.interactable = false;
                instantiate_object._tooltipTrigger.defaultContent = "Empty visitor";
                x.name = "EmptyVisitorPosition";
                break;
            case OccupancyType.Worker:
                instantiate_object._icon.sprite = _unassignedOccupant;
                instantiate_object._button.onClick.AddListener(delegate { PlayerAssignedNewOccupant(OccupancyType.Worker); });
                instantiate_object._tooltipTrigger.defaultContent = "Assign worker";
                x.name = "EmptyWorkerPosition";
                break;
            default:
                break;
        }
        _instantiated_OccupantObjects.Add(instantiate_object);
    }

    private void InstantiateFilledSpot(Unit unit, OccupancyType type)
    {
        GameObject x = Instantiate(_assignedOccupantButton, this.transform);
        OccupantObject instantiated_object = x.GetComponent<OccupantObject>();
        instantiated_object._button.onClick.AddListener(delegate { OnOccupantClick(_buttonIndex, type); });

        switch (type)
        {
            case OccupancyType.Tenant:
                if (_building._unitsInsideBuilding.Contains(unit))
                    instantiated_object._icon.sprite = _occupiedTenant;
                else
                    instantiated_object._icon.sprite = _occupiedButOutsideBuilding;
                instantiated_object._tooltipTrigger.defaultContent = "Select tenant";
                x.name = "OccupiedTenantPosition";
                break;
            case OccupancyType.Visitor:
                instantiated_object._icon.sprite = _filledVisitor;
                instantiated_object._tooltipTrigger.defaultContent = "Select visitor";
                x.name = "OccupiedVisitorPosition";
                break;
            case OccupancyType.Worker:
                if (_building._unitsInsideBuilding.Contains(unit))
                    instantiated_object._icon.sprite = _occupiedWorker;
                else
                    instantiated_object._icon.sprite = _occupiedButOutsideBuilding;
                instantiated_object._tooltipTrigger.defaultContent = "Select worker";
                x.name = "OccupiedWorkerPosition";
                break;
            default:
                break;
        }
        _buttonIndex++;
        _instantiated_OccupantObjects.Add(instantiated_object);
    }
    public void DestroyObjectsInList()
    {
        OccupantObject[] x = _instantiated_OccupantObjects.ToArray();
        int count = _instantiated_OccupantObjects.Count;
        for (int i = 0; i < count; i++)
            Destroy(x[i].gameObject);
        _instantiated_OccupantObjects = new List<OccupantObject>();
    }

    private void OnDisable()
    {
        _buttonIndex = 0;
    }
}
