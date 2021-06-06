﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    public delegate void OpenBuildMenu();
    public static event OpenBuildMenu OnOpenBuildMenu;

    [SerializeField]
    private Image _tab, _icon;
    [SerializeField]
    private GameObject _toActivate;
    [SerializeField] Guide _guideType;

    private void Start()
    {
        MenuDeactivator.OnCloseMenu += Activate;
        DemolitionButton.OnStartDemolition += Deactivate;
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
        OnOpenBuildMenu?.Invoke();
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

    private void OnDestroy()
    {
        MenuDeactivator.OnCloseMenu -= Activate;
        DemolitionButton.OnStartDemolition -= Deactivate;
    }
}
