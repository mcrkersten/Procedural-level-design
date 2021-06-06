using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using UnityEngine.UI;

public class OccupantSelectorCreator : MonoBehaviour
{
    [SerializeField] private GameObject _neutrtalOccupantSelectionObject;
    [SerializeField] private GameObject _contentField;
    private UnitAdministrator _unitAdministration;

    private void Update()
    {
        for (int i = 0; i < _contentField.transform.childCount; i++)
        {
            RectTransform rect = _contentField.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 85 * i + 125);
        }
    }

    public void Create(OccupancyType type)
    {
        _unitAdministration = UnitAdministrator.Instance;
        foreach (Unit unit in _unitAdministration._units)
        {
            if ( unit._unitType != UnitType.Animal && unit._unitType != UnitType.Enemy)
            {
                switch (type)
                {
                    case OccupancyType.Tenant:
                        if(unit._unitBrain._memory._home == null)
                            CreateObject(unit, OccupancyType.Tenant);
                        break;
                    case OccupancyType.Visitor:
                        break;
                    case OccupancyType.Worker:
                        if (unit._unitType == UnitType.Worker && unit._unitBrain._memory._workplaceBuilding == null)
                            CreateObject(unit, OccupancyType.Worker);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void CreateObject(Unit unit, OccupancyType type)
    {
        GameObject possibleOccupant = Instantiate(_neutrtalOccupantSelectionObject, _contentField.transform);
        OccupantSelectionObject occupantSelectionObject = possibleOccupant.GetComponent<OccupantSelectionObject>();
        occupantSelectionObject._unit = unit;
        occupantSelectionObject._unitName.text = unit._unitBrain._name;
        occupantSelectionObject._unitTypeIcon.sprite = UnitTypeDatabase.GetGetUnitTypeIcon(unit._unitType);
        occupantSelectionObject._type = type;
    }

    public void DestroyContent()
    {
        int lenght = _contentField.transform.childCount -1;
        for (int i = lenght; i >= 0; i--)
        {
            Destroy(_contentField.transform.GetChild(i).gameObject);
        }
    }
}
