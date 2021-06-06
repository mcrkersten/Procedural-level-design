using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

[CreateAssetMenu(fileName = "Interactable", menuName = "ScriptableObjects/InteractableInformation", order = 4), System.Serializable]
public class InteractableInformation : ScriptableObject 
{
    //Type information
    [HideInInspector] public string _interactableID;
    [HideInInspector] public InteractableType _interactableType;
    [HideInInspector] public GameObject _constructionPrefab;
    [HideInInspector] public GameObject _completedPrefab;

    //Construction information
    [SerializeField] public List<StorageInformation> _costs;
    [HideInInspector] public int _constructionPoints;

    //UI information
    [HideInInspector] public string _name;
    [HideInInspector] public string _description;
    [HideInInspector] public Sprite _image;
    [HideInInspector] public Vector2Int _size;

    //Capacity information
    [HideInInspector] public int _occupancyCapacity;
    [HideInInspector] public int _visitorCapacity;

    [SerializeField] public List<ResourceInformation> _thisBuildingProduces;

    [SerializeField] public List<StorageInformation> _storageTypes;

    public void Calculate()
    {
        _storageTypes = new List<StorageInformation>();
        foreach (ResourceInformation toProduce in _thisBuildingProduces)
        {
            StorageInformation si = new StorageInformation(toProduce._resourceType, 0);
            _storageTypes.Add(si);
        }

        foreach (ResourceInformation toProduce in _thisBuildingProduces)
        {
            foreach (CostInformation neededToProduce in toProduce._resourcesNeededToProduce)
            {
                StorageInformation si = new StorageInformation(neededToProduce._resourceInformation._resourceType, 0);
                _storageTypes.Add(si);
            }
        }
    }
}

[System.Serializable]
public class StorageInformation
{
    public ResourceType _resourceType;
    public int _maxStorage;

    public StorageInformation(ResourceType information, int amount)
    {
        _resourceType = information;
        _maxStorage = amount;
    }
}
