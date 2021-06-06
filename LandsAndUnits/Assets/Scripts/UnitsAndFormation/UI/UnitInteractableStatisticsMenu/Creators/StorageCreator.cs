using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StorageCreator : MonoBehaviour
{
    [SerializeField] private GameObject _assignedStorageObjectPrefab;
    [SerializeField] private GameObject _emptyStorageObjectPrefab;
    private List<GameObject> _instantiated_objects = new List<GameObject>();

    [SerializeField] private GameObject _sliderHolder;
    [SerializeField] private GameObject _storageHolder;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _amount;
    private int _maxStorage = 0;

    public void Populate(Building storage)
    {
        int x = 0;
        _maxStorage = 0;
        foreach (StockpileInformation stock in storage._stockpiles)
        {
            x++;
            CreateAssignedStorageObject(stock);
            _maxStorage += stock._max;
        }

        if (x < 2)
        {
            CreateEmptyStorageObject();
        }
        _amount.text = _maxStorage.ToString() + " Max";
        UpdateValues(storage);
    }

    public void UpdateValues(Building storage)
    {
        int x = 0;
        int currentStorageAmoumt = 0;
        foreach (GameObject item in _instantiated_objects)
        {
            StockObject s = item.GetComponent<StockObject>();
            if (s != null)
            {
                currentStorageAmoumt += storage._stockpiles[x]._currentStockAmount;
                s._amount.text = storage._stockpiles[x]._currentStockAmount.ToString();
                s._icon.sprite = ResourceDatabase.GetResourceIcon(storage._stockpiles[x]._resourceType);
                x++;
            }
        }
        _slider.value = (float)currentStorageAmoumt / (float)_maxStorage;
    }

    private void CreateAssignedStorageObject(StockpileInformation storage)
    {
        GameObject x = Instantiate(_assignedStorageObjectPrefab, _storageHolder.transform);
        StockObject stock = x.GetComponent<StockObject>();
        stock._amount.text = storage._currentStockAmount.ToString();
        stock._icon.sprite = ResourceDatabase.GetResourceIcon(storage._resourceType);
        _instantiated_objects.Add(x);
    }

    private void CreateEmptyStorageObject()
    {
        _instantiated_objects.Add(Instantiate(_emptyStorageObjectPrefab, _storageHolder.transform));
    }

    private void OnDestroy()
    {
        GameObject[] z = _instantiated_objects.ToArray();
        int zz = _instantiated_objects.Count;
        for (int i = 0; i < zz; i++)
            Destroy(z[i]);
        _instantiated_objects = new List<GameObject>();
    }
}
