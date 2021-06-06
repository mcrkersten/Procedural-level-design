using System.Collections.Generic;
using UnitsAndFormation;
using UnityEngine;

public class UnitBrain
{
    //Flag system
    public TalkCloudComponent _talkCloud { private set; get; }
    private WarningThreshold _currentEnergyThreshold;
    private WarningThreshold _currentHungerThreshold;
    public Unit _unitBody { private set; get; }
    public UnitType _unitType { private set; get; }
    public BehaviourModule _behaviourModule { private set; get; }
    public Health _health { private set; get; }
    public Memory _memory { private set; get; }
    public TargetInformation _targetInformation { protected set; get; }
    public string _name { private set; get; }
    public Building _currentEnteredBuilding { set; get; }
    public Vector3 _lastInteractionPosition;
    private int _searchRadius;

    public int _visitingTime = 0;

    public UnitBrain(Unit unit)
    {
        _unitBody = unit;
        _unitType = unit._unitType;
        _talkCloud = _unitBody._talkCloud;
        _searchRadius = unit._searchRadius;

        SetBehaviourModule();
        _targetInformation = new TargetInformation(this);
        _memory = new Memory();
        _health = new Health(this);
    }
    private void SetBehaviourModule()
    {
        switch (_unitBody._unitType)
        {
            case UnitType.Neutral:
                _behaviourModule = _unitBody.gameObject.AddComponent<BehaviourModule>();
                break;
            case UnitType.Harvester:
                _behaviourModule = _unitBody.gameObject.AddComponent<HarvestModule>();
                break;
            case UnitType.Builder:
                _behaviourModule = _unitBody.gameObject.AddComponent<BuilderModule>();
                break;
            case UnitType.Worker:
                _behaviourModule = _unitBody.gameObject.AddComponent<BehaviourModule>();
                break;
        }
    }

    #region Health Methods
    public void HealthResponse()
    {

    }

    public void EnergyResponse()
    {
        //When energy has reached it maximum
        if (_memory._hasFreeWill && _health._fineEnergy == _health._maximumValues._maxEnergy)
            //Makes the unit exit it's home and return it to is previous behavior
            _behaviourModule.BackToGroupBehaviour();

        float percentage = (_health._fineEnergy / _health._maximumValues._maxEnergy) * 100f;
        if (WarningDatabase.TriggeredActionThreshold(UnitWarningType.Energy, percentage, false))
        {
            if (_memory._hasFreeWill)
            {
                //If we are not already in one of these states.
                if (_memory._behaviourState != UnitBehaviourState.GoingHome && _memory._behaviourState != UnitBehaviourState.IsHome)
                {
                    //If this warning is 'heavier' than the current active behaviour/warning.
                    if (WarningDatabase.GetPriorityWeight(UnitWarningType.Energy) > WarningDatabase.GetPriorityWeight(_memory._behaviourState))
                        _behaviourModule.GoHome();
                }
            }
            else
            {
                //Inform player
                EnergyFlag(true);
            }
        }
    }

    public void HungerResponse()
    {
        float percentage = (_health._fineHunger / _health._maximumValues._maxHunger) * 100f;
        if (WarningDatabase.TriggeredActionThreshold(UnitWarningType.Food, percentage , true))
        {
            if (_memory._hasFreeWill)
            {
                if (_memory._behaviourState != UnitBehaviourState.GoingForMeal && _memory._behaviourState != UnitBehaviourState.Eating)
                {
                    //If this warning is 'heavier' than the current active behaviour/warning.
                    if (WarningDatabase.GetPriorityWeight(UnitWarningType.Food) > WarningDatabase.GetPriorityWeight(_memory._behaviourState))
                        _behaviourModule.GoForMeal();
                }
            }
            else
            {
                //Inform player
                HungerFlag(true);
            }
        }
    }

