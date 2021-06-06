using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using System.Linq;

[CreateAssetMenu(fileName = "UnitTypeDatabase", menuName = "ScriptableObjects/Database/UnitTypeDatabase", order = 1)]
public class UnitTypeDatabase : ScriptableObject
{
    [System.Serializable]
    public struct UnitTypeLibrary
    {
        public UnitType _type;
        public Sprite _unitTypeIcon;
        public UnitWellbeing _baseWellbeing;

        //General
        [Header("General")]
        public float _movementSpeed;
        public float _meeleeRange;
        [Range(1f, 10f)]
        public ushort _unitWeight;

        //Base module
        [Header("Base")]
        public GameObject _toolPrefab;
        public float _minimumTargetGroupDistance;

        [Header("Action")]
        [Range(1f, 10f)]
        public float _cooldownTime;
        public float _animationTime;
        public int _actionStrength;
        public int _maxStorage;
    }

    [System.Serializable]
    public struct UnitWellbeing
    {
        public int _maxHealth;
        public int _maxEnergy;
        public int _maxHunger;

        [Header("Max Drain Multipliers")]
        public int _maxEnergyMultiplier;
        public int _maxHungerMultiplier;

        [Header("Per tick")]
        public int _energyLossPerTick;
        public int _hungerIncreasePerTick;
    }

    [SerializeField]
    private List<UnitTypeLibrary> unitTypeLibraries;

    [SerializeField]
    public static UnitTypeDatabase instance;

    public void Init()
    {
        instance = this;
    }

    #region General
    public static Sprite GetGetUnitTypeIcon(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._unitTypeIcon;
    }
    public static float GetMovementSpeed(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._movementSpeed;
    }
    public static float GetMeeleeRange(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._meeleeRange;
    }
    public static ushort GetUnitWeight(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._unitWeight;
    }
    public static UnitWellbeing GetWellbeing(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._baseWellbeing;
    }
    #endregion

    #region Base
    public static GameObject GetTool(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._toolPrefab;
    }
    public static float GetMinimumTargetGroupDistance(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._minimumTargetGroupDistance;
    }
    #endregion;

    #region Action
    public static float GetCooldownTime(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._cooldownTime;
    }
    public static float GetAnimationTime(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._animationTime;
    }
    public static int GetActionStrenght(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._actionStrength;
    }

    public static int GetMaxStorage(UnitType type)
    {
        return instance.unitTypeLibraries.SingleOrDefault(x => x._type == type)._maxStorage;
    }
    #endregion
}
