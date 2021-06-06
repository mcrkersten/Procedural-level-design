using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

[CreateAssetMenu(fileName = "ResourceDatabase", menuName = "ScriptableObjects/Database/ResourceDatabase", order = 0)]
public class ResourceDatabase : ScriptableObject
{
    [SerializeField]
    public static ResourceDatabase instance;

    public void Init()
    {
        instance = this;
    }

    public List<ResourceInformation> resources = new List<ResourceInformation>();

    public static Sprite GetResourceIcon(ResourceType type)
    {
        return instance.resources.SingleOrDefault(x => x._resourceType == type)._icon;
    }

    public static GameObject GetResourceObject(ResourceType type)
    {
        return instance.resources.SingleOrDefault(x => x._resourceType == type)._resourcePrefab;
    }

    public static GameObject GetResourceStackObject(ResourceType type)
    {
        return instance.resources.SingleOrDefault(x => x._resourceType == type)._resourceStackPrefab;
    }

    public static ResourceInformation GetResourceInformation(ResourceType type)
    {
        return instance.resources.SingleOrDefault(x => x._resourceType == type);
    }
}