    private void FindFoodInteractable(out UnitInteractable interactable, out ResourceType type)
    {
        Workplace workplace;
        ResourceType resourceType01;
        FindClosestWorkplaceWithFood(out workplace, out resourceType01);

        Storage storage;
        ResourceType resourceType02;
        FindClosestStorageWithFood(out storage, out resourceType02);

        ResourceStack stack;
        ResourceType resourceType03;
        FindClosestStackWithFood(out stack, out resourceType03);

        if (workplace != null)
        {
            interactable = workplace;
            type = resourceType01;
            return;
        }
        else if (storage != null)
        {
            interactable = storage;
            type = resourceType02;
            return;
        }
        else if (stack != null)
        {
            interactable = stack;
            type = resourceType03;
            return;
        }
        else
        {
            interactable = null;
            type = ResourceType.None;
            return;
        }
    }

    public void HungerFlag(bool permanent)
    {
        float percentage = Mathf.Round(_health._fineHunger / UnitTypeDatabase.GetWellbeing(_unitBody._unitType)._maxHunger * 100f);
        WarningThreshold warning = WarningDatabase.GetThresholdMaxToMin(UnitWarningType.Food, percentage);

        if (_currentHungerThreshold != warning && !warning._firstInList)
        {
            if (warning._isLastInList)
                _talkCloud.EnableCloud(UnitWarningType.Food, permanent);
            else
                _talkCloud.EnableCloud(UnitWarningType.Food, false);
            _currentHungerThreshold = warning;
        }
    }

    public void EnergyFlag(bool permanent)
    {
        float percentage = Mathf.Round(_health._fineEnergy / UnitTypeDatabase.GetWellbeing(_unitBody._unitType)._maxEnergy * 100f);
        WarningThreshold warning = WarningDatabase.GetThresholdMinToMax(UnitWarningType.Energy, percentage);

        if (_currentEnergyThreshold != warning && !warning._firstInList)
        {
            if(warning._isLastInList)
                _talkCloud.EnableCloud(UnitWarningType.Energy, permanent);
            else
                _talkCloud.EnableCloud(UnitWarningType.Energy, false);
            _currentEnergyThreshold = warning;
        }
    }
    #endregion

    public void UpdateBehaviour(UnitBehaviourState state)
    {
        switch (state)
        {
            case UnitBehaviourState.GoingHome:
                _targetInformation.SetTargetTransform(_memory._home.transform);
                break;
            case UnitBehaviourState.GoingForMeal:
                _targetInformation.SetTargetTransform(_memory._taskInteractable.transform);
                break;
            case UnitBehaviourState.WorkplaceTask:
                _targetInformation.SetTargetTransform(_memory._taskInteractable.transform);
                break;
            default:
                break;
        }

        _memory.UpdateCurrentBehaviourState(state);
        UpdateActionState(UnitActionState.OutOfRange);
    }

    public void UpdateActionState(UnitActionState state)
    {
        _memory.UpdateActionState(state);
    }

    public void SetName(string name)
    {
        if(_name == null)
            _name = name;
    }

    /// <summary>
    /// Verify if unit has storage of given type. Creates storage if verification of given type failed.
    /// </summary>
    /// <param name="r_type">Type of storage</param>
    public void VerifyStorageOfUnit(ResourceType r_type)
    {
        bool hasStorage = false;
        foreach (StockpileInformation pile in _unitBody._storagCompartment._storage)
            if (pile._resourceType == r_type)
                hasStorage = true;

        if (!hasStorage)
            _unitBody._storagCompartment.CreateNewStorage(r_type);
    }

    public bool CompleteResourceRequest(Building building)
    {
        ResourceQuest r = _memory._resourceQuest;
        if (r != null && r._taskGivingBuilding == building)
        {
            building.DepositInToStorage(_unitBody);
            _memory._resourceQuest = null;
            return true;
        }
        else if (r != null && r._withdrawalResource)
        {
            bool succesfulWithdrawel = building.WithdrawalFromProducedStorage(_unitBody);
            if (succesfulWithdrawel) 
                _memory._resourceQuest = null;

            return succesfulWithdrawel;
        }
        else
        {
            return false;
        }

    }

