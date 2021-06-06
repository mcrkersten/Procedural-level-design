using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnitsAndFormation;

public class TroopIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public delegate void TroopDeletion();
    public static event TroopDeletion OnTroopDeletion;

    public delegate void TroopSelection(UnitGroup unitGroup, OutlineState state, bool afterSelection = default);
    public static event TroopSelection OnTroopSelection;

    private delegate void ResetColors(int index);
    private static event ResetColors OnResetColor;

    public int _groupIndex;
    public UnitGroup _unitGroup;
    private bool _isSelected;
    public Vector2 _size;
    public TextMeshProUGUI _amount;

    public Image _border;
    public Color _selectedColor;
    public Color _hoverColor;
    public Color _neutralColor;
    private Color _lastColor;

    public void Awake()
    {
        OnResetColor += ResetColor;
        InputManager.OnNumberButtonpress += OnButtonPress;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _lastColor = _border.color;
        _border.color = _hoverColor;
        OnTroopSelection?.Invoke(_unitGroup, OutlineState.AboutToBeSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTroopSelection?.Invoke(_unitGroup, OutlineState.Selected);
        SetSelectedColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _border.color = _lastColor;
        if (!_isSelected)
        {
            OnTroopSelection?.Invoke(_unitGroup, OutlineState.UnSelected);
        }
        else
        {
            _isSelected = false;
            OnTroopSelection?.Invoke(_unitGroup, OutlineState.UnSelected, true);
        }
    }

    public void UpdateVisuals(int number)
    {
        _groupIndex = number;
        _amount.text = _unitGroup._units.Count.ToString();
        UpdateElementPosition(number - 1);
    }

    private void UpdateElementPosition(int index)
    {
        Vector3 position = new Vector3(65 + (65 * index),0 ,0);
        RectTransform x = GetComponent<RectTransform>();
        x.localPosition = position;
        x.sizeDelta = _size;
    }

    private void OnButtonPress(int button)
    {
        if (button == _groupIndex)
        {
            OnTroopSelection?.Invoke(_unitGroup, OutlineState.Selected);
            SetSelectedColor();
        }
    }

    public void SetSelectedColor()
    {
        OnResetColor?.Invoke(_groupIndex);
        _border.color = _selectedColor;
        _lastColor = _border.color;

    }

    private void ResetColor(int index)
    {
        if (index != _groupIndex)
        {
            _border.color = _neutralColor;
        }
    }

    public void OnSelectBySingleUnit()
    {
        OnTroopSelection?.Invoke(_unitGroup, OutlineState.Selected);
        SetSelectedColor();
    }

    public void DestroyElement()
    {
        OnTroopDeletion?.Invoke();
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        OnResetColor -= ResetColor;
        InputManager.OnNumberButtonpress -= OnButtonPress;
    }
}
