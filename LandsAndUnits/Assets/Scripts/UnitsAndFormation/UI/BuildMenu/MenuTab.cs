using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuTab
{
    [SerializeField] private Transform _frontTab;
    [SerializeField] private Transform _backTab;

    public void ActivateTab()
    {
        _frontTab.gameObject.SetActive(true);
        _backTab.gameObject.SetActive(false);
    }

    public void DeactivateTab()
    {
        _frontTab.gameObject.SetActive(false);
        _backTab.gameObject.SetActive(true);
    }
}
