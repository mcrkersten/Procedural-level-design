using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormationUI;
using System.Linq;

namespace UnitsAndFormation
{
    public class GroupManager : MonoBehaviour, ISaveable
    {
        #region SINGLETON PATTERN
        private static GroupManager _instance;
        public static GroupManager Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GroupManager>();

                    if (_instance == null)
                    {
                        Debug.LogError("No Manager");
                    }
                }

                return _instance;
            }
        }
        #endregion

        public UnitGroupUIManager _unitUIManager;
        public EnemyManager _enemyManager;
        public List<UnitGroup> _groups = new List<UnitGroup>();
        public List<Unit> _selectedUnits = new List<Unit>();
        public UnitGroup _selectedGroup;
        public UnitType _typeOfSelection = UnitType.Neutral;

        public void Awake()
        {
            UnitInteractable.OnUnitInteractableMouseEvent += OnUnitInteractibleMouseEvent;
        }
        public void OnUnitSelection()
        {
            _selectedGroup = GetUnitGroup();
        }

        public void PlayerMovementInput(Vector3 destination, List<Vector3> unittargetPositions, Vector3 rot)
        {
            if (_selectedGroup == null)
            {
                NewGroupFromSelection();
            }

            for (int i = 0; i < _selectedUnits.Count; i++)
            {
                if (!_selectedGroup._units.Contains(_selectedUnits[i]))
                {
                    NewGroupFromSelection();
                    break;
                }
                else if(_selectedGroup._units.Count != _selectedUnits.Count)
                {
                    NewGroupFromSelection();
                    break;
                }
            }

            List<Vector3> positions = SortUnitPositions(unittargetPositions, _selectedGroup);
            _selectedGroup._targets = null;
            UpdateNavigationOfGroup(_selectedGroup, destination, positions, rot);
        }

        public UnitGroup GetGroupFromSingleUnit(Unit unit)
        {
            foreach (UnitGroup group in _groups)
            {
                if (group._units.Contains(unit))
                {
                    return group;
                }
            }
            return null;
        }

        private void NewGroupFromSelection()
        {
            UnitSelection x = UnitSelection.Instance;
            x.UnSelectAllUnitOutlines(_selectedUnits.Count);
            _selectedGroup = CreateNewGroup();
            _selectedGroup.SetOutlineStateOfGroup(OutlineState.Selected);
        }

        /// <summary>
        /// Gets called on every interactable-mouse-event
        /// </summary>
        /// <param name="interactable">The interactable that was clicked</param>
        /// <param name="state">The state of the click</param>
        private void OnUnitInteractibleMouseEvent(UnitInteractable interactable, OutlineState state)
        {
            if (state == OutlineState.Selected)
            {
                List<Unit> unitsFromInteractable = GetAllUnitsFromInteractable(interactable.transform);
                //if no group is selected and we do detect units assigned to this interactable
                if (unitsFromInteractable.Count != 0 && _selectedGroup == null)
                {
                    foreach (UnitGroup group in _groups)
                    {
                        //If there is just one group assigned, select that group.
                        if (group.GroupIsSelected(unitsFromInteractable))
                        {
                            _selectedGroup = group;
                            _selectedUnits = unitsFromInteractable;
                        }
                        else //Create new group
                        {
                            _selectedUnits = unitsFromInteractable;
                            NewGroupFromSelection();
                            break;
                        }
                    }

                    foreach (Unit unit in _selectedUnits)
                    {
                        unit.SetOutlineState(state);
                    }
                }
                else
                {
                    if(_selectedUnits.Count != 0)
                        AssignUnitsToInteractable(interactable);
                }
            }
        }

        /// <summary>
        /// Get a list of all units currently assinged to this interactable
        /// </summary>
        /// <param name="interactableTransform">The transform of the interactable</param>
        /// <returns></returns>
        private List<Unit> GetAllUnitsFromInteractable(Transform interactableTransform)
        {
            //Select working units
            List<Unit> temp = new List<Unit>();
            foreach (UnitGroup group in _groups)
            {
                if (group._targets != null && group._targets.Contains(interactableTransform))
                {
                    foreach (Unit unit in group._units)
                    {
                        if (unit._unitBrain._targetInformation.Position() == interactableTransform.position)
                        {
                            temp.Add(unit);
                        }
                    }
                }
            }
            return temp;
        }

        private void AssignUnitsToInteractable(UnitInteractable interactable)
        {
            List<UnitInteractable> interactables = new List<UnitInteractable>();
            List<Transform> transforms = new List<Transform>();
            transforms.Add(interactable.transform);

            interactables.Add(interactable);
            if(_selectedGroup != null)
            {
                _selectedGroup.NewAction(interactable);
                _selectedGroup.ChangeGroupState(GroupBehaviourState.Action);
            }
            else
            {
                NewGroupFromSelection();
                _selectedGroup.NewAction(interactable);
                _selectedGroup.ChangeGroupState(GroupBehaviourState.Action);
                foreach (Unit unit in _selectedUnits)
                    unit.SetOutlineState(OutlineState.Selected);
            }
        }

        private List<Vector3> SortUnitPositions(List<Vector3> positions, UnitGroup group)
        {
            List<Vector3> sortedPositions = new List<Vector3>();
            //Get average position of all positions
            Vector3 avarage = new Vector3();
            for (int i = 0; i < positions.Count; i++)
                avarage += positions[i];
            avarage /= positions.Count;

            List<Unit> sortedList = new List<Unit>();
            List<Unit> unSortedList = new List<Unit>();

            #region sortList
            unSortedList.AddRange(group._units);

            int count = unSortedList.Count;
            for (int i = 0; i < count; i++)
            {
                float maximumDistance = float.MaxValue;
                Unit sortedUnit = null;
                foreach (Unit unsortedUnit in unSortedList)
                {
                    Vector3 unsortedUnitPosition = new Vector3(unsortedUnit.transform.position.x, 0, unsortedUnit.transform.position.z);
                    float distance = Vector3.Distance(unsortedUnitPosition, avarage);
                    if(distance < maximumDistance)
                    {
                        maximumDistance = distance;
                        sortedUnit = unsortedUnit;
                    }
                }
                sortedList.Add(sortedUnit);
                unSortedList.Remove(sortedUnit);
            }
            #endregion

            #region Set positions
            foreach (Unit sortedUnit in sortedList)
            {
                float minimumDistance = 0;
                Vector3 position = Vector3.zero;
                foreach (Vector3 point in positions)
                {
                    Vector3 sortedUnitPosition = new Vector3(sortedUnit.transform.position.x, 0, sortedUnit.transform.position.z);
                    float distance = Vector3.Distance(sortedUnitPosition, point);

                    if(distance > minimumDistance)
                    {
                        minimumDistance = distance;
                        position = point;
                    }
                }
                sortedPositions.Add(position);
                positions.Remove(position);
            }
            return sortedPositions;
            #endregion
        }

        private void FixedUpdate()
        {
            List<UnitGroup> toDelete = new List<UnitGroup>();
            foreach (UnitGroup d in _groups)
            {
                d.BehaviourTick();

                //If group is empty, set to delete
                if (d._units.Count == 0)
                    toDelete.Add(d);
            }

            //Detete group
            foreach (UnitGroup delete in toDelete)
            {
                delete.UnsubScribe();
                _unitUIManager.DestroyElement(delete);
                _groups.Remove(delete);
            }
        }

        private void UpdateNavigationOfGroup(UnitGroup group, Vector3 destination, List<Vector3> positions, Vector3 rotation)
        {
            if (positions.Count != group._units.Count)
            {
                Debug.LogError("positions.count is not equal the unit.count " + group._units.Count + "units " + positions.Count + "positions", this);
                Debug.Break();
                return;
            }

            group.UpdateNavigation(destination, positions, rotation);
            group.ChangeGroupState(GroupBehaviourState.Travel);
        }

        private UnitGroup CreateNewGroup()
        {
            UnitAdministrator.Instance.ReleaseUnitsFromGroup(_selectedUnits);
            UnitGroup x = new UnitGroup(_selectedUnits, _selectedUnits[0]._unitType, GroupBehaviourState.Idle, Vector3.zero);
            _groups.Add(x);
            _selectedGroup = x;
            _unitUIManager.CreateNewElement(x);
            return x;
        }

        private UnitGroup CreateNewGroup(List<Unit> units, GroupBehaviourState state, Vector3 targetposition)
        {
            UnitGroup x = new UnitGroup(units, units[0]._unitType, state, targetposition);
            _groups.Add(x);
            _selectedGroup = x;
            _unitUIManager.CreateNewElement(x);
            return x;
        }

        private UnitGroup GetUnitGroup()
        {
            foreach (UnitGroup group in _groups)
            {
                if (group.GroupIsSelected(_selectedUnits))
                    return group;
            }
            return null;
        }

        public void OnClickedEnemyUnit(Unit enemy)
        {

        }

        public void OnSphereSelection(Vector3 center, List<Transform> targets)
        {
            if (_selectedGroup != null)
            {
                _selectedGroup.NewAction(center, targets);
                _selectedGroup.ChangeGroupState(GroupBehaviourState.Action);
            }
        }

        public object CaptureState()
        {
            SaveData data = new SaveData();
            data.GetGroupData(_groups);
            return data;
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;
            for (int i = 0; i < saveData._groupData.Count; i++)
            {
                List<Unit> _units = new List<Unit>();
                foreach (string id in saveData._groupData[i]._unitIDs)
                {
                    Unit unit = UnitAdministrator.Instance._units.SingleOrDefault(u => u._ID == id);
                    _units.Add(unit);
                }
                CreateNewGroup(_units, (GroupBehaviourState)saveData._groupData[i]._state, saveData.ReturnPositionData(saveData._groupData[i]));
            }
        }

        [System.Serializable] private struct SaveData
        {
            public List<GroupData> _groupData;

            public void GetGroupData(List<UnitGroup> groups)
            {
                _groupData = new List<GroupData>();
                foreach (UnitGroup group in groups)
                {

                    List<string> data = new List<string>();
                    foreach (Unit unit in group._units)
                    {
                        data.Add(unit._ID);
                    }

                    GroupData gdata = new GroupData()
                    {
                        _unitIDs = data,
                        _state = (int)group._state,
                        _targetPositionX = group._targetPosition.x,
                        _targetPositionY = group._targetPosition.y,
                        _targetPositionZ = group._targetPosition.z,
                    };

                    _groupData.Add(gdata);
                }
            }

            public Vector3 ReturnPositionData(GroupData data)
            {
                return new Vector3(data._targetPositionX, data._targetPositionY, data._targetPositionZ);
            }

            [System.Serializable] public struct GroupData
            {
                public int _state;
                public List<string> _unitIDs;

                public float _targetPositionX;
                public float _targetPositionY;
                public float _targetPositionZ;
            }
        }
    }
}

