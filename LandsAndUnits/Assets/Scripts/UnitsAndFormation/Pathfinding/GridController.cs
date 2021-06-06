using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitsAndFormation
{
    public class GridController : MonoBehaviour
    {
        public FlowField GenesisField;
        public bool _lateUpdateFlowfield;
        public float _cellRadius = .5f;

        #region SINGLETON PATTERN
        private static GridController _instance;
        public static GridController Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GridController>();
                }

                return _instance;
            }
        }
        #endregion

        public void CreateGenesisField()
        {
            GenesisField = null;
            GenesisField = new FlowField(_cellRadius);
            GenesisField.InitializeGenesis();
        }

        public void FixedUpdate()
        {
            if (_lateUpdateFlowfield)
            {
                _lateUpdateFlowfield = false;
                GenesisField?.UpdateAllUnitInteractableIntergrationLayers();
            }
        }
    }
}
