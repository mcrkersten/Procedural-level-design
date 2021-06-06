using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager _instance;
    public UnitTypeDatabase _unitTypeDatabase;
    public WarningDatabase _warningDatabase;
    public ColourDatabase _colourDatabase;
    public ResourceDatabase _resourceDatabase;
    public BuildingTypeDatabase _buildingTypeDatabase;
    public void Awake()
    {
        _instance = this;
        _unitTypeDatabase.Init();
        _warningDatabase.Init();
        _colourDatabase.Init();
        _resourceDatabase.Init();
        _buildingTypeDatabase.Init();
    }
}