    #region Food search methodes
    public bool SearchInteractableWithConsumable()
    {
        //Find building with consumable resource
        UnitInteractable interactable;
        ResourceType type;
        FindFoodInteractable(out interactable, out type);

        if (interactable != null)
        {
            interactable.CreateID();
            _memory._taskInteractable = interactable;

            int resourceWanted = CalculateNeeds(type);
            Debug.Log("Unit wants " + resourceWanted + " * " + type);
            _memory._resourceQuest = new ResourceQuest(type, resourceWanted, true);
            return true;
        }
        return false;
    }

    private int CalculateNeeds(ResourceType type)
    {
        int hunger = _health._hunger;
        int nutrition = ResourceDatabase.GetResourceInformation(type)._nutritionalValue;
        int amount = hunger / nutrition;    
        return amount;
    }

    private void FindClosestWorkplaceWithFood(out Workplace workplace, out ResourceType type)
    {
        workplace = null;
        type = ResourceType.None;

        float distance = _searchRadius;
        foreach (Workplace worplace in BuildBuildingsLibrary.Instance._instantiatedWorkplaces)
        {
            foreach (StockpileInformation stock in worplace._producedStockStockpiles)
            {
                ResourceInformation x = ResourceDatabase.GetResourceInformation(stock._resourceType);
                float currentDistance = Vector3.Distance(worplace.gameObject.transform.position, _unitBody.transform.position);
                Debug.Log(currentDistance + " " + distance);
                //If consumable, has stock and is no base ingredient.
                if (x._consumable && stock._currentStockAmount > 0 && stock._isProduced && currentDistance < distance)
                {
                    workplace = worplace;
                    type = x._resourceType;
                }
            }
        }
    }

    private void FindClosestStorageWithFood(out Storage storage, out ResourceType type)
    {
        storage = null;
        type = ResourceType.None;

        float distance = _searchRadius;
        foreach (Storage storages in BuildBuildingsLibrary.Instance._instantiatedStorages)
        {
            foreach (StockpileInformation stock in storages._producedStockStockpiles)
            {
                ResourceInformation x = ResourceDatabase.GetResourceInformation(stock._resourceType);
                float currentDistance = Vector3.Distance(storages.gameObject.transform.position, _unitBody.transform.position);
                Debug.Log(currentDistance + " " + distance);
                //If consumable, has stock and is no base ingredient.
                if (x._consumable && stock._currentStockAmount > 0 && stock._isProduced && currentDistance < distance)
                {
                    storage = storages;
                    type = x._resourceType;
                }
            }
        }
    }

    private void FindClosestStackWithFood(out ResourceStack stack, out ResourceType type)
    {
        stack = null;
        type = ResourceType.None;

        float distance = _searchRadius;
        foreach (ResourceStack stackX in BuildBuildingsLibrary.Instance._instantiatedResourceStacks)
        {
            float currentDistance = Vector3.Distance(stackX.gameObject.transform.position, _unitBody.transform.position);
            Debug.Log(currentDistance + " " + distance);
            if (stackX._resourceInformation._consumable && currentDistance < distance)
            {
                stack = stackX;
                type = stackX._resourceInformation._resourceType;
            }
        }
    }
    #endregion

