using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public class EnemyManager : MonoBehaviour
    {
        #region SINGLETON PATTERN
        private static EnemyManager _instance;
        public static EnemyManager Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<EnemyManager>();

                    if (_instance == null)
                    {
                        Debug.LogError("No EnemyManager");
                    }
                }

                return _instance;
            }
        }
        #endregion

        public List<UnitGroup> _enemyGroups = new List<UnitGroup>();
        public List<Transform> _groupSpawnPositions = new List<Transform>();
        [SerializeField]
        private GameObject _enemyPrefab;
        public Vector2Int minMaxEnemies;
        public float _scale;
        [ContextMenu("SpawnEnemies")]
        public void SpawnEnemies()
        {
            foreach (Transform pos in _groupSpawnPositions)
            {
                CreateEnemyGroup(pos);
            }
        }

        private void CreateEnemyGroup(Transform trans)
        {

        }
    }
}