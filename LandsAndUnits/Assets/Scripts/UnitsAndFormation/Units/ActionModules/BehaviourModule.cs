using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

/// <summary>
/// Module that contains behaviours that a unit can perform.
/// </summary>
public class BehaviourModule : MonoBehaviour
{
    public float _minimumTargetGroupDistance { protected set; get; }
    public int _actionStrenght { protected set; get; }
    protected float _cooldownTime;
    protected float _animationTime;
    protected GameObject _instantiatedTool;

    protected UnitBrain _unitBrain;
    private int _collectedResourceCount = 0;
    public int _activeIntergrationLayer = 0;
    public AnimationStateController _animationController { protected set; get; }

    public void ExecuteInteractableBehaviour()
    {
        InteractableType type = _unitBrain._targetInformation._interactable._type;
        if (_unitBrain._targetInformation._interactable._scheduledForDestruction)
        {
            if (_unitBrain._unitBody._unitType == UnitType.Builder)
            {
                OnDestruction();
                return;
            }
        }

        switch (type)
        {
            case InteractableType.None:
                break;
            case InteractableType.Construction:
                OnConstruction();
                break;
            case InteractableType.Housing:
                OnHousing();
                break;
            case InteractableType.Resource:
                OnResource();
                break;
            case InteractableType.Workplace:
                OnWorkplace();
                break;
            case InteractableType.Storage:
                OnStorage();
                break;
            case InteractableType.ResourceStack:
                OnResourceStack();
                break;
            case InteractableType.Other:
                OnOther();
                break;
            default:
                break;
        }
    }

    #region UnitInteractable actions
    protected virtual void OnConstruction()
    {
        //Override in BuilderModule
    }

    protected virtual void OnHousing()
    {
        //Request enter to house
        OccupancyType enterType;
        if (_unitBrain.RequestEnterToBuilding((Building)_unitBrain._targetInformation._interactable, out enterType))
        {
            //Enter granted
            if(enterType == OccupancyType.Tenant)
                _unitBrain.UpdateBehaviour(UnitBehaviourState.IsHome);
        }
        else
        {
            //Enter denied
        }
    }

    protected virtual void OnResource()
    {
        if (_unitBrain._unitType == UnitType.Harvester)
        {
            _unitBrain.UpdateBehaviour(UnitBehaviourState.Working);
            StartCoroutine(ModuleSpecificInteractionIEnumerator(_cooldownTime, _animationTime));
        }
    }

    protected virtual void OnWorkplace()
    {
        OccupancyType enterType;
        //Request enter to workplace
        if (_unitBrain.RequestEnterToBuilding((Building)_unitBrain._targetInformation._interactable, out enterType))
        {
            //Enter granted
            bool competedResourceRequest = _unitBrain.CompleteResourceRequest((Building)_unitBrain._targetInformation._interactable);
            switch (enterType)
            {
                case OccupancyType.Worker:
                    _unitBrain.UpdateBehaviour(UnitBehaviourState.Working);
                    if (competedResourceRequest)
                        Debug.Log(_unitBrain._unitBody + "Succesful resourceRequest" );
                break;
                case OccupancyType.Visitor:
                    _unitBrain.UpdateBehaviour(UnitBehaviourState.Visiting);
                    if (competedResourceRequest)
                    {
                        Debug.Log(_unitBrain._unitBody + "Succesful resourceRequest");
                    }
                break;
            }
            return;
        }
        else
        {
            //Enter denied
        }
    }

    protected virtual void OnStorage()
    {
        ModuleSpecificInteraction();
    }

    protected virtual void OnResourceStack()
    {
        _collectedResourceCount = 1;
        StartCoroutine(ResourceStackInteractionIEnumerator(_collectedResourceCount));
    }

    protected virtual void OnOther()
    {
        Debug.Log("Other");
        ModuleSpecificInteraction();
    }

    public virtual void OnDestruction()
    {

    }
    #endregion

    private void Awake()
    {
        _animationController = this.GetComponent<AnimationStateController>();
    }

    public virtual void Start()
    {
        _unitBrain = this.GetComponent<Unit>()._unitBrain;
        UnitType type = _unitBrain._unitType;
        _minimumTargetGroupDistance = UnitTypeDatabase.GetMinimumTargetGroupDistance(type);
        _cooldownTime = UnitTypeDatabase.GetCooldownTime(type);
        _animationTime = UnitTypeDatabase.GetAnimationTime(type);
        GameObject tool = UnitTypeDatabase.GetTool(type);
        if (tool != null && type != UnitType.Neutral)
        {
            _instantiatedTool = Instantiate(tool, Vector3.zero, new Quaternion(), _unitBrain._unitBody._toolPivot);
            _instantiatedTool.transform.localPosition = Vector3.zero;
            _instantiatedTool.transform.localRotation = tool.transform.rotation;
            _actionStrenght = UnitTypeDatabase.GetActionStrenght(_unitBrain._unitType);
        }
    }

