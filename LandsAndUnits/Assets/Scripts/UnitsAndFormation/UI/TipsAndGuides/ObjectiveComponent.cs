using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveComponent : MonoBehaviour
{
    public Guide _guide;
    public int _index;
    [SerializeField] private Color _completedColor;
    [SerializeField] public TextMeshProUGUI _text;
    [SerializeField] private Image _infill;
    [SerializeField] private Image _box;

    public void OnComplete()
    {
        _text.color = _completedColor;
        _infill.color = _completedColor;
        _box.color = _completedColor;

        _infill.gameObject.SetActive(true);
    }
}
