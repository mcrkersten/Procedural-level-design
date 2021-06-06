using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

[CreateAssetMenu(fileName = "FoilageLayerSettings", menuName = "ScriptableObjects/FoilageLayerSettings", order = 0), System.Serializable]
public class FoilageLayerSettings : ScriptableObject
{
    public List<FoilageLayer> _foilageLayers;
}

[System.Serializable]
public class FoilageLayer
{
    public bool _visableInGameMenu;
    [Header("Spawn settings")]
    public CellType _type;
    public int _foilageAmountPerCell;

    [Header("Spawn percentages")]
    public int _foilageSpawnPercentage;

    [Header("postitional settings")]
    public float _resourceHeightOffset;
    public float _foilageHeightOffset;
    public bool _randomPositionOnCell;

    [Header("Resource Spawn options")]
    public List<ResourceSpawnOptions> _resourceSpawnOptions = new List<ResourceSpawnOptions>();

    [Header("Objects")]
    public List<GameObject> _foilageObjects = new List<GameObject>();
    [HideInInspector] public Layer _layer;
}

[System.Serializable]
public class ResourceSpawnOptions
{
    public ResourceType _type;
    public int _minimum;
    public int _maximum;
    public List<InteractableInformation> _resources = new List<InteractableInformation>();
    [HideInInspector] public int _objectCount = 0;
}

public class Layer
{
    public List<Cell> _cells = new List<Cell>();
    public Layer(Cell firstCell)
    {
        _cells.Add(firstCell);
    }

    public void ShuffleCells()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            Cell temp = _cells[i];
            int randomIndex = Random.Range(i, _cells.Count);
            _cells[i] = _cells[randomIndex];
            _cells[randomIndex] = temp;
        }
    }
}