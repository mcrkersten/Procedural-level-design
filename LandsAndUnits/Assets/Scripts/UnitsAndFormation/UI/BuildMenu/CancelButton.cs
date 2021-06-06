using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    public delegate void CancelBuildButtonClick();
    public static event CancelBuildButtonClick OnCancelBuildButtonClick;

    public delegate void CancelDemolitionButtonClick();
    public static event CancelDemolitionButtonClick OnCancelDemolitionButtonClick;

    [SerializeField]
    private Image _tab, _icon;

    [SerializeField] private List<GameObject> _tabs = new List<GameObject>();

    private void Start()
    {
        BuildMenuButton.OnActivateBuilding += Activate;
        DemolitionButton.OnStartDemolition += Activate;
        MenuTrigger.OnOpenBuildMenu += Deactivate; 
        Deactivate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverStart();
        Cursor.visible = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DemolitionSystem._demolitionActive == true)
        {
            DemolitionSystem._demolitionActive = false;
            foreach (GameObject item in _tabs)
                item.SetActive(true);
            OnCancelDemolitionButtonClick?.Invoke();
        }
        else
        {
            OnCancelBuildButtonClick?.Invoke();
        }

        OnReset();
        Deactivate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnReset();
    }

    private void Activate()
    {
        this.gameObject.SetActive(true);
    }

    private void OnHoverStart()
    {
        _tab.color = new Color(.8f, .8f, .8f, 1f);
        _icon.color = new Color(.85f, .85f, .85f, 1f);
    }

    private void OnDown()
    {
        _tab.color = new Color(.5f, .5f, .5f, 1f);
        _icon.color = new Color(.7f, .7f, .7f, 1f);
    }

    private void OnReset()
    {
        _tab.color = new Color(1f, 1f, 1f, .25f);
        _icon.color = new Color(1f, 1f, 1f, 1f);
    }

    private void Deactivate()
    {
        this.gameObject.SetActive(false);
        Cursor.visible = true;
    }
}
