using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using UnityEngine.UI;
using TMPro;
public class WorkTaskCreator : MonoBehaviour
{
    [Header("sceneObject")]
    [SerializeField] private StatisticsCanvas _statisticsCanvas;
    [SerializeField] private GameObject _emptyTaskObject;

    [Header("mainPrefabs")]
    [SerializeField] private GameObject _workTaskPanelObject;
    private List<GameObject> _instantiated_objects = new List<GameObject>();

    [Header("subPrefabs")]
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private GameObject _textPrefab;
    [SerializeField] private GameObject _workTaskIconObjectPrefab;
    [SerializeField] private GameObject _sliderPrefab;

    [Header("Icon sprites")]
    [SerializeField] private Sprite _workTaskIcon;
    [SerializeField] private Sprite _selfChosenTaskIcon;

    [Header("")]
    [SerializeField] private Sprite _energyIcon;
    [SerializeField] private Sprite _workplaceVisitorIcon;

    [Header("")]
    [SerializeField] private Sprite _searchIcon;
    [SerializeField] private Sprite _movingIcon;
    private bool onWayBack;
    private UnitBehaviourState State { 
        set 
        { 
            if(currentState != value) 
            {
                Debug.Log(value);
                currentState = value;
                CreateWorkTask(_statisticsCanvas._selectedUnit);
            }
        } 
        get { return currentState; } 
    }

    private UnitBehaviourState currentState = UnitBehaviourState.None;
    public void OnEnable()
    {
        CreateWorkTask(_statisticsCanvas._selectedUnit);
    }

    public void Update()
    {
        Unit unit = _statisticsCanvas._selectedUnit;

        if(currentState != _statisticsCanvas._selectedUnit._unitBrain._memory._behaviourState)
            State = _statisticsCanvas._selectedUnit._unitBrain._memory._behaviourState;


        if (unit._unitBrain._memory._resourceQuest != null)
        {
            if (onWayBack != unit._unitBrain._memory._resourceQuest._onReturn)
            {
                onWayBack = unit._unitBrain._memory._resourceQuest._onReturn;
                CreateWorkTask(unit);
            }
        }
        UpdateSliders(unit);

    }

    private void CreateWorkTask(Unit unit)
    {
        DestroyInstantiatedObjects();
        _emptyTaskObject.SetActive(false);

        switch (unit._unitBrain._memory._behaviourState)
        {
            case UnitBehaviourState.GoingHome:
                DrawMovementTask(unit._unitBrain._memory._home);
                break;
            case UnitBehaviourState.IsHome:
                DrawWorkingInsideBuildingTask(unit);
                break;
            case UnitBehaviourState.GoingForMeal:
                DrawMovementTask(unit._unitBrain._memory._taskInteractable);
                break;
            case UnitBehaviourState.Eating:
                DrawWorkingInsideBuildingTask(unit);
                break;
            case UnitBehaviourState.WorkplaceTask:
                DrawWorkplaceTask(unit);
                break;
            case UnitBehaviourState.Working:
                //Working inside
                if (unit._unitBrain._currentEnteredBuilding != null)
                    DrawWorkingInsideBuildingTask(unit);
                //WorkingOutside
                else
                    DrawWorkingOutsideBuildingTask(unit);
                break;
            default:
                _emptyTaskObject.SetActive(true);
                break;
        }
    }

    private void DrawWorkplaceTask(Unit unit)
    {
        WorkObjectIcon();

        GameObject workTask = Instantiate(_workTaskPanelObject, this.transform);
        _instantiated_objects.Add(workTask);

        OrigenIcon(unit, workTask.transform);
        Slider(unit, workTask.transform);
        TargetIcon(unit, workTask.transform);

        ResourceTypeIcon(unit, workTask.transform);
    }

