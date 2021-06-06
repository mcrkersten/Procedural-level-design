using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

public class Storage : Building, ISaveable
{

    protected override void Awake()
    {
        _type = InteractableType.Storage;
        base.Awake();
        CreateStockpileInformation();
    }

    protected override void CreateStockpileInformation()
    {
        foreach (StorageInformation resource in _components._interactableInformation._storageTypes)
        {
            StockpileInformation stockpile = new StockpileInformation(resource._maxStorage, resource._resourceType, false);
            _stockpiles.Add(stockpile);
            _stockCount++;
        }
    }

    public override bool Action(int strenght, Unit unit)
    {
        //If the unit has a request
        if(unit._unitBrain._memory._resourceQuest != null)
        {
            if (unit._unitBrain._memory._resourceQuest._withdrawalResource)
            {
                WithdrawalFromStorage(unit);
                unit._unitBrain._memory._resourceQuest._onReturn = true;
            }
            else
            {
                DepositInToStorage(unit);
                unit._unitBrain._memory._resourceQuest._onReturn = true;
            }

            StartCoroutine(WaitBeforeReturnToTask(1f, unit));
            return true;
        }
        else
        {
            OpenStorageMenu(unit);
            return true;
        }
    }

    private IEnumerator WaitBeforeReturnToTask(float timeToWait, Unit unit) 
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToWait)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        switch (unit._unitType)
        {
            case UnitType.Neutral:
                break;
            case UnitType.Harvester:
                unit._unitBrain._behaviourModule.ReturnToHarvesting();
                break;
            case UnitType.Builder:
                unit._unitBrain._behaviourModule.ReturnToTaskBuilding();
                break;
            case UnitType.Worker:
                unit._unitBrain._behaviourModule.ReturnToTaskBuilding();
                break;
            default:
                break;
        }
    }

    private void OpenStorageMenu(Unit unit)
    {

    }
}
