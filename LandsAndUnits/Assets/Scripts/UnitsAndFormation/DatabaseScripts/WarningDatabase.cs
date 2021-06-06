using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnitsAndFormation;
[CreateAssetMenu(fileName = "WarningDatabase", menuName = "ScriptableObjects/Database/WarningDatabase", order = 1)]
public class WarningDatabase : ScriptableObject
{
    [System.Serializable]
    public struct ObjectWarningSymbol
    {
        public ObjectWarningType _warningType;
        public int _weight;
        public string _description;
        public Color _color;
        public GameObject _prefab;
    }
    [System.Serializable]
    public struct UnitWarning
    {
        public UnitWarningType _warningType;
        public UnitBehaviourState _unitDecisionBehaviour;
        public string _description;
        public Sprite _icon;
        public int _weight;
        [SerializeField]
        public WarningThreshold[] _warningThreshold;
        [Range(0,100), Tooltip("If value is passed this number, the unit will try to perform the action of this warningtype")]
        public int _actionThreshold; 
    }

    [SerializeField, Header("Object warnings")]
    private List<ObjectWarningSymbol> objectWarning;

    [SerializeField, Header("Unit warnings")]
    private List<UnitWarning> unitWarning;

    [SerializeField]
    public static WarningDatabase instance;

    public void Init()
    {
        instance = this;
    }

    #region ObjectWarning
    public static int GetWarningWeight(ObjectWarningType type)
    {
        return instance.objectWarning.SingleOrDefault(x => x._warningType == type)._weight;
    }

    public static string GetDescription(ObjectWarningType type)
    {
        return instance.objectWarning.SingleOrDefault(x => x._warningType == type)._description;
    }

    public static Color GetColor(ObjectWarningType type)
    {
        return instance.objectWarning.SingleOrDefault(x => x._warningType == type)._color;
    }

    public static GameObject GetPrefab(ObjectWarningType type)
    {
        return instance.objectWarning.SingleOrDefault(x => x._warningType == type)._prefab;
    }
    #endregion

    #region UnitWarning
    public static string GetDescription(UnitWarningType type)
    {
        return instance.unitWarning.SingleOrDefault(x => x._warningType == type)._description;
    }
    public static Sprite GetIcon(UnitWarningType type)
    {
        return instance.unitWarning.SingleOrDefault(x => x._warningType == type)._icon;
    }
    public static WarningThreshold[] GetThresholdArray(UnitWarningType type)
    {
        return instance.unitWarning.SingleOrDefault(x => x._warningType == type)._warningThreshold;
    }
    public static int GetPriorityWeight(UnitWarningType type)
    {
        return instance.unitWarning.SingleOrDefault(x => x._warningType == type)._weight;
    }
    public static int GetPriorityWeight(UnitBehaviourState type)
    {
        return instance.unitWarning.SingleOrDefault(x => x._unitDecisionBehaviour == type)._weight;
    }
    public static int GetActionThreshold(UnitWarningType type)
    {
        return instance.unitWarning.SingleOrDefault(x => x._warningType == type)._actionThreshold;
    }
    #endregion

    public static WarningThreshold GetThresholdMinToMax(UnitWarningType type, float th)
    {
        List<WarningThreshold> threshholds = GetThresholdArray(type).ToList();
        WarningThreshold baseThreshhold;
        baseThreshhold = threshholds[0];
        foreach (WarningThreshold item in threshholds)
        {
            if (th >= item._threshhold)
            {
                if (threshholds.IndexOf(item) == 0)
                    item._firstInList = true;
                if (threshholds.IndexOf(item) == threshholds.Count - 1)
                    item._isLastInList = true;
                return item;
            }
        }
        return baseThreshhold;
    }

    public static WarningThreshold GetThresholdMaxToMin(UnitWarningType type, float th)
    {
        List<WarningThreshold> threshholds = GetThresholdArray(type).ToList();
        WarningThreshold baseThreshhold;
        baseThreshhold = threshholds[threshholds.Count - 1];
        foreach (WarningThreshold item in threshholds)
        {
            if (th <= item._threshhold)
            {
                if (threshholds.IndexOf(item) == threshholds.Count - 1)
                    item._firstInList = true;
                if (threshholds.IndexOf(item) == 0)
                    item._isLastInList = true;
                baseThreshhold = item;
            }
        }
        return baseThreshhold;
    }

    /// <summary>
    /// Returns if 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="currentValue"></param>
    /// <param name="isSmaller"></param>
    /// <returns></returns>
    public static bool TriggeredActionThreshold(UnitWarningType type, float currentValue, bool isSmaller)
    {
        int threshhold = GetActionThreshold(type);

        if(isSmaller)
        {
            if (currentValue > threshhold)
                return true;
            else
                return false;
        }
        else
        {
            if (currentValue < threshhold)
                return true;
            else
                return false;
        }
    }
}

[System.Serializable]
public class WarningThreshold
{
    [Range(0, 100)]
    public float _threshhold;
    public Color _color;
    [HideInInspector] public bool _isLastInList;
    [HideInInspector] public bool _firstInList;
}

