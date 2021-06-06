using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class StockObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public StockpileInformation _stockInformation;
    public int _index;
    public TextMeshProUGUI _amount;
    public Image _icon;
    public TooltipTrigger _toolTip;
    private bool _lockedToCurser;

    private Vector2 _startPosition;

    public delegate void StockObjectRelease(StockpileInformation storage);
    public static event StockObjectRelease OnStockObjectRelease;

    public void OnPointerDown(PointerEventData eventData)
    {
        _startPosition = this.transform.position;
        _lockedToCurser = true;
    }

    private void Update()
    {
        if (_lockedToCurser)
        {
            Vector2 position = Input.mousePosition;
            transform.position = position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> _raycasts = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, _raycasts);
        bool isHoveringUI = false;

        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        for (int i = 0; i < _raycasts.Count; i++)
        {
            if (_raycasts[i].gameObject.transform != this.transform && !children.Contains(_raycasts[i].gameObject.transform) && _raycasts[i].gameObject.transform.root.CompareTag("UI"))
            {
                Debug.Log(_raycasts[i].gameObject);
                isHoveringUI = true;
            }
        }

        _lockedToCurser = false;
        if (isHoveringUI)
        {
            this.transform.DOMove(_startPosition, .5f).SetEase(Ease.OutBack);
            _lockedToCurser = false;
        }
        else
        {
            OnStockObjectRelease?.Invoke(_stockInformation);
        }
    }
}
