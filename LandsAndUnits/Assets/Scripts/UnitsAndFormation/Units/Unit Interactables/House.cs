using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

public class House : Building
{
    protected override void Awake()
    {
        base.Awake();
        _type = InteractableType.Housing;

        CreateOccupancyInformation();
    }

    protected override void CreateOccupancyInformation()
    {
        OccupancyInformation x = new OccupancyInformation(_components._interactableInformation._occupancyCapacity, OccupancyType.Tenant);
        _occupancyInformation.Add(x);
    }

    private void Sleep()
    {
        List<Unit> temp = _unitsInsideBuilding;
        foreach (Unit u in temp)
            u._unitBrain._health.AddEnergy((UnitTypeDatabase.GetWellbeing(u._unitType)._energyLossPerTick));
    }
}
