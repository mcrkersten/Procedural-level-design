using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public class AnimalGroup : UnitGroup
    {
        public delegate void RequestNewNavigation(AnimalGroup x);
        public static event RequestNewNavigation OnRequestNewNavigation;
        public int _navIndex = 0;

        public AnimalGroup(List<Unit> units, UnitType type, GroupBehaviourState state, Vector3 targetPosition) : base(units, type, state, targetPosition)
        {

        }
    }
}
