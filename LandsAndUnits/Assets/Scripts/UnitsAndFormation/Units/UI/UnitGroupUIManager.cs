using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitsAndFormation;

namespace UnitsAndFormationUI
{
    public class UnitGroupUIManager : MonoBehaviour
    {
        public GameObject _troopIconPrefab;
        private List<TroopIcon> _troopIcons = new List<TroopIcon>();

        [SerializeField]
        private RectTransform _troopParent;
        [SerializeField]
        private RectTransform _troopWindow;
        [SerializeField]
        public ExpandArrow _expandArrow;

        private bool _expand;



        private void Awake()
        {
            TroopIcon.OnTroopDeletion += UpdateElements;
            UnitSelection.OnSelectGroupWithSingleUnit += CheckIfGroupContainsUnit;
            UpdateTroopWindow();
        }

        private void Update()
        {
            if(_expandArrow._isExpanded != _expand)
            {
                _expand = _expandArrow._isExpanded;
                if (_expand)
                {
                    ExpandWindow();
                }
                else
                {
                    RetractWindow();
                }

                UpdateTroopWindow();
            }
        }

        private void ExpandWindow()
        {
            _expandArrow._sprite.eulerAngles = new Vector3(0, 0, 180);
        }

        private void RetractWindow()
        {
            _expandArrow._sprite.eulerAngles = new Vector3(0, 0, 0);
        }

        public void CreateNewElement(UnitGroup group)
        {
            GameObject x = Instantiate(_troopIconPrefab, _troopParent);
            TroopIcon g = x.GetComponent<TroopIcon>();

            g._unitGroup = group;
            _troopIcons.Add(g);
            g.UpdateVisuals(_troopIcons.Count);
            g.SetSelectedColor();
            UpdateElements();
        }

        private void UpdateTroopWindow()
        {
            if(_troopIcons.Count <= 3)
            {
                _expandArrow.gameObject.SetActive(false);
                _troopWindow.sizeDelta = new Vector2(145 + (65 * _troopIcons.Count), _troopWindow.sizeDelta.y);
                _troopParent.sizeDelta = new Vector2(230, _troopParent.sizeDelta.y);
            }
            else
            {
                _expandArrow.gameObject.SetActive(true);

                if (_expand)
                {
                    _troopWindow.sizeDelta = new Vector2(160 + (65 * _troopIcons.Count), _troopWindow.sizeDelta.y);
                    _troopParent.sizeDelta = new Vector2(230 + (65 * _troopIcons.Count), _troopParent.sizeDelta.y);
                }
                else{
                    _troopWindow.sizeDelta = new Vector2(160 + (65 * 3f), _troopWindow.sizeDelta.y);
                    _troopParent.sizeDelta = new Vector2(230, _troopParent.sizeDelta.y);
                }
                    
            }
        }

        public void DestroyElement(UnitGroup group)
        {
            TroopIcon x = null;
            foreach (TroopIcon item in _troopIcons)
            {
                if(item._unitGroup == group)
                {
                    x = item;
                    break;
                }
            }

            if(x != null)
            {
                _troopIcons.Remove(x);
                x.DestroyElement();
            }
        }

        public void UpdateElements()
        {
            int x = 1;
            foreach (TroopIcon element in _troopIcons)
            {
                element.UpdateVisuals(x++);
            }
            UpdateTroopWindow();
        }

        private void CheckIfGroupContainsUnit(Unit u)
        {
            foreach (TroopIcon TI in _troopIcons)
            {
                if (TI._unitGroup._units.Contains(u))
                {
                    TI.OnSelectBySingleUnit();
                }
            }
        }

        private void OnDestroy()
        {
            GroupElement.OnGroupElementUIDeletion -= UpdateElements;
            UnitSelection.OnSelectGroupWithSingleUnit -= CheckIfGroupContainsUnit;
        }
    }
}
