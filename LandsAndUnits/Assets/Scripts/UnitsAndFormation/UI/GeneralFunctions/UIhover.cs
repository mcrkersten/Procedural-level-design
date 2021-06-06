using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UnitsAndFormation {
    public class UIhover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public delegate void UiHover();
        public static event UiHover OnUiHover;

        bool _isHovered;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            InputManager.Instance.EnterUIHover();
            OnUiHover?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            InputManager.Instance.ExitUIHover();
        }

        public void OnDisable()
        {
            if(_isHovered)
                InputManager.Instance.ExitUIHover();
        }
    }
}

