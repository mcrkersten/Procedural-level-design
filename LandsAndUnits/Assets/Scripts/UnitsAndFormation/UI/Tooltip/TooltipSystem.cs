using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem _current;
    public Tooltip _tooltip;
    public void Awake()
    {
        _current = this;
    }

    public static void Show(string content, string header = "")
    {
        _current._tooltip.SetText(content, header);
        _current._tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if(_current != null && _current._tooltip != null)
            _current._tooltip.gameObject.SetActive(false);
    }
}
