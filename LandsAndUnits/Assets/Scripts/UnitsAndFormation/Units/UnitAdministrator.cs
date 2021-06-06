using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class UnitAdministrator : MonoBehaviour, ISaveable
{
    #region SINGLETON PATTERN
    private static UnitAdministrator _instance;
    public static UnitAdministrator Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UnitAdministrator>();

                if (_instance == null)
                {
                    Debug.LogError("No UnitAdministrator");
                }
            }

            return _instance;
        }
    }
    #endregion

    public delegate void ReleaseUnitFromGroup(List<Unit> units);
    public static event ReleaseUnitFromGroup OnReleaseUnitFromGroup;
    public List<Unit> _units { private set; get; } = new List<Unit>();
    private List<Unit> _toBeAdded = new List<Unit>();

    [SerializeField] private GameObject _harvesterPrefab;
    [SerializeField] private GameObject _workerPrefab;
    [SerializeField] private GameObject _builderPrefab;
    [SerializeField] private GameObject _neutralPrefab;

    private void Start()
    {
        InvokeRepeating("DrainUnitNeeds", 2.0f, 1f);
    }

    /// <summary>
    /// Signup new Unit to the UnitManager
    /// </summary>
    /// <param name="newUnit">the Unit to be signedup</param>
    public void SignupNewUnit(Unit newUnit)
    {
        if (!_units.Contains(newUnit))
            _toBeAdded.Add(newUnit);
        newUnit._unitBrain.SetName(NameGenerator(newUnit._unitType) + (_units.Count + _toBeAdded.Count).ToString());
    }

    public void ReleaseUnitsFromGroup(List<Unit> u)
    {
        OnReleaseUnitFromGroup?.Invoke(u);
    }

    public void RemoveUnitFromGame(Unit u)
    {
        List<Unit> l = new List<Unit>();
        l.Add(u);
        OnReleaseUnitFromGroup?.Invoke(l);

        if (_units.Contains(u))
            _units.Remove(u);
    }

    private string NameGenerator(UnitType type)
    {
        switch (type)
        {
            case UnitType.Neutral:
                return "Neutral willy ";
            case UnitType.Harvester:
                return "Harvester ";
            case UnitType.Builder:
                return "Builder ";
            case UnitType.Worker:
                return "Worker ";
            case UnitType.Enemy:
                return "ENEMY";
            case UnitType.Ally:
                return "Ally ";
            case UnitType.Animal:
                return "Bob ";
            default:
                return "Noname ";
        }
    }

    private void DrainUnitNeeds()
    {
        foreach (Unit unit in _units)
        {
                unit._unitBrain._health.DrainEnergy(UnitTypeDatabase.GetWellbeing(unit._unitType)._energyLossPerTick);
                unit._unitBrain._health.HungerIncrease(UnitTypeDatabase.GetWellbeing(unit._unitType)._hungerIncreasePerTick);
        }
        _units.AddRange(_toBeAdded);
        _toBeAdded = new List<Unit>();
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.GetUnitData(_units);
        return data;
    }

    public void RestoreState(object state)
    {
        GameObject u = null;
        var saveData = (SaveData)state;
        for (int i = 0; i < saveData._unitData.Count; i++)
        {
            Debug.Log("Unit");
            switch ((UnitType)saveData._unitData[i]._unitType)
            {
                case UnitType.Neutral:
                    u = Instantiate(_neutralPrefab);
                    break;
                case UnitType.Harvester:
                    u = Instantiate(_harvesterPrefab);
                    break;
                case UnitType.Builder:
                    u = Instantiate(_builderPrefab);
                    break;
                case UnitType.Worker:
                    u = Instantiate(_workerPrefab);
                    break;
                case UnitType.Enemy:
                    break;
                default:
                    break;
            }

            Vector3 position = new Vector3(saveData._unitData[i]._positionX,
                saveData._unitData[i]._positionY,
                saveData._unitData[i]._positionZ);

            u.transform.position = position;
            Unit unit = u.GetComponent<Unit>();
            unit._ID = saveData._unitData[i]._id;
            unit._unitBrain.UpdateActionState((UnitActionState)saveData._unitData[i]._animationState);
            _units.Add(unit);
        }
    }

    [System.Serializable]
    private struct SaveData
    {
        public List<UnitData> _unitData;

        public void GetUnitData(List<Unit> allUnits)
        {
            _unitData = new List<UnitData>();
            foreach (Unit unit in allUnits)
            {
                Debug.Log(unit._ID);
                UnitData data = new UnitData
                {
                    _id = unit._ID,
                    _unitType = (int)unit._unitType,
                    _animationState = (int)unit._unitBrain._memory._actionState,
                    _positionX = unit.transform.position.x,
                    _positionY = unit.transform.position.y,
                    _positionZ = unit.transform.position.z
                };
                _unitData.Add(data);
            }
        }

        [System.Serializable] public struct UnitData
        {
            public string _id;
            public int _unitType;
            public int _animationState;

            #region position
            public float _positionX;
            public float _positionY;
            public float _positionZ;
            #endregion
        }
    }
}
