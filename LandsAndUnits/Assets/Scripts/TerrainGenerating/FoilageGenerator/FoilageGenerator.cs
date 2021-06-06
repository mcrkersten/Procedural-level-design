using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;
public class FoilageGenerator : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static FoilageGenerator _instance;
    public static FoilageGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<FoilageGenerator>();
            }

            return _instance;
        }
    }
    #endregion

    public bool _generateForMenu;

    public delegate void FoilageGenerated();
    public static event FoilageGenerated OnFoilageGenerated;

    private List<GameObject> _generatedObjects = new List<GameObject>();
    [SerializeField] private LayerMask _mask;
    public FoilageLayerSettings _foilageLayers;
    public float _generationProgress { private set; get; }
    public bool _generationIsDone { private set; get; }



    public void InstantiateAllObjects()
    {
        DestroyAll();
        float progress = 0;
        FlowField field = GridController.Instance.GenesisField;
        foreach (Cell cell in field._allCells)
        {
            FoilageLayer layer = _foilageLayers._foilageLayers.SingleOrDefault(layer => layer._type == cell._type);
            if (layer?._layer != null)
            {
                layer._layer._cells.Add(cell);
                continue;
            }
            if(layer != null)
                layer._layer = new Layer(cell);
        }

        foreach (FoilageLayer layer in _foilageLayers._foilageLayers)
        {
            if (layer._layer != null)
            {
                layer._layer.ShuffleCells();
                foreach (Cell cell in layer._layer?._cells)
                {
                    CreateOjects(_foilageLayers._foilageLayers.Single(layer => layer._type == cell._type), cell);
                }
                progress++;
                _generationProgress = ((float)progress / (float)_foilageLayers._foilageLayers.Count);
            }
        }

        if(!_generateForMenu)
            _generationIsDone = true;

        OnFoilageGenerated?.Invoke();
    }

    private void CreateOjects(FoilageLayer layer, Cell cell)
    {
        if(layer._resourceSpawnOptions.Count > 0)
        {
            if(!CreateResources(layer, cell))
            {
                CreateFoilage(layer, cell);
            }
        }
        else
        {
            CreateFoilage(layer, cell);
        }
    }

    private bool CreateResources(FoilageLayer layer, Cell cell)
    {
        RaycastHit groundDetection;
        Vector3 randomPositionOnCell = Vector3.zero;

        if (!layer._randomPositionOnCell)
            randomPositionOnCell = new Vector3(Random.Range(-.9f, .9f), 0, Random.Range(-.9f, .9f));

        InteractableInformation information = GetRandomObject(layer);
        GameObject resourceToSpawn = null;
        if (information != null)
            resourceToSpawn = information._completedPrefab;

        if (resourceToSpawn != null && Physics.Raycast(cell._worldPosition + new Vector3(0, 50, 0) + randomPositionOnCell, Vector3.down, out groundDetection, Mathf.Infinity, _mask))
        {
            ResourceInteractable point = resourceToSpawn.GetComponent<ResourceInteractable>();
            GameObject instantiatedObject = Instantiate(resourceToSpawn, cell._worldPosition + new Vector3(0, layer._resourceHeightOffset + groundDetection.point.y + point._yVectorOffset, 0), new Quaternion(), null);
            instantiatedObject.GetComponent<UnitInteractable>()._interactableID = information._interactableID;
            instantiatedObject.GetComponent<UnitInteractable>()._isMenuInstance = _generateForMenu;

            if (point._useGroundNormal)
                instantiatedObject.transform.up = groundDetection.normal;

            instantiatedObject.transform.Rotate(new Vector3(0, Random.Range(0f, 360f)));
            _generatedObjects.Add(instantiatedObject);

            return true;
        }
        return false;
    }

    private InteractableInformation GetRandomObject(FoilageLayer layer)
    {
        int resourceTypeIndex = Random.Range(0, layer._resourceSpawnOptions.Count);
        int objectIndex = Random.Range(0, layer._resourceSpawnOptions[resourceTypeIndex]._resources.Count);

        if (layer._resourceSpawnOptions[resourceTypeIndex]._objectCount < layer._resourceSpawnOptions[resourceTypeIndex]._minimum)
        {
            layer._resourceSpawnOptions[resourceTypeIndex]._objectCount++;
            return layer._resourceSpawnOptions[resourceTypeIndex]._resources[objectIndex];
        }
        else
        {
            for (int i = 0; i < layer._resourceSpawnOptions[resourceTypeIndex]._resources.Count; i++)
            {
                if (layer._resourceSpawnOptions[resourceTypeIndex]._objectCount < layer._resourceSpawnOptions[resourceTypeIndex]._maximum)
                {
                    layer._resourceSpawnOptions[resourceTypeIndex]._objectCount++;
                    return layer._resourceSpawnOptions[resourceTypeIndex]._resources[objectIndex];
                }
            }
        }
        return null;
    }

    private void CreateFoilage(FoilageLayer layer, Cell cell)
    {
        int random = Random.Range(0, 100);
        if (random < layer._foilageSpawnPercentage)
        {
            int objectCount = Random.Range(1, layer._foilageAmountPerCell);
            for (int i = 0; i < objectCount; i++)
            {
                RaycastHit groundDetection;
                Vector3 offset = Vector3.zero;
                if (!layer._randomPositionOnCell)
                {
                    offset = new Vector3(Random.Range(-.9f, .9f), 0, Random.Range(-.9f, .9f));
                }

                if (Physics.Raycast(cell._worldPosition + new Vector3(0, 50, 0) + offset, Vector3.down, out groundDetection, Mathf.Infinity, _mask))
                {
                    int randomIndex = Random.Range(0, layer._foilageObjects.Count);
                    GameObject instantiatedObject = Instantiate(layer._foilageObjects[randomIndex], cell._worldPosition + new Vector3(0, layer._foilageHeightOffset + groundDetection.point.y, 0), new Quaternion(), null);
                    instantiatedObject.transform.up = groundDetection.normal;

                    instantiatedObject.transform.Rotate(new Vector3(0, Random.Range(0f, 360f)));
                    _generatedObjects.Add(instantiatedObject);
                }
            }
        }
    }

    [ContextMenu("Destroy")]
    public void DestroyAll()
    {
        GameObject[] z = _generatedObjects.ToArray();
        int zz = _generatedObjects.Count;
        for (int i = 0; i < zz; i++)
            Destroy(z[i]);

        _generatedObjects = new List<GameObject>();

        foreach (FoilageLayer item in _foilageLayers._foilageLayers)
        {
            item._layer = null;
            foreach (ResourceSpawnOptions options in item._resourceSpawnOptions)
            {
                options._objectCount = 0;
            }
        }
    }
}