    private void DrawWorkingInsideBuildingTask(Unit unit)
    {
        if (unit._unitBrain._currentEnteredBuilding != null)
        {
            Building building = unit._unitBrain._currentEnteredBuilding;
            foreach (OccupancyInformation occupancy in building._occupancyInformation)
            {
                if (occupancy._occupants.Contains(unit))
                {
                    GameObject icon = Instantiate(_workTaskIconObjectPrefab, this.transform);
                    _instantiated_objects.Add(icon);

                    GameObject workTask = Instantiate(_workTaskPanelObject, this.transform);
                    _instantiated_objects.Add(workTask);

                    switch (occupancy._occupancyType)
                    {
                        case OccupancyType.Tenant:
                            icon.GetComponent<Image>().sprite = _selfChosenTaskIcon;

                            GameObject iconObject01 = Instantiate(_iconPrefab, workTask.transform);
                            iconObject01.GetComponent<Image>().sprite = _energyIcon;

                            GameObject slider01 = Instantiate(_sliderPrefab, workTask.transform);
                            Slider slider = slider01.GetComponent<Slider>();
                            slider.value = unit._unitBrain._health._fineEnergy / unit._unitBrain._health._maximumValues._maxEnergy;
                            break;
                        case OccupancyType.Visitor:
                            icon.GetComponent<Image>().sprite = _selfChosenTaskIcon;

                            GameObject iconObject02 = Instantiate(_iconPrefab, workTask.transform);
                            iconObject02.GetComponent<Image>().sprite = _workplaceVisitorIcon;

                            GameObject slider02 = Instantiate(_sliderPrefab, workTask.transform);
                            Slider slider0 = slider02.GetComponent<Slider>();
                            slider0.value = unit._unitBrain._health._fineHunger / unit._unitBrain._health._maximumValues._maxHunger;
                            break;
                        case OccupancyType.Worker:
                            icon.GetComponent<Image>().sprite = _workTaskIcon;

                            GameObject iconObject03 = Instantiate(_iconPrefab, workTask.transform);
                            iconObject03.GetComponent<Image>().sprite = _workTaskIcon;

                            GameObject textObject03 = Instantiate(_textPrefab, workTask.transform);
                            textObject03.GetComponent<TextMeshProUGUI>().text = "Working";
                            break;
                    }
                }
            }
        }
    }

    private void DrawWorkingOutsideBuildingTask(Unit unit)
    {
        if(unit._unitBrain._memory._lastInteractedResource != null)
        {
            GameObject icon = Instantiate(_workTaskIconObjectPrefab, this.transform);
            _instantiated_objects.Add(icon);

            GameObject workTask = Instantiate(_workTaskPanelObject, this.transform);
            _instantiated_objects.Add(workTask);

            GameObject textObject01 = Instantiate(_textPrefab, workTask.transform);
            textObject01.GetComponent<TextMeshProUGUI>().text = "Working";

            GameObject iconObject04 = Instantiate(_iconPrefab, workTask.transform);
            iconObject04.GetComponent<Image>().sprite = ResourceDatabase.GetResourceIcon(((ResourceInteractable)unit._unitBrain._memory._lastInteractedResource)._resourceInformation._resourceType);

            GameObject iconObject03 = Instantiate(_iconPrefab, workTask.transform);
            iconObject03.GetComponent<Image>().sprite = BuildingTypeDatabase.GetBuildingTypeIcon(unit._unitBrain._memory._lastInteractedResource._type);
        }
    }

    private void DrawMovementTask(UnitInteractable building)
    {
        //  ----------------------    --------
        //  | icon3 icon2  icon1 |    | icon |
        //  ----------------------    --------
        //        worktask

        GameObject icon = Instantiate(_workTaskIconObjectPrefab, this.transform);
        _instantiated_objects.Add(icon);
        icon.GetComponent<Image>().sprite = _selfChosenTaskIcon;

        GameObject workTask = Instantiate(_workTaskPanelObject, this.transform);
        _instantiated_objects.Add(workTask);

        GameObject iconObject01 = Instantiate(_iconPrefab, workTask.transform);
        iconObject01.GetComponent<Image>().sprite = BuildingTypeDatabase.GetBuildingTypeIcon(building._type);

        GameObject iconObject02 = Instantiate(_iconPrefab, workTask.transform);
        switch (building._type)
        {
            case InteractableType.Housing:
                iconObject02.GetComponent<Image>().sprite = _energyIcon;
                break;
            case InteractableType.Resource:
                iconObject02.GetComponent<Image>().sprite = ResourceDatabase.GetResourceIcon(((ResourceInteractable)building)._resourceInformation._resourceType); ;
                break;
            case InteractableType.Workplace:
                iconObject02.GetComponent<Image>().sprite = _workplaceVisitorIcon;
                break;
            default:
                break;
        }

        GameObject iconObject03 = Instantiate(_iconPrefab, workTask.transform);
        iconObject03.GetComponent<Image>().sprite = _movingIcon;
    }

