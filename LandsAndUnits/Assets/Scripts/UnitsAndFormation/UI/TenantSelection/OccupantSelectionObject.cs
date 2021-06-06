using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnitsAndFormation;
using UnityEngine.UI;

public class OccupantSelectionObject : MonoBehaviour
{
    public OccupancyType _type;
    public Image _unitTypeIcon;
    public TextMeshProUGUI _unitName;
    [HideInInspector] public Unit _unit;
    [SerializeField] private Button _button;


    public delegate void AssignNewOccupant(Unit buildingForTenant, OccupancyType occupancyType);
    public static event AssignNewOccupant OnAssignNewOccupant;

    private void Start()
    {
        _button.onClick.AddListener(FireDelegate);
    }

    private void FireDelegate()
    {
        OnAssignNewOccupant?.Invoke(_unit, _type);
    }
}
