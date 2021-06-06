using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DemolitionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    public delegate void StartDemolition();
    public static event StartDemolition OnStartDemolition;

    [SerializeField] private Image _tab, _icon;

    private void Start()
    {
        MenuDeactivator.OnCloseMenu += Activate;
        MenuTrigger.OnOpenBuildMenu += Deactivate;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tab.color = new Color(_tab.color.r, _tab.color.g, _tab.color.b, 1f);
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, 1f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _tab.color = new Color(_tab.color.r, _tab.color.g, _tab.color.b, .35f);
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, .5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DemolitionSystem._demolitionActive = true;
        TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.DEMOLISH, 0);
        TipsAndGuidesManager._instance.AdvanceGuide(Guide.DEMOLISH, 1);
        OnStartDemolition?.Invoke();
        Deactivate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tab.color = new Color(_tab.color.r, _tab.color.g, _tab.color.b, .35f);
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, .5f);
    }

    private void Activate()
    {
        this.gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
