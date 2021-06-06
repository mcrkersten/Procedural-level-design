using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnitsAndFormation;

namespace UnitsAndFormationUI
{
    public class GroupElement : MonoBehaviour
    {
        public delegate void GroupElementUIDeletion();
        public static event GroupElementUIDeletion OnGroupElementUIDeletion;

        public delegate void GroupElementUISelection(UnitGroup unitGroup, OutlineState state, bool afterSelection = default);
        public static event GroupElementUISelection OnGroupElementUISelection;

        private delegate void ResetColors(int index);
        private static event ResetColors OnResetColor;

        public TextMeshProUGUI _amount;
        public TextMeshProUGUI _groupNumber;
        public Image _icon;
        public Image _border;
        public Image _background;
        public int _groupIndex;

        [SerializeField]
        private Color defaultColor;
        [SerializeField]
        private Color selectedColor;

        public UnitGroup _unitGroup;
        public Vector2 _size;

        private bool isClicked = false;

        private void Awake()
        {
            OnResetColor += ResetColor;
            InputManager.OnNumberButtonpress += OnButtonPress;
        }

        private void OnButtonPress(int button)
        {
            if(button == _groupIndex)
            {
                OnGroupElementUISelection?.Invoke(_unitGroup, OutlineState.Selected);
                SetSelectedColor();
            }
        }

        public void UpdateVisuals(int number)
        {
            _groupIndex = number;
            _amount.text = _unitGroup._units.Count.ToString();
            _groupNumber.text = number.ToString();
            UpdateElementPosition(number -1);
        }

        public void DestroyElement()
        {
            StartCoroutine(DestroyAnimation(1f));
        }

        private IEnumerator DestroyAnimation(float waitTime)
        {
            Color startColorBC = _background.color;
            Color startColorIcon = _icon.color;
            float elapsedTime = 0;
            while (elapsedTime < waitTime)
            {
                Color newColorBC = new Color(startColorBC.r, startColorBC.g, startColorBC.b, Mathf.Lerp(startColorBC.a, 0, (elapsedTime / waitTime)));
                Color newColorIcon = new Color(startColorIcon.r, startColorIcon.g, startColorIcon.b, Mathf.Lerp(startColorIcon.a, 0, (elapsedTime / waitTime)));
                _amount.color = newColorBC;
                _groupNumber.color = newColorBC;
                _background.color = newColorBC;
                _border.color = newColorBC;
                _icon.color = newColorIcon;

                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
            //Destroy
            OnGroupElementUIDeletion?.Invoke();
            Destroy(this.gameObject);
        }

        private void UpdateElementPosition(int index)
        {
            Vector2 position = new Vector2(10 + ((_size.x + 10) * index), 10);
            RectTransform x = GetComponent<RectTransform>();
            x.position = position;
            x.sizeDelta = _size;
        }

        public void OnHover()
        {
            OnGroupElementUISelection?.Invoke(_unitGroup, OutlineState.AboutToBeSelected);
        }

        public void OnDeHover()
        {
            if (!isClicked)
            {
                OnGroupElementUISelection?.Invoke(_unitGroup, OutlineState.UnSelected);
            }
            else
            {
                isClicked = false;
                OnGroupElementUISelection?.Invoke(_unitGroup, OutlineState.UnSelected, true);
            }
        }

        public void OnClick()
        {
            isClicked = true;
            OnGroupElementUISelection?.Invoke(_unitGroup, OutlineState.Selected);
            SetSelectedColor();
        }

        public void OnSelectBySingleUnit()
        {
            OnGroupElementUISelection?.Invoke(_unitGroup, OutlineState.Selected);
            SetSelectedColor();
        }

        public void SetSelectedColor()
        {
            _amount.color = selectedColor;
            _groupNumber.color = selectedColor;
            _border.color = selectedColor;
            OnResetColor?.Invoke(_groupIndex);
        }

        private void ResetColor(int index)
        {
            if(index != _groupIndex)
            {
                _amount.color = defaultColor;
                _groupNumber.color = defaultColor;
                _border.color = defaultColor;
            }
        }

        private void OnDestroy()
        {
            OnResetColor -= ResetColor;
            InputManager.OnNumberButtonpress -= OnButtonPress;
        }
    }
}
