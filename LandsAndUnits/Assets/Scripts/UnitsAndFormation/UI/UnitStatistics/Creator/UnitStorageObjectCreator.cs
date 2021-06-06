using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class UnitStorageObjectCreator : MonoBehaviour
{
    [SerializeField] private StatisticsCanvas _statisticsCanvas; // <- has selected unit

    [SerializeField] private GameObject _assignedStorageObject;
    [SerializeField] private GameObject _emptydStorageObject;
    private List<GameObject> _instantiatedObjects = new List<GameObject>();

    private void Start()
    {
        StockObject.OnStockObjectRelease += OnStockObjectEject;
    }

    private void OnEnable()
    {
        FillStorage(_statisticsCanvas._selectedUnit);
    }

    private void Update()
    {
        int x = 0;
        foreach (StockpileInformation pile in _statisticsCanvas._selectedUnit._storagCompartment._storage)
        {
            StockObject stock = _instantiatedObjects[x].GetComponent<StockObject>();
            if (stock != null)
            {
                stock._amount.text = pile._currentStockAmount.ToString();
                stock._icon.sprite = ResourceDatabase.GetResourceIcon(pile._resourceType);
                stock._stockInformation = pile;
            }
            else
            {
                if(x == 0)
                {
                    FillStorage(_statisticsCanvas._selectedUnit);
                }
            }
            x++;
        }
        for (int i = x; i < _instantiatedObjects.Count; i++)
        {
            StockObject stock = _instantiatedObjects[x].GetComponent<StockObject>();
            if (stock != null)
            {
                FillStorage(_statisticsCanvas._selectedUnit);
            }
        }
    }

    private void FillStorage(Unit unit)
    {
        DestroyInstantiatedObjects();
        int index = 0;
        foreach (StockpileInformation storage in unit._storagCompartment._storage)
        {
            GameObject storageObject = Instantiate(_assignedStorageObject, this.transform);
            StockObject stock = storageObject.GetComponent<StockObject>();
            stock._amount.text = storage._currentStockAmount.ToString();
            stock._icon.sprite = ResourceDatabase.GetResourceIcon(storage._resourceType);
            stock._index = index;
            _instantiatedObjects.Add(storageObject);
            index++;
        }
        for (int i = index; i < 4; i++)
        {
            GameObject storageObject = Instantiate(_emptydStorageObject, this.transform);
            _instantiatedObjects.Add(storageObject);
        }
    }

    private void OnStockObjectEject(StockpileInformation storage)
    {
        _statisticsCanvas._selectedUnit.DropResource(storage._currentStockAmount ,storage._resourceType, true);
        FillStorage(_statisticsCanvas._selectedUnit);
    }

    private void DestroyInstantiatedObjects()
    {
        GameObject[] x = _instantiatedObjects.ToArray();
        int count = _instantiatedObjects.Count;
        for (int i = 0; i < count; i++)
            Destroy(x[i]);
        _instantiatedObjects = new List<GameObject>();
    }
}
