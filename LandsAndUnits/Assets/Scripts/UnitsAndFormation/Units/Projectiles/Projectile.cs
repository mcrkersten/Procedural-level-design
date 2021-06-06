using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile", order = 1)]
    public class Projectile : ScriptableObject
    {
        public GameObject _projectilePrefab;
        public int _damage;
        public UnitType _source;
    }
}

