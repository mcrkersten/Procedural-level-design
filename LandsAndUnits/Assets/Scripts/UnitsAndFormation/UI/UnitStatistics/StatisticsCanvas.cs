using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnitsAndFormation;
public class StatisticsCanvas : MonoBehaviour
{
    public Unit _selectedUnit { private set; get; }
    [SerializeField] private GameObject _rect;

    private void Start()
    {
        //Listners
        Unit.OnUnitClick += OnUnitSelection;
        UnitSelection.OnUnSelectUnitInformation += OnUnitDeselection;
    }

    private void Update()
    {

    }

    private void OnUnitSelection(Unit unit)
    {
        _selectedUnit = unit;
        _rect.SetActive(true);
    }

    private void OnUnitDeselection()
    {
        _rect.SetActive(false);
        _selectedUnit = null;
    }

    private void OnDestroy()
    {
        Unit.OnUnitClick -= OnUnitSelection;
        UnitSelection.OnUnSelectUnitInformation -= OnUnitDeselection;
    }
}
