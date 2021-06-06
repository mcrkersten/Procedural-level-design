using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitsAndFormation
{
    public class UnitGroup
    {
        public int _intergrationLayer { private set; get; }
        public Vector3 _targetPosition { private set; get; }

        public List<Unit> _units = new List<Unit>();
        public GroupBehaviourState _state { private set; get; }
        public UnitType _type { private set; get; }
        public List<Transform> _targets;
        private int _travelingUnits = 0;
        private FlowField _flowfield;
        private List<IEnumerator> _numerators = new List<IEnumerator>();

        //Constructor
        public UnitGroup(List<Unit> units, UnitType type, GroupBehaviourState state, Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _type = type;
            _units.AddRange(units);
            UnitAdministrator.OnReleaseUnitFromGroup += ReleaseUnitFromGroup;
            UnitInteractable.OnUnitInteractableMouseEvent += OnUnitInteractableMouseEvent;
            _flowfield = GridController.Instance.GenesisField;
            _intergrationLayer = _flowfield.AddNewIntergrationLayer();
            ChangeGroupState(state);
            _flowfield.UpdateSignleIntergrationLayer(targetPosition, _intergrationLayer);
        }

        public void UpdateNavigation(Vector3 destination, List<Vector3> positions, Vector3 direction)
        {
            _targetPosition = destination;

            _flowfield.UpdateSignleIntergrationLayer(destination, _intergrationLayer);
  
            int x = 0;
            foreach (Unit u in _units)
            {
                ExitBuilding(u);
                u._unitBrain._targetInformation.SetTargetRotation(direction);
                u._unitBrain._targetInformation.SetTargetPosition(positions[x++]);
            }
        }

        public Vector3 GetCenterPositionOfThisGroup()
        {
            Vector3 sumPos = Vector3.zero;
            foreach (Unit u in _units)
            {
                sumPos += u.transform.position;
            }
            return sumPos / _units.Count;
        }

        public bool GroupIsSelected(List<Unit> selection)
        {
            if (selection.Count != _units.Count)
                return false;

            foreach (Unit unit in selection)
            {
                //If the list contains a unit that is not ours
                if (!_units.Contains(unit))
                    return false;
            }
            return true;
        }

        public void NewAction(Vector3 destination, List<Transform> group)
        {
            _targetPosition = destination;
            _targets = group;

            _flowfield.UpdateSignleIntergrationLayer(destination, _intergrationLayer);

            Vector3 t;
            if (group.Count > 1)
            {
                t = HelperFunctions.GetCenterOfTransforms(group);
            }
            else
            {
                t = group[0].position;
            }
            foreach (Unit u in _units)
            {
                ExitBuilding(u);
                u._unitBrain._targetInformation.SetTargetPosition(t);
            }
        }

        public void NewAction(UnitInteractable interactable)
        {         
            _targetPosition = interactable._targetTransform.position;
            _flowfield.UpdateSignleIntergrationLayer(interactable, _intergrationLayer);
            _targets = new List<Transform>();
            _targets.Add(interactable.transform);
            Debug.Log(interactable);

            foreach (Unit u in _units)
            {
                ExitBuilding(u);
                u._unitBrain._targetInformation.SetTargetPosition(_targetPosition);
            }
        }

        public void ChangeGroupState(GroupBehaviourState state)
        {
            _state = state;

            foreach (Unit unit in _units)
            {
                switch (state)
                {
                    case GroupBehaviourState.Idle:
                        break;
                    case GroupBehaviourState.Travel:
                        unit._unitBrain._targetInformation.ResetTargetTransform();
                        unit._unitBrain.UpdateActionState(UnitActionState.OutOfRange);
                        break;
                    case GroupBehaviourState.Attack:
                        break;
                    case GroupBehaviourState.Action:
                        unit._unitBrain._targetInformation.SetTargetPosition(PickTargetFromGroup(unit).position);
                        unit._unitBrain.UpdateActionState(UnitActionState.OutOfRange);

                        break;
                    case GroupBehaviourState.Flee:
                        break;
                    default:
                        break;
                }
            }
        }

        public void BehaviourTick()
        {
            _travelingUnits = 0;
            foreach (Unit unit in _units)
            {
                //If the unit makes Behaviour decisions and it want's to do something else.
                if (unit._unitBrain._memory._hasFreeWill && unit._unitBrain._memory._behaviourState != UnitBehaviourState.None)
                {
                    //A player input execution for movement has priority.
                    if (_state != GroupBehaviourState.Travel)
                    {
                        UnitBehaviour(unit);
                        continue;
                    }

                }

                GroupBehaviour(unit);
            }

            //If there are no traveling units left, we set this group to the Idle state.
            if (_state == GroupBehaviourState.Travel)
            {
                if (_travelingUnits == 0)
                {
                    ChangeGroupState(GroupBehaviourState.Idle);
                }
            }
        }

        public void SetOutlineStateOfGroup(OutlineState state)
        {
            foreach (Unit unit in _units)
            {
                unit.SetOutlineState(state);
            }
        }

        private void UnitBehaviour(Unit unit)
        {
            switch (unit._unitBrain._memory._behaviourState)
            {
                case UnitBehaviourState.None:
                    //We have nothing to do.
                    //Look for work.
                    unit._unitBrain.SearchForWork();
                    break;
                case UnitBehaviourState.GoingHome:
                    TravelToInteractable(unit, unit._unitBrain._memory._home);
                    break;
                case UnitBehaviourState.IsHome:
                    break;
                case UnitBehaviourState.GoingForMeal:
                    TravelToInteractable(unit, unit._unitBrain._memory._taskInteractable);
                    break;
                case UnitBehaviourState.Eating:
                    break;
                case UnitBehaviourState.Visiting:
                    UnitVisitorMode(unit, 300);
                    break;
                case UnitBehaviourState.Reset:
                    unit._unitBrain.UpdateBehaviour(UnitBehaviourState.None);
                    unit._unitBrain._targetInformation.SetTargetPosition(_targetPosition);
                    break;
                case UnitBehaviourState.WorkplaceTask:
                    TravelToInteractable(unit, unit._unitBrain._memory._taskInteractable);
                    break;
                case UnitBehaviourState.Working:
                    break;
                default:
                    break;
            }
        }

        //Makes the unit behave as the group wishes.
        private void GroupBehaviour(Unit unit)
        {
            switch (_state)
            {
                case GroupBehaviourState.Idle:
                    IdleBehaviour(unit);
                    break;
                case GroupBehaviourState.Travel:
                    _travelingUnits += TravelBehaviour(unit);
                    break;
                case GroupBehaviourState.Attack:
                    AttackBehaviour(unit);
                    break;
                case GroupBehaviourState.Action:
                    ActionBehaviour(unit);
                    break;
                case GroupBehaviourState.Flee:
                    break;
                default:
                    break;
            }
        }

        private void OnUnitInteractableMouseEvent(UnitInteractable interactable, OutlineState state)
        {
            //If the interactable is used by this group
            if(_targets != null && _targets.Contains(interactable.transform))
            {
                foreach (Unit u in _units)
                {
                    //Only for the units that are working on this interactable
                    if(u._unitBrain._targetInformation.Position() == interactable.transform.position)
                    {
                        //Only change state when unit is unselected or about to be selected
                        if(state == OutlineState.UnSelected && u._lastState != OutlineState.Selected)
                            u.SetOutlineState(state);
                        else if(state == OutlineState.AboutToBeSelected && u._lastState == OutlineState.UnSelected)
                            u.SetOutlineState(state);
                    }
                }
            }
        }

        private void ReleaseUnitFromGroup(List<Unit> u)
        {
            List<Unit> contains = new List<Unit>();
            foreach (Unit unit in u)
            {
                if (_units.Contains(unit))
                {
                    contains.Add(unit);
                }
            }

            foreach (Unit toRemove in contains)
            {
                toRemove._unitBrain.UpdateActionState(UnitActionState.Idle);
                _units.Remove(toRemove);
            }
        }

        #region NormalBehaviours
        private void IdleBehaviour(Unit unit)
        {

        }

        private int TravelBehaviour(Unit unit)
        {
            int x = 0;
            if(!unit.MoveUnit(_targetPosition, _intergrationLayer))
                x = 1;
            else
            {
                unit._unitBrain.UpdateActionState(UnitActionState.Idle);
                unit.StopMoving(true);
            }
            return x;
        }

        private void AttackBehaviour(Unit unit)
        {

        }

        private void ActionBehaviour(Unit unit)
        {
            switch (unit._unitBrain._memory._actionState)
            {
                case UnitActionState.Idle:
                    break;
                case UnitActionState.OutOfRange:
                    if (UnitIsInRangeOfTargetGroup(unit))
                        unit._unitBrain.UpdateActionState(UnitActionState.InRangeOfTargetGroup);
                    else
                        unit.MoveUnit(_targetPosition, _intergrationLayer);
                    break;

                case UnitActionState.InRangeOfTargetGroup:
                    Transform pickedTarget = PickTargetFromGroup(unit);
                    UnitInteractable inter = GetUnitInteractableFromTransform(pickedTarget);
                    if (inter != null && !inter.isDemolished)
                    {
                        //If we are not in range of the "entrance" or "set targetposition"
                        if (unit._unitBrain._targetInformation.IsInRangeOfTransform(pickedTarget))
                        {
                            unit._unitBrain._targetInformation.SetTargetTransform(pickedTarget);
                            unit._unitBrain.UpdateActionState(UnitActionState.InRangeOfTarget);
                        }
                        else
                        {
                            unit.MoveUnit(inter._targetTransform.position, inter._intergrationLayer);
                        }
                    }
                    else
                    {
                        unit._unitBrain.UpdateActionState(UnitActionState.Idle);
                    }
                    break;
                case UnitActionState.InRangeOfTarget:
                    unit.StopMoving(false);
                    unit._unitBrain._behaviourModule.ExecuteInteractableBehaviour();
                    unit._unitBrain.UpdateActionState(UnitActionState.PerformingAction);
                    break;
                case UnitActionState.PerformingAction:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns a transform that the given Unit will use as target
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private Transform PickTargetFromGroup(Unit unit)
        {
            Transform closestTarget = FindClosestTargetinTargetGroup(unit.transform.position);
            UnitInteractable b = closestTarget?.GetComponent<UnitInteractable>();
            if (closestTarget.GetComponent<UnitInteractable>() != null)
                closestTarget = b._targetTransform;
            return closestTarget;
        }

        private UnitInteractable GetUnitInteractableFromTransform(Transform theTransform)
        {
            if (theTransform == null)
                return null;
            UnitInteractable interactable = theTransform.GetComponent<UnitInteractable>();
            if (interactable == null)
                interactable = theTransform.GetComponentInParent<UnitInteractable>();
            return interactable;
        }
        #endregion

        #region AbnormalBehaviours
        private void ExitBuilding(Unit unit)
        {
            if (unit._unitBrain != null)
            {
                unit._unitBrain.UpdateBehaviour(UnitBehaviourState.None);
                unit._unitBrain._behaviourModule.ExitBuilding();
            }
        }

        private void TravelToInteractable(Unit unit, UnitInteractable interactable)
        {
            switch (unit._unitBrain._memory._actionState)
            {
                case UnitActionState.Idle:
                    break;
                case UnitActionState.OutOfRange:
                    if (unit._unitBrain._targetInformation.IsInRangeOfInteractable(interactable))
                        unit._unitBrain.UpdateActionState(UnitActionState.InRangeOfTarget);
                    else
                        unit.MoveUnit(interactable.transform.position, interactable._intergrationLayer);
                    break;
                case UnitActionState.InRangeOfTargetGroup:
                    break;
                case UnitActionState.InRangeOfTarget:
                    unit.StopMoving(false);
                    unit._unitBrain._behaviourModule.ExecuteInteractableBehaviour();
                    unit._unitBrain.UpdateActionState(UnitActionState.PerformingAction);
                    break;
                case UnitActionState.PerformingAction:
                    break;
                default:
                    break;
            }
        }
        #endregion

        public bool UnitIsInRangeOfTargetGroup(Unit unit)
        {
            Vector2 target = new Vector2(_targetPosition.x, _targetPosition.z);
            Vector2 position = new Vector2(unit.transform.position.x, unit.transform.position.z);

            float distanceToTarget = Vector2.Distance(target, position);

            //If not reached minimum 'perform action' distance | walk
            if (distanceToTarget > unit._unitBrain._behaviourModule._minimumTargetGroupDistance - (unit._unitBrain._behaviourModule._minimumTargetGroupDistance / 10f))
                return false;
            else
                return true;
        }

        private Transform FindClosestTargetinTargetGroup(Vector3 unitPosition)
        {
            Transform closestTarget = null;
            float d = float.MaxValue;
            foreach (Transform target in _targets)
            {
                if (target != null)
                {
                    float ed = Vector3.Distance(unitPosition, target.position);
                    if (ed < d)
                    {
                        d = ed;
                        closestTarget = target;
                    }
                }
            }
            return closestTarget;
        }

        private void UnitVisitorMode(Unit unit, int time)
        {
            unit._unitBrain._visitingTime++;
            if(time < unit._unitBrain._visitingTime)
            {
                //Now we may exit building and reset behaviour
                unit._unitBrain._visitingTime = 0;
                unit._unitBrain._behaviourModule.BackToGroupBehaviour();
            }
        }

        public void UnsubScribe()
        {
            UnitAdministrator.OnReleaseUnitFromGroup -= ReleaseUnitFromGroup;
        }
    }
}
