using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnitsAndFormation;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI _headerField;
    public TextMeshProUGUI _contentField;
    public LayoutElement _layoutElement;

    public int _characterWrapLimit;
    public RectTransform _rectTransform;

    public void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        Vector2 position = Input.mousePosition;
        Vector2 offset = new Vector2(35f, 0f);
        position += offset;

        transform.position = position;
    }

    public void SetText(string content, string header = "")
    {
        Vector2 position = Input.mousePosition;
        Vector2 offset = new Vector2(15f, 0f);
        position += offset;

        transform.position = position;

        if (string.IsNullOrEmpty(header))
        {
            _headerField.gameObject.SetActive(false);
        }
        else
        {
            _headerField.gameObject.SetActive(true);
            _headerField.text = header;
        }

        _contentField.text = content;

        int headerLenght = _headerField.text.Length;
        int contentLenght = _contentField.text.Length;

        _layoutElement.enabled = (headerLenght > _characterWrapLimit || contentLenght > _characterWrapLimit) ? true : false;
    }

    public void SetUnitInformation(Unit unit)
    {

    }

    public void Update()
    {
        if (Application.isEditor)
        {
            int headerLenght = _headerField.text.Length;
            int contentLenght = _contentField.text.Length;

            _layoutElement.enabled = (headerLenght > _characterWrapLimit || contentLenght > _characterWrapLimit) ? true : false;
        }

        Vector2 position = Input.mousePosition;
        Vector2 offset = new Vector2(35f, 0f);
        position += offset;

        transform.position = position;
    }
}