    #region ModuleInteraction
    public virtual void ModuleSpecificInteraction()
    {
        if(_unitBrain._targetInformation._interactable != null && _unitBrain._targetInformation._interactable.Action(_actionStrenght, _unitBrain._unitBody))
        {
            //Action when succes.
            return;
        }
        //We failed the action, take apropreate action
        FollowupAction();
    }

    protected virtual void FollowupAction()
    {
        Debug.Log(_unitBrain._unitBody + " is done with: " + _unitBrain._targetInformation._interactable);
        StopAllCoroutines();
        _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.None);
        _unitBrain.UpdateActionState(UnitActionState.Idle);
    }

    public virtual IEnumerator ModuleSpecificInteractionIEnumerator(float cooldownTime, float animationTime) {
        yield return null;
    }

    public IEnumerator ResourceStackInteractionIEnumerator(int amount)
    {
        float elapsedTime = 0;
        _animationController.TriggerResourceThrow();
        while (elapsedTime < .30f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ModuleSpecificInteraction();
        _collectedResourceCount++;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_unitBrain._targetInformation._interactable != null)
        {
            if (_unitBrain._memory._resourceQuest != null)
            {
                if (amount < _unitBrain._memory._resourceQuest._amount)
                {
                    StartCoroutine(ResourceStackInteractionIEnumerator(_collectedResourceCount));
                }
                else
                {
                    _unitBrain._memory._resourceQuest = null;
                    _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.None);
                    FollowupAction();
                }
            }
            else
            {
                StartCoroutine(ResourceStackInteractionIEnumerator(0));
            }
        }
    }
    #endregion

    #region Actions
    public void ReturnToTaskBuilding()
    {
        if (_unitBrain._memory._resourceQuest != null)
        {
            _unitBrain._memory._taskInteractable = _unitBrain._memory._resourceQuest._taskGivingBuilding;
            _unitBrain.UpdateBehaviour(UnitBehaviourState.WorkplaceTask);
        }
    }
    public void ReturnToHarvesting()
    {
        if (_unitBrain._unitType == UnitType.Harvester)
        {
            if(_unitBrain._memory._lastInteractedResource != null)
            {
                //Search for a new resource to harvest near last harvested resource if that resource is in cooldown.
                if (((ResourceInteractable)_unitBrain._memory._lastInteractedResource)._inCooldown)
                {
                    UnitInteractable selected = HelperFunctions.LocateNextHarvestableResouce(_unitBrain._unitBody, InteractableType.Resource, ((ResourceInteractable)_unitBrain._memory._lastInteractedResource)._resourceInformation._resourceType);
                    if (selected != null)// Found resource
                    {
                        _unitBrain._memory._taskInteractable = selected;
                        _unitBrain.UpdateBehaviour(UnitBehaviourState.WorkplaceTask);
                        return;
                    }
                    else//No near resources to last harvested resouce, search for resources near the unit.
                    {
                        selected = HelperFunctions.LocateNextHarvestableResouce(_unitBrain._unitBody ,InteractableType.Resource, ((ResourceInteractable)_unitBrain._memory._lastInteractedResource)._resourceInformation._resourceType);
                        if (selected != null)//Found resource
                        {
                            _unitBrain._memory._taskInteractable = selected;
                            _unitBrain.UpdateBehaviour(UnitBehaviourState.WorkplaceTask);
                            return;
                        }
                        else//No resource found.
                        {
                            //Unit has nothing to do
                            _unitBrain.UpdateBehaviour(UnitBehaviourState.None);
                            return;
                        }
                    }
                }
                else
                {
                    //Go back to last interacted resource
                    _unitBrain._memory._taskInteractable = _unitBrain._memory._lastInteractedResource;
                    _unitBrain.UpdateBehaviour(UnitBehaviourState.WorkplaceTask);
                    return;
                }
            }

            //The unit can do nothing because we do not know to what harvesttype it is assigned to.
            _unitBrain.UpdateBehaviour(UnitBehaviourState.None);
            return;
        }
    }
    public void BackToGroupBehaviour()
    {
        _unitBrain.UpdateBehaviour(UnitBehaviourState.Reset);
        _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.None);
        ExitBuilding();
    }


    public void GoForMeal()
    {
        Debug.Log("Search food in bag", _unitBrain._unitBody);
        StockpileInformation consumableStock = _unitBrain._unitBody._storagCompartment.GetMostNutritionalStockFromUnitStorage();
        if (consumableStock != null)
        {
            //We have food in storage, eat it.
            _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.Eating);
            StartCoroutine(EatToMinimumHungerPercentage(consumableStock, 25f));
            return;
        }
        else
        {
            //Find food-source
            Debug.Log("Search food", _unitBrain._unitBody);
            bool foundBuildingWithConsumable = _unitBrain.SearchInteractableWithConsumable();
            if (foundBuildingWithConsumable)
            {
                _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.GoingForMeal);
                _unitBrain.HungerFlag(false); //? Do we want to give different type of warning, something less intrusive
                return;
            }
        }
        //We faild to find food, inform player
        _unitBrain.HungerFlag(true);
    }
    private IEnumerator EatToMinimumHungerPercentage(StockpileInformation stockToEat, float percentageToEat)
    {
        EatResourceFromUnitStorage(stockToEat);
        yield return new WaitForSeconds(1);

        if (_unitBrain._health._fineHunger > (_unitBrain._health._maximumValues._maxHunger * (percentageToEat / 100f)))
        {
            GoForMeal();
        }
        else
        {
            _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.None);
        }
    }
    private void EatResourceFromUnitStorage(StockpileInformation stockToEat)
    {
        int resource = _unitBrain._unitBody._storagCompartment.WithdrawalResource(1, stockToEat._resourceType);
        if (resource > 0)
        {
            EatResource(ResourceDatabase.GetResourceInformation(stockToEat._resourceType), resource);
        }
    }
    private void EatResource(ResourceInformation resource, int amount)
    {
        _unitBrain._health.ReduceHunger(resource._nutritionalValue * amount);
    }


    public void GoHome()
    {
        if (_unitBrain._memory._home != null)
        {
            _unitBrain._memory.UpdateCurrentBehaviourState(UnitBehaviourState.GoingHome);
            _unitBrain.EnergyFlag(false); //? Do we want to give different type of warning, something less intrusive
        }
    }
    public void EnterBuilding(Building building, OccupancyType enterType)
    {
        _unitBrain._unitBody.InvokeUnselect();
        _unitBrain._unitBody.gameObject.SetActive(false);
        _unitBrain.UpdateActionState(UnitActionState.Idle);
    }
    public void ExitBuilding()
    {
        if (_unitBrain._currentEnteredBuilding != null)
        {
            _unitBrain._currentEnteredBuilding.ExitBuilding(_unitBrain._unitBody);
            _unitBrain._unitBody.transform.position = _unitBrain._currentEnteredBuilding._targetTransform.position;
            _unitBrain._unitBody.transform.rotation = _unitBrain._currentEnteredBuilding._targetTransform.rotation;
            _unitBrain._currentEnteredBuilding = null;
            _unitBrain._unitBody.gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Sends unit to a building of given type to collect a given resource
    /// </summary>
    /// <param name="r_type">The Type of resource the unit needs to collect</param>
    /// <param name="b_type">The type of building the unit needs to collect from</param>
    /// <param name="returnBuilding">The building that asked the unit to collect a resource, the unit will return to this building</param>
    public void CollectResource(ResourceType r_type, InteractableType b_type, UnitInteractable returnBuilding)
    {
        GameObject building = HelperFunctions.LocateNearestInteractableOfType(_unitBrain._unitBody.transform.position, b_type, r_type);
        if (building != null)
        {
            _unitBrain._memory._taskInteractable = building.GetComponent<UnitInteractable>();
            _unitBrain._memory._resourceQuest = new ResourceQuest(r_type, _unitBrain._unitBody._maxStorage, true, returnBuilding);
            _unitBrain.VerifyStorageOfUnit(r_type);
            _unitBrain.UpdateBehaviour(UnitBehaviourState.WorkplaceTask);
            ExitBuilding();
        }
        else
        {
            Debug.Log("No " + b_type + "building of type " + r_type + " was found");
        }
    }

    /// <summary>
    /// The unit will search for the closest storage of a given type and dump its collected resource.
    /// </summary>
    /// <param name="r_type">given resource</param>
    public void StoreHarvestedResource(ResourceType r_type)
    {
        List<UnitInteractable> storages = HelperFunctions.LocateUnFilledHarvestableStorages(60f);
        foreach (UnitInteractable inter in storages)
        {
            Storage storage = (Storage)inter;
            foreach (StockpileInformation  stock in storage._stockpiles)
            {
                if(stock._resourceType == r_type)
                {
                    _unitBrain._memory._taskInteractable = storage;
                    _unitBrain._memory._resourceQuest = new ResourceQuest(r_type, _unitBrain._unitBody._maxStorage, false);
                    _unitBrain.UpdateBehaviour(UnitBehaviourState.WorkplaceTask);
                    ExitBuilding(); // <- to make sure
                    Debug.Log("Heading to: " + _unitBrain._memory._taskInteractable);
                    return;
                }
            }
        }
        Debug.Log("No Storage building of type " + r_type + " was found");
    }
    #endregion

    public void OnDestroy()
    {
        Destroy(_instantiatedTool);
    }
}