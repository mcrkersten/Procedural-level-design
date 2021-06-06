using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int _woodAmount
    {
        get {
            return WoodAmount;
        }
    }
    private int WoodAmount;

    public int _foodAmount
    {
        get {
            return FoodAmount;
        }
    }
    private int FoodAmount;

    public int _stoneAmount
    {
        get {
            return StoneAmount;
        }
    }
    private int StoneAmount;

    public delegate void ResourceUserInterfaceUpdate(ResourceType resource);
    public static event ResourceUserInterfaceUpdate OnResourceUserInterfaceUpdate;

    public void Awake()
    {
        WoodAmount = 0;
        FoodAmount = 0;
        StoneAmount = 0;

        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {

    }
}

[System.Serializable]
public class ResourceSpend{
    public ResourceType _resourceType;
    public int _amount;
    public ResourceSpend(ResourceType information, int amount)
    {
        _resourceType = information;
        _amount = amount;
    }
}
