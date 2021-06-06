using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class MouseIconHandler : MonoBehaviour
{
    [SerializeField] private Texture2D _construct;
    [SerializeField] private Texture2D _harvest;
    [SerializeField] private Texture2D _placement;
    [SerializeField] private Texture2D _demolition;
    public CursorMode cursorMode = CursorMode.Auto;
    void Start()
    {
        UnitInteractable.OnUnitInteractableCursorEvent += UpdateCursorType;
        BuildingPlacer.OnBuildingPlacement += UpdateCursorType;
    }

    private void UpdateCursorType(CursorType x)
    {
        UnitType type = UnitType.Neutral;
        if (GroupManager.Instance._selectedUnits.Count != 0)
            type = GroupManager.Instance._selectedUnits[0]._unitType;

        switch (x)
        {
            case CursorType.Construct:
                if (type == UnitType.Builder)
                    if (GroupManager.Instance._selectedUnits.Count != 0)
                        Cursor.SetCursor(_construct, Vector2.zero, cursorMode);
                    else
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.Harvest:
                if (type == UnitType.Harvester)
                    if (GroupManager.Instance._selectedUnits.Count != 0)
                        Cursor.SetCursor(_harvest, Vector2.zero, cursorMode);
                    else
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.SelectUnit:
                break;
            case CursorType.MoveUnit:
                break;
            case CursorType.Placement:
                Cursor.SetCursor(_placement, Vector2.zero, cursorMode);
                break;
            case CursorType.Demolition:
                Cursor.SetCursor(_demolition, Vector2.zero, cursorMode);
                break;
            case CursorType.None:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
    }

    private void OnDestroy()
    {
        UnitInteractable.OnUnitInteractableCursorEvent -= UpdateCursorType;
    }
}
