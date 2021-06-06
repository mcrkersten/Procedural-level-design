using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "TipAndGuideData", menuName = "ScriptableObjects/TipAndGuideData", order = 6), System.Serializable]
public class TipAndGuideData : ScriptableObject
{
    public Guide _guideType;
    public List<GuidePoint> _data = new List<GuidePoint>();

    public GuidePoint GetData(int index)
    {
        return _data.SingleOrDefault(inter => (inter)._index == index);
    }
}

[System.Serializable]
public class GuidePoint
{
    public Guide _guide;
    public int _index;
    public string _title;
    public string _text;
    public string _objective;
    public MenuHighlight _toHighlight;
}

public enum MenuHighlight
{
    NONE = 0,
    DEMOLISH_BUTTON,
    BUILD_BUTTON,
    CITIES_TAB,
    STORAGE_TAB,
    SHIPYARD_TAB,
    OTHER_TAB
}
