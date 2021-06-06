using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public enum OutlineState
    {
        UnSelected = 0,
        Selected,
        AboutToBeSelected
    }

    public enum UnitType
    {
        Neutral = 0,
        Harvester,
        Builder,
        Worker,
        Enemy,
        Ally,
        Animal,
        none
    }
    public enum UnitBehaviourState
    {
        None = 0,
        GoingHome,
        IsHome,
        GoingForMeal,
        Eating,
        Visiting,
        WorkplaceTask,
        Working,
        Reset
    }
    public enum UnitActionState
    {
        Idle = 0,
        OutOfRange,
        InRangeOfTarget,
        InRangeOfTargetGroup,
        PerformingAction,
    }
    public enum UnitWarningType
    {
        none = default,
        Housing = 0,
        Food,
        Energy,
        Health
    }

    public enum GroupBehaviourState
    {
        Idle = 0,
        Travel,
        Attack,
        Action,
        Flee,
    }

    public enum ResourceType
    {
        None = default,
        Wood,
        Stone,
        Wheat,
        Bread,
        Berry
    }

    public enum ObjectWarningType
    {
        None = 0,
        NeedsConstruction,
        ScheduledForDestruction
    }

    public enum InteractableType
    {
        None = 0,
        Construction,
        Housing,
        Resource,
        Workplace,
        Storage,
        ResourceStack,
        Other
    }

    public enum CursorType
    {
        None = 0,
        Construct,
        Harvest,
        SelectUnit,
        MoveUnit,
        Placement,
        Demolition
    }

    public enum OccupancyType
    {
        Tenant = 0,
        Visitor,
        Worker,
        None
    }
}