    public bool RequestEnterToBuilding(Building building, out OccupancyType enterType)
    {
        bool enterGranted = building.RequestEnter(_unitBody, out enterType);
        if (enterGranted)
        {
            _currentEnteredBuilding = building;
            _behaviourModule.EnterBuilding(building, enterType);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SearchForWork()
    {
        switch (_unitType)
        {
            case UnitType.Harvester:
                //look for storages that need filling
                List<UnitInteractable> storages = HelperFunctions.LocateUnFilledHarvestableStorages(60f);
                Storage selectedStorage = (Storage)HelperFunctions.GetClosestUnitInteractableInListToVector3(storages, _unitBody.transform.position);
                _unitBody._unitBrain._behaviourModule.CollectResource(selectedStorage._stockpiles[0]._resourceType ,InteractableType.Resource , selectedStorage);
                break;
            case UnitType.Builder:
                //Look for buildings that need building or flaged for demolishment
                break;
            case UnitType.Worker:
                //Look for empty workplaces if unit has no assigned workplace.
                //Else go to workplace
                break;
            default:
                break;
        }
    }
}

public class Multipliers
{
    private UnitBrain _brain;
    public float _energyMultiplier { get { return CalculateCurrentEnergyMultiplierBasedOnHunger(); } }
    public float _hungerMultiplier { get { return CalculateCurrentHungerMultiplierBasedOnEnergy(); } }
    public Multipliers(UnitBrain module)
    {
        _brain = module;
    }

    private float CalculateCurrentEnergyMultiplierBasedOnHunger()
    {
        float percentage = _brain._health._hunger / _brain._health._maximumValues._maxHunger;
        return _brain._health._maximumValues._maxEnergyMultiplier * percentage;
    }
    private float CalculateCurrentHungerMultiplierBasedOnEnergy()
    {
        float percentage = _brain._health._energy / _brain._health._maximumValues._maxEnergy;
        return _brain._health._maximumValues._maxHungerMultiplier * percentage;
    }
}

public class ResourceQuest
{
    public bool _withdrawalResource; //To either collect or deposit resource
    public ResourceType _resourceType;
    public int _amount;
    public UnitInteractable _taskGivingBuilding;
    public bool _onReturn;
    public ResourceQuest(ResourceType type, int amount, bool withdrawal, UnitInteractable requester = null)
    {
        _resourceType = type;
        _amount = amount;
        _withdrawalResource = withdrawal;
        _taskGivingBuilding = requester;
    }
}

public class Health
{
    public UnitTypeDatabase.UnitWellbeing _maximumValues { private set; get; }
    public int _health { private set; get; }
    public int _energy { get { return Mathf.RoundToInt(_fineEnergy); } }
    public float _fineEnergy { private set; get; }
    public int _hunger { get { return Mathf.RoundToInt(_fineHunger); } }
    public float _fineHunger { private set; get; }

    private UnitBrain _brain;
    private Multipliers _multipliers;

    ///On new Unit
    public Health(UnitBrain brain)
    {
        _brain = brain;
        _multipliers = new Multipliers(_brain);
        _maximumValues = UnitTypeDatabase.GetWellbeing(_brain._unitBody._unitType);
        _health = _maximumValues._maxHealth;
        _fineEnergy = _maximumValues._maxEnergy;
        _fineHunger = 0;
    }

    ///On load Unit
    public void LoadHealth(int health = 0, float energy = 0f, float hunger = 0f)
    {
        _health = health;
        _fineEnergy = energy;
        _fineHunger = hunger;
    }

    public void AddHealth(int amount)
    {
        _health += amount;
    }

    public void ReduceHealth(int amount)
    {
        if ((_health - amount) > 0)
            _health -= amount;
        else
            _health = 0;
        _brain.HealthResponse();
        _brain._behaviourModule._animationController.TriggerDamage();
    }

    public void ReduceHunger(int amount)
    {
        if (_fineHunger - amount > 0)
            _fineHunger -= amount;
        else
            _fineHunger = 0;
        _brain._behaviourModule._animationController.TriggerResponse(Response.Eat);
    }
    public void HungerIncrease(int baseAmount)
    {
        if (_fineHunger + (baseAmount * _multipliers._hungerMultiplier) < _maximumValues._maxHunger)
            _fineHunger += baseAmount + (baseAmount * _multipliers._hungerMultiplier);
        else
        {
            _fineHunger = _maximumValues._maxHunger;
            ReduceHealth(baseAmount);
        }

        _brain.HungerResponse();
    }

    public void AddEnergy(int amount)
    {
        if (_fineEnergy + amount < _maximumValues._maxEnergy)
            _fineEnergy += amount;
        else
            _fineEnergy = _maximumValues._maxEnergy;
    }
    public void DrainEnergy(int baseAmount)
    {
        if ((_fineEnergy - (baseAmount * _multipliers._energyMultiplier)) > 0)
            _fineEnergy -= baseAmount + (baseAmount * _multipliers._energyMultiplier);
        else
        {
            _fineEnergy = 0;
            ReduceHealth(baseAmount);
        }

        _brain.EnergyResponse();
    }
}

public class Memory
{
    //Memory
    public UnitInteractable _taskInteractable { set { _taskBuildingID = value._ID; } get { return BuildBuildingsLibrary.Instance.GetUnknownInheritanceUnitInteractable(_taskBuildingID); } }
    private string _taskBuildingID;
    public UnitInteractable _lastInteractedResource { set { _lastInteractedID = value._ID; } get { return BuildBuildingsLibrary.Instance.GetResourceWithID(_lastInteractedID); } }
    private string _lastInteractedID;
    public UnitInteractable _home { set { _homeID = value._ID; } get { return BuildBuildingsLibrary.Instance.GetHouseWithID(_homeID); } }
    private string _homeID;
    public UnitInteractable _workplaceBuilding { set { _workplaceID = value._ID; } get { return BuildBuildingsLibrary.Instance.GetWorkplaceWithID(_workplaceID); } }
    private string _workplaceID;
    public UnitBehaviourState _behaviourState { private set; get; }
    public UnitActionState _actionState { private set; get; }
    public bool _hasFreeWill;
    public ResourceQuest _resourceQuest;

    public void LoadMemory(string homeID, string workplaceID, string lastInteractedID, int behavourState, bool hasFreeWill)
    {

        _homeID = homeID;
        _workplaceID = workplaceID;
        _lastInteractedID = lastInteractedID;

        _behaviourState = (UnitBehaviourState)behavourState;
        _hasFreeWill = hasFreeWill;
    }
    public void RemoveHome()
    {
        _homeID = null;
    }

    public void UpdateCurrentBehaviourState(UnitBehaviourState state)
    {
        _behaviourState = state;
    }

    public void UpdateActionState(UnitActionState state)
    {
        _actionState = state;
    }
}

public class TargetInformation
{
    private UnitBrain _unitBrain;
    private Vector3 _position;
    private Vector3 _eulerRotation;
    private Transform _transform;
    public UnitInteractable _interactable { private set; get; }

    public TargetInformation(UnitBrain controllingUnit)
    {
        _unitBrain = controllingUnit;
    }

    public void SetTargetPosition(Vector3 position)
    {
        _transform = null;
        _position = position;
    }

    public void SetTargetRotation(Vector3 rotation)
    {
        _eulerRotation = rotation;
    }

    public Vector3 Position()
    {
        return _position;
    }

    public Vector3 Rotation()
    {
        return _eulerRotation;
    }

    public Transform Transform()
    {
        return _transform;
    }

    public void SetTargetTransform(Transform transform)
    {
        Debug.Log(transform, transform);
        _transform = transform;
        SetInteractedUnitInteractable();
    }

    public void ResetTargetTransform()
    {
        _unitBrain._behaviourModule.StopAllCoroutines();
        _transform = null;
    }

    public void UpdateState()
    {

    }

    protected void SetInteractedUnitInteractable()
    {
        _unitBrain._lastInteractionPosition = _unitBrain._unitBody.transform.position;
        _interactable = _transform.GetComponent<UnitInteractable>();
        if (_interactable == null)
            _interactable = _transform.GetComponentInParent<UnitInteractable>();
    }

    public bool IsInRangeOfInteractable(UnitInteractable interactable)
    {
        if (Vector3.Distance(_unitBrain._unitBody.transform.position, interactable._targetTransform.position) < interactable._interactionRadius * 2f)
        {
            Debug.Log("In Range");
            return true;
        }
        return false;
    }

    public bool IsInRangeOfTransform(Transform trans)
    {
        UnitInteractable x = trans.GetComponent<UnitInteractable>();
        if (x == null)
            x = trans.GetComponentInParent<UnitInteractable>();
        if (Vector3.Distance(_unitBrain._unitBody.transform.position, trans.position) < x._interactionRadius * 2f)
            return true;
        return false;
    }
}