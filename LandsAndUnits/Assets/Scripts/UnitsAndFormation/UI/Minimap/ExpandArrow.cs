using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnitsAndFormation;

public class ExpandArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public bool _isExpanded;
    public RectTransform _sprite;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isExpanded = !_isExpanded;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
