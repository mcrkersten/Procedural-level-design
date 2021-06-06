using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class HighlightComponent : MonoBehaviour, IPointerClickHandler, IGuideButton
{
    [SerializeField] private MenuHighlight _highlight;
    [SerializeField] private MenuType _type;
    private Color _baseColor;
    private bool _isHighlighted;

    private Image _image;
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Awake()
    {
        TipAndGuidesScreen.OnTriggerMenuHighlight += OnHighlightCall;
        switch (_type)
        {
            case MenuType.Image:
                GetImage();
                break;
            case MenuType.Text:
                GetText();
                break;
        }
    }

    // Update is called once per frame
    public void OnHighlightCall(MenuHighlight highlight)
    {
        if(_highlight == highlight)
        {
            switch (_type)
            {
                case MenuType.Image:
                    _image.DOColor(DatabaseManager._instance._colourDatabase.Highlight, 1f).SetLoops(-1, LoopType.Yoyo);
                    break;
                case MenuType.Text:
                    _text.DOColor(DatabaseManager._instance._colourDatabase.Highlight, 1f).SetLoops(-1, LoopType.Yoyo);
                    break;
            }
            _isHighlighted = true;

        }
        else
        {
            switch (_type)
            {
                case MenuType.Image:
                    _image.DOKill();
                    _image.color = _baseColor;
                    break;
                case MenuType.Text:
                    _text.DOKill();
                    _text.color = _baseColor;
                    break;
            }
            _isHighlighted = false;
        }
    }

    private void GetImage()
    {
        _image = GetComponent<Image>();
        if (_image != null)
        {
            _baseColor = _image.color;
        }
    }

    private void GetText()
    {
        _text = GetComponent<TextMeshProUGUI>();
        if(_text != null)
        {
            _baseColor = _text.color;
        }
    }

    private void OnDestroy()
    {
        TipAndGuidesScreen.OnTriggerMenuHighlight -= OnHighlightCall;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isHighlighted)
        {
            _isHighlighted = false;

            switch (_type)
            {
                case MenuType.Image:
                    _image.DOKill();
                    _image.color = _baseColor;
                    break;
                case MenuType.Text:
                    _text.DOKill();
                    _text.color = _baseColor;
                    break;
            }
        }
    }

    private enum MenuType
    {
        Image = 0,
        Text
    }
}
