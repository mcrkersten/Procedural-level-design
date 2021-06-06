using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TutorialEnableButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject _enabledIcon;
    public void OnPointerClick(PointerEventData eventData)
    {

        _enabledIcon.SetActive(!_enabledIcon.activeSelf);
        GameManager._instance._tutorialEnabled = _enabledIcon.activeSelf;
    }
}
