using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

public class BuildBuildingsLibrary : MonoBehaviour, ISaveable
{
    #region SINGLETON PATTERN
    private static BuildBuildingsLibrary _instance;
    public static BuildBuildingsLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<BuildBuildingsLibrary>();

                if (_instance == null)
                {
                    Debug.LogError("No BuildBuildingsLibrary");
                }
            }

            return _instance;
        }
    }
    #endregion

    #region Instantiated
    public List<ResourceInteractable> _instantiatedResources { private set; get; }
    public List<House> _instantiatedHouses { private set; get; }
    public List<Workplace> _instantiatedWorkplaces { private set; get; }
    public List<Storage> _instantiatedStorages { private set; get; }
    public List<Construction> _instantiatedConstructions { private set; get; }
    public List<ResourceStack> _instantiatedResourceStacks { private set; get; }
    public List<UnitInteractable> _instantiatedOthers { private set; get; }
    public List<UnitInteractable> _AllInstantiatedInteractables
    {
        get
        {
            List<UnitInteractable> all = new List<UnitInteractable>();
            all.AddRange(_instantiatedResources);
            all.AddRange(_instantiatedHouses);
            all.AddRange(_instantiatedWorkplaces);
            all.AddRange(_instantiatedStorages);
            all.AddRange(_instantiatedResourceStacks);
            all.AddRange(_instantiatedConstructions);
            return all;
        }
    }
    #endregion

    private static List<Object> _resources = new List<Object>();
    private static List<Object> _housing = new List<Object>();
    private static List<Object> _stacks = new List<Object>();
    private static List<Object> _storages = new List<Object>();
    private static List<Object> _wordplaces = new List<Object>();
    private static List<Object> _other = new List<Object>();

    private void Awake()
    {
        LoadCleanLists();
        LoadScriptableobjects();


        UnitInteractable.OnDeclareInteractable += AddInteractableToList;
        UnitInteractable.OnUnDeclareInteractable += RemoveInteractableFromList;
    }

    private void LoadScriptableobjects()
    {
        _resources = Resources.LoadAll("UnitInteractables/NaturalResources", typeof(InteractableInformation)).ToList();
        _housing = Resources.LoadAll("UnitInteractables/Housing", typeof(InteractableInformation)).ToList();
        _stacks = Resources.LoadAll("UnitInteractables/Stacks", typeof(InteractableInformation)).ToList();
        _storages = Resources.LoadAll("UnitInteractables/Storages", typeof(InteractableInformation)).ToList();
        _storages = Resources.LoadAll("UnitInteractables/Workplaces", typeof(InteractableInformation)).ToList();
        _other = Resources.LoadAll("UnitInteractables/Other", typeof(InteractableInformation)).ToList();
    }

    private void LoadCleanLists()
    {
        _instantiatedResources = new List<ResourceInteractable>();
        _instantiatedHouses = new List<House>();
        _instantiatedWorkplaces = new List<Workplace>();
        _instantiatedStorages = new List<Storage>();
        _instantiatedConstructions = new List<Construction>();
        _instantiatedResourceStacks = new List<ResourceStack>();
        _instantiatedOthers = new List<UnitInteractable>();
    }

    private void AddInteractableToList(UnitInteractable interactable)
    {
        switch (interactable._type)
        {
            case InteractableType.Construction:
                _instantiatedConstructions.Add((Construction)interactable);
                break;
            case InteractableType.Housing:
                _instantiatedHouses.Add((House)interactable);
                break;
            case InteractableType.Resource:
                _instantiatedResources.Add((ResourceInteractable)interactable);
                break;
            case InteractableType.Workplace:
                _instantiatedWorkplaces.Add((Workplace)interactable);
                break;
            case InteractableType.Storage:
                _instantiatedStorages.Add((Storage)interactable);
                break;
            case InteractableType.ResourceStack:
                _instantiatedResourceStacks.Add((ResourceStack)interactable);
                break;
            case InteractableType.Other:
                _instantiatedOthers.Add(interactable);
                break;
            default:
                break;
        }
    }

    private void RemoveInteractableFromList(UnitInteractable interactable)
    {
        switch (interactable._type)
        {
            case InteractableType.Construction:
                _instantiatedConstructions.Remove((Construction)interactable);
                break;
            case InteractableType.Housing:
                _instantiatedHouses.Remove((House)interactable);
                break;
            case InteractableType.Resource:
                _instantiatedResources.Remove((ResourceInteractable)interactable);
                break;
            case InteractableType.Workplace:
                _instantiatedWorkplaces.Remove((Workplace)interactable);
                break;
            case InteractableType.Storage:
                _instantiatedStorages.Remove((Storage)interactable);
                break;
            case InteractableType.ResourceStack:
                _instantiatedResourceStacks.Remove((ResourceStack)interactable);
                break;
            case InteractableType.Other:
                _instantiatedOthers.Remove(interactable);
                break;
            default:
                break;
        }
    }

    public UnitInteractable GetUnknownInheritanceUnitInteractable(string ID)
    {
        return _AllInstantiatedInteractables.SingleOrDefault(interactable => interactable._ID == ID);
    }

    public House GetHouseWithID(string ID)
    {
        return _instantiatedHouses.SingleOrDefault(house => house._ID == ID);
    }

    public Workplace GetWorkplaceWithID(string ID)
    {
        return _instantiatedWorkplaces.SingleOrDefault(workplace => workplace._ID == ID);
    }

    public ResourceInteractable GetResourceWithID(string ID)
    {
        return _instantiatedResources.SingleOrDefault(resource => resource._ID == ID);
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.GetResourceHarvestingPointInformation(_instantiatedResources);
        data.GetHousingInformation(_instantiatedHouses);
        data.GetWorkplaceInformation(_instantiatedWorkplaces);
        data.GetStorageInformation(_instantiatedStorages);
        data.GetConstructionInformation(_instantiatedConstructions);
        data.GetOtherInformation(_instantiatedOthers);
        return data;
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        if (saveData._resources != null)
            for (int i = 0; i < saveData._resources.Count; i++)
                InstantiateObject(saveData._resources[i]);

        if (saveData._houses != null)
            for (int i = 0; i < saveData._houses.Count; i++)
                InstantiateObject(saveData._houses[i]);

        if (saveData._workplaces != null)
            for (int i = 0; i < saveData._workplaces.Count; i++)
                InstantiateObject(saveData._workplaces[i]);

        if (saveData._storages != null)
            for (int i = 0; i < saveData._storages.Count; i++)
                InstantiateObject(saveData._storages[i]);

        if (saveData._construction != null)
            for (int i = 0; i < saveData._construction.Count; i++)
                InstantiateObject(saveData._construction[i]);

        if(saveData._resourceStack != null)
            for (int i = 0; i < saveData._resourceStack.Count; i++)
                InstantiateObject(saveData._resourceStack[i]);

        if (saveData._others != null)
            for (int i = 0; i < saveData._others.Count; i++)
                InstantiateObject(saveData._others[i]);
    }

    private void InstantiateObject(SaveData.InteractableData data)
    {
        Vector3 position = new Vector3(data._positionX, data._positionY, data._positionZ);
        Quaternion rotation = new Quaternion(data._rotationX, data._rotationY, data._rotationZ, data._rotationW);

        GameObject toInstantiate = GetBuilding(data._interactableID, data._isConstruction);
        GameObject inst  = Instantiate(toInstantiate, position,rotation, null);
        inst.GetComponent<UnitInteractable>()._interactableID = data._interactableID;
    }

    private static GameObject GetBuilding(string id, bool isConstruction)
    {
        Debug.Log(id);
        InteractableInformation information = null;
        GameObject toReturn = null;
        switch (id[0])
        {
            case '0':
                information = ((InteractableInformation)_resources.SingleOrDefault(inter => ((InteractableInformation)inter)._interactableID == id));
                break;
            case '1':
                information = ((InteractableInformation)_housing.SingleOrDefault(inter => ((InteractableInformation)inter)._interactableID == id));
                break;
            case '2':
                information = ((InteractableInformation)_wordplaces.SingleOrDefault(inter => ((InteractableInformation)inter)._interactableID == id));
                break;
            case '3':
                information = ((InteractableInformation)_storages.SingleOrDefault(inter => ((InteractableInformation)inter)._interactableID == id));
                break;
            case '4':
                information = ((InteractableInformation)_other.SingleOrDefault(inter => ((InteractableInformation)inter)._interactableID == id));
                break;
            case '5':
                information = ((InteractableInformation)_stacks.SingleOrDefault(inter => ((InteractableInformation)inter)._interactableID == id));
                break;
        }

        if (isConstruction)
            toReturn = information._constructionPrefab;
        else
            toReturn = information._completedPrefab;

        return toReturn;
    }

    [System.Serializable] private struct SaveData
    {

        public List<InteractableData> _resources;
        public List<InteractableData> _houses;
        public List<InteractableData> _workplaces;
        public List<InteractableData> _storages;
        public List<InteractableData> _construction;
        public List<InteractableData> _resourceStack;
        public List<InteractableData> _others;

        public void GetResourceHarvestingPointInformation(List<ResourceInteractable> points)
        {
            _resources = new List<InteractableData>();
            foreach (ResourceInteractable item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = false,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _resources.Add(data);
                Debug.Log(data._interactableID);
            }
        }
        public void GetHousingInformation(List<House> points)
        {
            _houses = new List<InteractableData>();
            foreach (House item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = false,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _houses.Add(data);
                Debug.Log(data._interactableID);
            }
        }
        public void GetWorkplaceInformation(List<Workplace> points)
        {
            _workplaces = new List<InteractableData>();
            foreach (Workplace item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = false,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _workplaces.Add(data);
                Debug.Log(data._interactableID);
            }
        }
        public void GetStorageInformation(List<Storage> points)
        {
            _storages = new List<InteractableData>();
            foreach (Storage item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = false,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _storages.Add(data);
                Debug.Log(data._interactableID);
            }
        }
        public void GetConstructionInformation(List<Construction> points)
        {
            _construction = new List<InteractableData>();
            foreach (Construction item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = true,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _construction.Add(data);
                Debug.Log(data._interactableID);
            }
        }
        public void GetStackInformation(List<ResourceStack> points)
        {
            _resourceStack = new List<InteractableData>();
            foreach (ResourceStack item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = false,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _resourceStack.Add(data);
                Debug.Log(data._interactableID);
            }
        }
        public void GetOtherInformation(List<UnitInteractable> points)
        {
            _others = new List<InteractableData>();
            foreach (UnitInteractable item in points)
            {
                InteractableData data = new InteractableData
                {
                    _id = item._ID,
                    _interactableID = item._interactableID,
                    _isConstruction = false,

                    _positionX = item.transform.position.x,
                    _positionY = item.transform.position.y,
                    _positionZ = item.transform.position.z,

                    _rotationX = item.transform.rotation.x,
                    _rotationY = item.transform.rotation.y,
                    _rotationZ = item.transform.rotation.z,
                    _rotationW = item.transform.rotation.w,
                };
                _others.Add(data);
                Debug.Log(data._interactableID);
            }
        }

        [System.Serializable] public struct InteractableData
        {
            public string _id;
            public string _interactableID;
            public bool _isConstruction;

            #region position
            public float _positionX;
            public float _positionY;
            public float _positionZ;
            #endregion

            #region rotation
            public float _rotationX;
            public float _rotationY;
            public float _rotationZ;
            public float _rotationW;
            #endregion
        }
    }

    private void OnDestroy()
    {
        UnitInteractable.OnDeclareInteractable -= AddInteractableToList;
        UnitInteractable.OnUnDeclareInteractable -= RemoveInteractableFromList;
    }
}