    private void UpdateSliders(Unit unit)
    {
        foreach (GameObject item in _instantiated_objects)
        {
            Slider slider = item.GetComponent<Slider>();
            if (slider != null)
            {
                if (unit._unitBrain._memory._resourceQuest != null)
                {
                    float maxValue = Vector3.Distance(unit._unitBrain._lastInteractionPosition, unit._unitBrain._memory._taskInteractable.transform.position);
                    float distanceToTarget = Vector3.Distance(unit.transform.position, unit._unitBrain._memory._taskInteractable._targetTransform.transform.position);
                    float percentage = (distanceToTarget / maxValue);
                    slider.value = 1f - percentage;
                }
                else if (unit._unitBrain._memory._lastInteractedResource != null)
                {
                    float maxValue = Vector3.Distance(unit._unitBrain._lastInteractionPosition, unit._unitBrain._memory._lastInteractedResource._targetTransform.position);
                    float distanceToTarget = Vector3.Distance(unit.transform.position, unit._unitBrain._memory._lastInteractedResource._targetTransform.position);
                    float percentage = (distanceToTarget / maxValue);
                    slider.value = 1f - percentage;
                }
            }
        }
    }

    private void DestroyInstantiatedObjects()
    {
        GameObject[] x = _instantiated_objects.ToArray();
        int count = _instantiated_objects.Count;
        for (int i = 0; i < count; i++)
            Destroy(x[i]);
        _instantiated_objects = new List<GameObject>();
    }

    #region WorktaskObjects
    private void WorkObjectIcon()
    {
        GameObject icon = Instantiate(_workTaskIconObjectPrefab, this.transform);
        icon.GetComponent<Image>().sprite = _workTaskIcon;
        _instantiated_objects.Add(icon);
    }

    private void OrigenIcon(Unit unit, Transform instantiateTransform)
    {
        //Building request type < Comes from
        if (unit._unitBrain._memory._resourceQuest._taskGivingBuilding != null)
        {
            Debug.Log("lol");
            GameObject iconObject01 = Instantiate(_iconPrefab, instantiateTransform.transform);
            iconObject01.GetComponent<Image>().sprite = BuildingTypeDatabase.GetBuildingTypeIcon(unit._unitBrain._memory._resourceQuest._taskGivingBuilding._type);
        }
        else
        {
            GameObject iconObject01 = Instantiate(_iconPrefab, instantiateTransform.transform);
            if(!onWayBack)
                iconObject01.GetComponent<Image>().sprite = BuildingTypeDatabase.GetBuildingTypeIcon(InteractableType.Resource);
            else
                iconObject01.GetComponent<Image>().sprite = BuildingTypeDatabase.GetBuildingTypeIcon(InteractableType.Storage);
        }
    }

    private void TargetIcon(Unit unit, Transform instantiateTransform)
    {
        //We are moving to a building
        if (unit._unitBrain._memory._taskInteractable != null)
        {
            GameObject iconObject03 = Instantiate(_iconPrefab, instantiateTransform.transform);
            iconObject03.GetComponent<Image>().sprite = BuildingTypeDatabase.GetBuildingTypeIcon(unit._unitBrain._memory._taskInteractable._type);
        }
        //We are searching for a building
        else
        {
            GameObject iconObject03 = Instantiate(_iconPrefab, instantiateTransform.transform);
            iconObject03.GetComponent<Image>().sprite = _searchIcon;
        }
    }

    private void Slider(Unit unit, Transform instantiateTransform)
    {
        //Slider
        GameObject sliderObject = Instantiate(_sliderPrefab, instantiateTransform.transform);
        Slider slider = sliderObject.GetComponent<Slider>();
        _instantiated_objects.Add(sliderObject);

        if (unit._unitBrain._memory._resourceQuest._taskGivingBuilding != null)
        {
            float distanceBetween = Vector3.Distance(unit._unitBrain._memory._resourceQuest._taskGivingBuilding._targetTransform.position, unit._unitBrain._memory._taskInteractable.transform.position);
            float distanceToTarget = Vector3.Distance(unit._unitBrain._unitBody.transform.position, unit._unitBrain._memory._taskInteractable.transform.position);
            float percentage = 1f - (distanceToTarget / distanceBetween);
            slider.value = percentage;
        }
        else if (unit._unitBrain._memory._lastInteractedResource != null)
        {
            float distanceBetween = Vector3.Distance(unit._unitBrain._lastInteractionPosition, unit._unitBrain._memory._lastInteractedResource._targetTransform.position);
            float distanceToTarget = Vector3.Distance(unit._unitBrain._unitBody.transform.position, unit._unitBrain._memory._taskInteractable.transform.position);
            float percentage = 1f - (distanceToTarget / distanceBetween);
            slider.value = percentage;
        }
    }

    private void ResourceTypeIcon(Unit unit, Transform instantiateTransform)
    {
        //Resource request type < Goes to type
        GameObject iconObject02 = Instantiate(_iconPrefab, instantiateTransform.transform);
        iconObject02.GetComponent<Image>().sprite = ResourceDatabase.GetResourceIcon(unit._unitBrain._memory._resourceQuest._resourceType);
    }
    #endregion
}
