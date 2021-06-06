using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

[CreateAssetMenu(fileName = "BuildingTypeDatabase", menuName = "ScriptableObjects/Database/BuildingTypeDatabase", order = 1)]
public class BuildingTypeDatabase : ScriptableObject
{
    [SerializeField]
    public static BuildingTypeDatabase instance;

    [System.Serializable]
    public struct BuildingTypeInformation
    {
        public InteractableType _type;
        public Sprite _icon;
    }

    public void Init()
    {
        instance = this;
    }

    [SerializeField] private List<BuildingTypeInformation> buildings = new List<BuildingTypeInformation>();

    public static Sprite GetBuildingTypeIcon(InteractableType type)
    {
        return instance.buildings.SingleOrDefault(x => x._type == type)._icon;
    }
}
