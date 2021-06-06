using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using UnityEngine.UI;
using TMPro;

public class StatisticsMenu : MonoBehaviour
{
    public int _lineLenghtOffset;
    [SerializeField, Header("Object creators")]
    private GameObject _occupantCreator;
    [SerializeField]
    private GameObject _workObjectCreator;
    [SerializeField]
    private GameObject _storageCreator;

    [SerializeField, Header("Basic Objects")]
    private GameObject _sliderObject;
    [SerializeField]
    private GameObject _dividerObject;

    [Header("")]
    [SerializeField]
    private List<GameObject> _popup = new List<GameObject>();

    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField]
    private Transform _line;
    public Vector3 _targetOffset = new Vector3(200, 150, 0);
    private Building _lastClickedInteractable;

    private List<SliderObject> _instantiated_SliderObjects = new List<SliderObject>();
    private List<GameObject> _instantiated_DividerObjects = new List<GameObject>();

    private List<OccupantObjectCreator> _instantiated_OccupantObjectCreators = new List<OccupantObjectCreator>();
    private List<WorkObjectCreator> _instantiated_WorkObjectCreators = new List<WorkObjectCreator>();
    private List<StorageCreator> _instantiated_StorageCreators = new List<StorageCreator>();
    private bool active;

    private void Awake()
    {
        MenuDeactivator.OnCloseMenu += Deactivate;
        UnitInteractable.OnUnitInteractableMouseEvent += ShowStatistics;
        Building.OnUnitMenuOpen += ShowBuildingfunctionMenu;
        OccupantMenuManager.OnAssignNewOccupant += UpdateOccupancyStatistics;
    }

    private void ShowStatistics(UnitInteractable newSelectedBuilding, OutlineState state)
    {
        if (state != OutlineState.Selected) return;


        if (GroupManager.Instance._selectedGroup == null)
        {
            if (newSelectedBuilding is Building)
            {
                Building building = (Building)newSelectedBuilding;
                _title.text = (building._components._interactableInformation._name);
                _lastClickedInteractable = (Building)newSelectedBuilding;
            }

            if (newSelectedBuilding is Workplace)
            {
                CreateOccupancyCreator((Building)newSelectedBuilding, OccupancyType.Worker);
                CreateWorkObjectCreator((Workplace)newSelectedBuilding);
                CreateOccupancyCreator((Building)newSelectedBuilding, OccupancyType.Visitor);
                Activate();
            }
            else if(newSelectedBuilding is House)
            {
                CreateOccupancyCreator((Building)newSelectedBuilding, OccupancyType.Tenant);
                Activate();
            }
            else if (newSelectedBuilding is Construction)
            {
                CreateSliders(newSelectedBuilding);
                Activate();
            }
            else if(newSelectedBuilding is ResourceInteractable)
            {
                //Draw Resource information
            }
            else if(newSelectedBuilding is Storage)
            {
                CreateStorageCreator((Storage)newSelectedBuilding);
                Activate();
            }
        }
    }

    private void ShowBuildingfunctionMenu(UnitInteractable newSelectedBuilding)
    {

    }

    private void Update()
    {
        if (_lastClickedInteractable != null)
        {
            UpdateSliderStatistics();
            UpdateWorkObjectCreator();
            UpdateStorageCreator();
            UpdateLine();
            UpdatePosition();
        }
    }

    private void UpdateLine()
    {
        //Rotate and change size of line
        Vector3 target = RectTransformUtility.WorldToScreenPoint(Camera.main, _lastClickedInteractable.transform.position);
        Vector3 lookPos = target - _line.transform.position;
        lookPos.z = 0;
        Vector3 rotation = Quaternion.LookRotation(lookPos).eulerAngles;
        _line.transform.eulerAngles = new Vector3(0, 0, rotation.x);
        float dist = Vector3.Distance(target, _line.transform.position);
        RectTransform lineTrans = (RectTransform)_line;
        lineTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dist - _lineLenghtOffset);
    }

    private void UpdatePosition()
    {
        // Apply that offset to get a target position.
        Vector3 target = RectTransformUtility.WorldToScreenPoint(Camera.main, _lastClickedInteractable.transform.position);
        Vector3 targetPosition = target + _targetOffset;
        // Smooth follow.    
        transform.position += (targetPosition - transform.position) * .1f;
    }

    private void CreateSliders(UnitInteractable interactable)
    {
        interactable.UpdateSliderInformation();
        foreach (SliderInformation slider in interactable._sliderStatistics)
        {
            GameObject s = Instantiate(_sliderObject, transform.GetChild(0).transform);
            SliderObject so = s.GetComponent<SliderObject>();
            _instantiated_SliderObjects.Add(so);

            so._title.text = slider._title + ":";
            so._slider.value = slider._value;
        }
    }
    private void UpdateSliderStatistics()
    {
        _lastClickedInteractable.UpdateSliderInformation();

        int i = 0;
        foreach (SliderObject slider in _instantiated_SliderObjects)
        {
            slider._slider.value = _lastClickedInteractable._sliderStatistics[i]._value;
            i++;
        }
    }

    private void CreateDivider()
    {
        GameObject d = Instantiate(_dividerObject, transform.GetChild(0).transform);
        _instantiated_DividerObjects.Add(d);
    }

    private void CreateOccupancyCreator(Building interactable, OccupancyType type)
    {
        foreach (OccupancyInformation capacity in interactable._occupancyInformation)
        {
            if(capacity._occupancyType == type)
            {
                GameObject creatorGameObject = Instantiate(_occupantCreator, transform.GetChild(0).transform);
                OccupantObjectCreator creator = creatorGameObject.GetComponent<OccupantObjectCreator>();
                creator._occupancyInformation = capacity;
                creator._building = interactable;
                creator._title.text = capacity._occupancyType.ToString() + "s:";
                creator.Populate();

                _instantiated_OccupantObjectCreators.Add(creator);
            }
        }
    }

    private void UpdateOccupancyStatistics()
    {
        _lastClickedInteractable.UpdateOccupancyInformation();
        foreach (OccupantObjectCreator creator in _instantiated_OccupantObjectCreators)
        {
            creator.DestroyObjectsInList();
            creator.Populate();
        }
    }

    private void CreateWorkObjectCreator(Workplace newSelectedBuilding)
    {
        GameObject x = Instantiate(_workObjectCreator, transform.GetChild(0).transform);
        WorkObjectCreator creator = x.GetComponent<WorkObjectCreator>();
        _instantiated_WorkObjectCreators.Add(creator);

        creator.CreateStockDivision(newSelectedBuilding);
        creator.CreateProgressDivision(newSelectedBuilding);
        creator.CreateProducedStockDivision(newSelectedBuilding);
    }

    private void UpdateWorkObjectCreator()
    {
        foreach (WorkObjectCreator creator in _instantiated_WorkObjectCreators)
        {
            if (_lastClickedInteractable as Workplace)
            {
                creator.UpdateCreatedStockDivision((Workplace)_lastClickedInteractable);
                creator.UpdateProgressDivision((Workplace)_lastClickedInteractable);
                creator.UpdateStockDivision((Workplace)_lastClickedInteractable);
            }
        }
    }

    private void CreateStorageCreator(Storage newSelectedBuilding)
    {
        GameObject storageObject = Instantiate(_storageCreator, transform.GetChild(0).transform);
        StorageCreator creator = storageObject.GetComponent<StorageCreator>();
        _instantiated_StorageCreators.Add(creator);
        creator.Populate(newSelectedBuilding);
    }

    private void UpdateStorageCreator()
    {
        foreach (StorageCreator creator in _instantiated_StorageCreators)
        {
            creator.UpdateValues(_lastClickedInteractable);
        }
    }

    private void Activate()
    {
        Vector3 target = RectTransformUtility.WorldToScreenPoint(Camera.main, _lastClickedInteractable.transform.position);
        Vector3 targetPosition = target + _targetOffset;
        transform.position = (targetPosition);
        foreach (GameObject g in _popup)
        {
            g.SetActive(true);
        }

        active = true;
    }

    private void Deactivate()
    {
        if(_lastClickedInteractable != null)
        {
            _lastClickedInteractable.OnUnselectUnitInteractable(null);
        }
        _lastClickedInteractable = null;
        foreach (GameObject g in _popup)
        {
            g.SetActive(false);
        }
        DestroyAllObjectsInMenuList();
        active = false;
    }

    private void DestroyAllObjectsInMenuList()
    {
        if (active)
        {
            OccupantObjectCreator[] x = _instantiated_OccupantObjectCreators.ToArray();
            int xx = _instantiated_OccupantObjectCreators.Count;
            for (int i = 0; i < xx; i++)
                Destroy(x[i].gameObject);
            _instantiated_OccupantObjectCreators = new List<OccupantObjectCreator>();

            SliderObject[] y = _instantiated_SliderObjects.ToArray();
            int yy = _instantiated_SliderObjects.Count;
            for (int i = 0; i < yy; i++)
                Destroy(y[i].gameObject);
            _instantiated_SliderObjects = new List<SliderObject>();

            GameObject[] z = _instantiated_DividerObjects.ToArray();
            int zz = _instantiated_DividerObjects.Count;
            for (int i = 0; i < zz; i++)
                Destroy(z[i]);
            _instantiated_DividerObjects = new List<GameObject>();

            WorkObjectCreator[] w = _instantiated_WorkObjectCreators.ToArray();
            int ww = _instantiated_WorkObjectCreators.Count;
            for (int i = 0; i < ww; i++)
                Destroy(w[i].gameObject);
            _instantiated_WorkObjectCreators = new List<WorkObjectCreator>();

            StorageCreator[] v = _instantiated_StorageCreators.ToArray();
            int vv = _instantiated_StorageCreators.Count;
            for (int i = 0; i < vv; i++)
                Destroy(v[i].gameObject);
            _instantiated_StorageCreators = new List<StorageCreator>();
        }
    }

    private void OnDestroy()
    {
        Construction.OnUnitInteractableMouseEvent -= ShowStatistics;
        MenuDeactivator.OnCloseMenu -= Deactivate;
    }
}
