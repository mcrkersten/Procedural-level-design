using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

[CreateAssetMenu(fileName = "ResourceScriptable", menuName = "ScriptableObjects/ResourceScriptable", order = 1), System.Serializable]
public class ResourceInformation : ScriptableObject
{
    [Header("Resource object variables")]
    public ResourceType _resourceType;
    public GameObject _resourcePrefab;
    public GameObject _resourceStackPrefab;
    public GameObject _particlePrefab;
    public Sprite _icon;

    [Header("Resource harvest variables")]
    public int _hitsToHarvest;
    public int _harvestTillCooldown;
    public float _cooldownTime;
    public int _instantiatedGameObjectsPerHarvest;
    public int _resourceAmountPerInstantiatedGameObject;

    [Header("Resource production variables")]
    public List<CostInformation> _resourcesNeededToProduce;
    public int _workEnergyNeededToProduce;

    public bool _consumable;
    public int _nutritionalValue;

    public bool _isHarvestable;
}

[System.Serializable]
public class CostInformation
{
    public ResourceInformation _resourceInformation;
    public int _amount;
}
