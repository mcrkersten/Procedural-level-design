using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class OccupantMenuManager : MonoBehaviour
{
    private Building _currentBuilding;
    [SerializeField] private OccupantSelectorCreator _selectorCreator;
    public delegate void AssignNewOccupant();
    public static event AssignNewOccupant OnAssignNewOccupant;

    // Start is called before the first frame update
    void Start()
    {
        OccupantObjectCreator.OnPlayerAssignesNewOccupant += ActivateMenu;
        OccupantSelectionObject.OnAssignNewOccupant += AssignOccupant;
        MenuDeactivator.OnCloseMenu += DeactivateMenu;
    }

    private void ActivateMenu(Building building, OccupancyType type)
    {
        _currentBuilding = building;
        _selectorCreator.gameObject.SetActive(true);
        _selectorCreator.Create(type);
    }

    private void DeactivateMenu()
    {
        _currentBuilding = null;
        _selectorCreator.DestroyContent();
        _selectorCreator.gameObject.SetActive(false);
    }

    private void AssignOccupant(Unit unit, OccupancyType occupancyType)
    {
        _currentBuilding.AssignOccupant(unit, occupancyType);
        switch (occupancyType)
        {
            case OccupancyType.Tenant:
                unit._unitBrain._memory._home = _currentBuilding;
                break;
            case OccupancyType.Visitor:
                break;
            case OccupancyType.Worker:
                unit._unitBrain._memory._workplaceBuilding = _currentBuilding;
                break;
            default:
                break;
        }
        OnAssignNewOccupant?.Invoke();
        DeactivateMenu();
    }

    private void OnDestroy()
    {
        OccupantObjectCreator.OnPlayerAssignesNewOccupant -= ActivateMenu;
        OccupantSelectionObject.OnAssignNewOccupant -= AssignOccupant;
        MenuDeactivator.OnCloseMenu -= DeactivateMenu;
    }
}
