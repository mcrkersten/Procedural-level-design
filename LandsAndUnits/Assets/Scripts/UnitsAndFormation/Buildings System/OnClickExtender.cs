using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickExtender : MonoBehaviour
{
    private UnitInteractable _interactable;
    private TooltipTrigger _toolTipTrigger;

    private void Awake()
    {
        _interactable = this.transform.GetComponentInParent<UnitInteractable>();
    }

    private void OnMouseEnter()
    {
        _interactable = this.transform.GetComponentInParent<UnitInteractable>();
        if (_interactable != null)
            _interactable.OnMouseEnter();

        if (_toolTipTrigger != null)
            _toolTipTrigger.OnMouseEnter();
    }

    private void OnMouseUp()
    {
        if(_interactable != null)
            _interactable.OnMouseUp();
    }

    private void OnMouseExit()
    {
        if (_interactable != null)
            _interactable.OnMouseExit();

        if (_toolTipTrigger != null)
            _toolTipTrigger.OnMouseExit();
    }
}
