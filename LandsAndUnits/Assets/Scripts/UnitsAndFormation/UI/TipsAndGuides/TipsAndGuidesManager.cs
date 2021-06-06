using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnitsAndFormation;
public class TipsAndGuidesManager : MonoBehaviour
{
    public static TipsAndGuidesManager _instance;
    [SerializeField] private List<TipAndGuideData> _guides = new List<TipAndGuideData>();
    [SerializeField] private TipAndGuidesScreen _tipAndGuidesScreen;
    [SerializeField] public ObjectiveBuilder _objectiveBuilder;

    public delegate void TriggerSelectionArrowTip(UnitType type);
    public static event TriggerSelectionArrowTip OnTriggerSelectionArrowTip;

    private void Awake()
    {
        _instance = this;
        _tipAndGuidesScreen = GetComponent<TipAndGuidesScreen>();
    }

    public void ActivateGuideSeries(Guide guide)
    {
        if (GameManager._instance._tutorialEnabled)
        {
            _tipAndGuidesScreen.ActivateGuidePoint(_guides.SingleOrDefault(inter => (inter)._guideType == guide).GetData(0));
            _objectiveBuilder.CreateObjectiveList(_guides.SingleOrDefault(inter => (inter)._guideType == guide));

            if (guide == Guide.CAMERA_CONTROLS)
                CameraController._instance._completedControlls = new Vector4(0, 1, 1, 1);
        }
    }

    public void AdvanceGuide(Guide guide, int point)
    {
        if (GameManager._instance._tutorialEnabled)
        {
            _tipAndGuidesScreen.ActivateGuidePoint(_guides.SingleOrDefault(inter => (inter)._guideType == guide).GetData(point));
            if (guide == Guide.DEMOLISH && point == 2)
                OnTriggerSelectionArrowTip?.Invoke(UnitType.Builder);
        }
    }

    public void CloseMenu()
    {
        _tipAndGuidesScreen.CloseMenu();
    }

    public void SelectedUnit()
    {
        OnTriggerSelectionArrowTip?.Invoke(UnitType.none);
    }
}

public enum Guide
{
    DEMOLISH,
    CAMERA_CONTROLS,
    BUILDING
}