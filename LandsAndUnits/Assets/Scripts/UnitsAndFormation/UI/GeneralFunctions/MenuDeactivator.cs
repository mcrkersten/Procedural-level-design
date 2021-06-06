using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuDeactivator : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

    [SerializeField]
    public delegate void CloseMenu();
    public static event CloseMenu OnCloseMenu;

    public bool disableMouseOnExit;


    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.visible = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnCloseMenu?.Invoke();
    }
}
