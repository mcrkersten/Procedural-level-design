using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

public class Workplace : Building
{
    [HideInInspector] public List<ResourceInformation> _resourcesToProduce = new List<ResourceInformation>();
    public List<WorkOrder> _workOrders = new List<WorkOrder>();
    int tick = 0;

    protected override void Awake()
    {
        base.Awake();
        _type = InteractableType.Workplace;
        _resourcesToProduce = _components._interactableInformation._thisBuildingProduces;
        CreateStockpileInformation();
    }

    private void FixedUpdate()
    {
        tick += 1;
        if(tick == 60)
        {
            WorkOnWorkorders();
            tick = 0;
        }
    }

    [ContextMenu("Add Resources")]
    public void AddResourcesManualy()
    {
        foreach (StockpileInformation stock in _stockpiles)
        {
            stock.DepositResource(stock._max);
        }
    }

    protected override void CreateStockpileInformation()
    {
        OccupancyInformation workers = new OccupancyInformation(_components._interactableInformation._occupancyCapacity, OccupancyType.Worker);
        OccupancyInformation visitors = new OccupancyInformation(_components._interactableInformation._visitorCapacity, OccupancyType.Visitor);
        _occupancyInformation.Add(workers);
        _occupancyInformation.Add(visitors);

        foreach (ResourceInformation resource in _resourcesToProduce)
        {
            foreach (StorageInformation storage in _components._interactableInformation._storageTypes)
            {
                if(storage._resourceType != resource._resourceType)
                {
                    StockpileInformation stockpile = new StockpileInformation(storage._maxStorage, storage._resourceType, false);
                    _stockpiles.Add(stockpile);
                }
            }
        }

        foreach (ResourceInformation resource in _resourcesToProduce)
        {
            foreach (StorageInformation storage in _components._interactableInformation._storageTypes)
            {
                if(storage._resourceType == resource._resourceType)
                {
                    StockpileInformation stockpile = new StockpileInformation(storage._maxStorage, storage._resourceType, true);
                    _producedStockStockpiles.Add(stockpile);
                }
            }
        }
    }

    public void WorkOnWorkorders()
    {
        int unitsWorking = 0;
        for (int i = 0; i < _occupancyInformation.Count; i++)
        {
            if(_occupancyInformation[i]._occupancyType == OccupancyType.Worker)
            {
                foreach (Unit unit in _unitsInsideBuilding)
                {
                    if (_occupancyInformation[i]._occupants.Contains(unit))
                    {
                        unitsWorking += unit._unitBrain._behaviourModule._actionStrenght;
                    }
                }
            }
        }

        foreach (ResourceInformation toProduce in _resourcesToProduce)
        {
            if (HasActiveWorkOrder(toProduce))
            {
                WorkOrder workOrder =  _workOrders.First(order => order._resourceToProduce._resourceType == toProduce._resourceType);
                int result = workOrder.Work(unitsWorking);

                StockpileInformation stockpile = _producedStockStockpiles.First(stock => stock._resourceType == toProduce._resourceType);
                stockpile.DepositResource(result);
                if (result == 1)
                    _workOrders.Remove(workOrder);
            }
            else
            {
                ResourceType type = CreateWorkOrder(toProduce);
                if (type != ResourceType.None)
                {
                    Unit selectedWorker = SelectWorker();
                    if(selectedWorker != null)
                    {
                        Debug.Log("Not enough " + type);
                        SendWorkerToStorage(selectedWorker, type);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if a workorder with the type of the resourceToProduce exists
    /// </summary>
    /// <param name="resourceToProduce"></param>
    /// <returns>Exists or not</returns>
    private bool HasActiveWorkOrder(ResourceInformation resourceToProduce)
    {
        foreach (WorkOrder order in _workOrders)
        {
            if(order._resourceToProduce._resourceType == resourceToProduce._resourceType)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Create a workOrder object with type of the resourceToProduce and takes the necessary resources from the building.
    /// </summary>
    /// <param name="resourceToProduce"></param>
    /// <returns>If the creation whas a succes and the building had enough resources to create a workOrder.</returns>
    private ResourceType CreateWorkOrder(ResourceInformation resourceToProduce)
    {
        foreach (CostInformation cost in resourceToProduce._resourcesNeededToProduce)
        {
            foreach (StockpileInformation stock in _stockpiles)
            {
                if(cost._resourceInformation._resourceType == stock._resourceType)
                    if (stock.StockCheck(cost._amount))
                        continue;
                    else
                        return stock._resourceType;
            }
        }

        Transaction(resourceToProduce);
        WorkOrder newObject = new WorkOrder(resourceToProduce);
        _workOrders.Add(newObject);
        return ResourceType.None;
    }

    /// <summary>
    /// Charge the necessary resources from the building.
    /// </summary>
    /// <param name="stockToProduce"></param>
    private void Transaction(ResourceInformation stockToProduce)
    {
        foreach (CostInformation cost in stockToProduce._resourcesNeededToProduce)
        {
            foreach (StockpileInformation stock in _stockpiles)
            {
                if (cost._resourceInformation._resourceType == stock._resourceType)
                    stock.WithdrawalResource(cost._amount);
            }
        }
    }

    private Unit SelectWorker()
    {
        foreach (Unit unit in _unitsInsideBuilding)
        {
            if(unit._unitType == UnitType.Worker)
            {
                foreach (OccupancyInformation occupancy in _occupancyInformation)
                {
                    if (occupancy._occupancyType == OccupancyType.Worker)
                    {
                        if (occupancy._occupants.Contains(unit))
                        {
                            return unit;
                        }
                    }
                }
            }
        }
        Debug.Log("No active workers");
        return null;
    }

    private void SendWorkerToStorage(Unit unit, ResourceType type)
    {
        Debug.Log(gameObject.name + " commands " + unit._unitBrain._name + " to gather " + type);
        unit._unitBrain._behaviourModule.CollectResource(type, InteractableType.Storage, this);
    }

    protected override void DestroyInteractable()
    {
        _components._buildingCollider.enabled = false;
        foreach (StockpileInformation stock in _producedStockStockpiles)
        {
            if (stock._currentStockAmount > 0)
            {
                GameObject stack = Instantiate(ResourceDatabase.GetResourceStackObject(stock._resourceType),
                    GetStackPosition(),
                    this.transform.rotation,
                    null);

                ResourceStack resourceStack = stack.GetComponent<ResourceStack>();
                resourceStack._amountOfResources = stock._currentStockAmount;
                resourceStack._resourceInformation = ResourceDatabase.GetResourceInformation(stock._resourceType);
            }
        }
        base.DestroyInteractable();
    }
}
[System.Serializable]
public class WorkOrder
{
    public ResourceInformation _resourceToProduce;
    public float _progress;
    private int _workDone;

    public WorkOrder(ResourceInformation toProduce)
    {
        _resourceToProduce = toProduce;
        _progress = 0;
        _workDone = 0;
    }

    public int Work(int strenght)
    {
        _workDone += strenght;
        _progress = (float)_workDone / (float)_resourceToProduce._workEnergyNeededToProduce;

        if(_workDone >= _resourceToProduce._workEnergyNeededToProduce)
        {
            return 1;
        }
        return 0;
    }
}