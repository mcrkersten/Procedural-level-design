using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CornerObject : MonoBehaviour
{
    public GameObject _icon;
    public GameObject _text;

    private List<GameObject> _instantiated_objects = new List<GameObject>();

    public void InstantiateIcon(Sprite icon)
    {
        GameObject x = Instantiate(_icon, this.transform);
        _instantiated_objects.Add(x);
        Image i = x.GetComponent<Image>();
        i.sprite = icon;
    }

    public void InstantiateText(string text)
    {
        GameObject x = Instantiate(_text, this.transform);
        _instantiated_objects.Add(x);
        TextMeshProUGUI tmp = x.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
    }

    public void DestroyInstantiatedObjects()
    {
        GameObject[] x = _instantiated_objects.ToArray();
        int count = _instantiated_objects.Count;
        for (int i = 0; i < count; i++)
            Destroy(x[i]);
        _instantiated_objects = new List<GameObject>();
    }
}
