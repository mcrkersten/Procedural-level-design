using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabSelectorButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public int _nr;
    public delegate void TabClick(int x);
    public static event TabClick OnTabClick;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTabClick?.Invoke(_nr);
    }
}
