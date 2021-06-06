using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class SelectionSphere : MonoBehaviour
{
    public List<Transform> _selection = new List<Transform>();
    bool isUnitSelection = false;
    bool IsUnitInteractableSelection = false;
    UnitType _selectionType;

    private void Awake()
    {
        if (_selection.Count == 0)
        {
            _selectionType = GroupManager.Instance._selectedGroup._type;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Unit") || other.CompareTag("UnitInteractable"))
        {
            if(_selection.Count == 0)
            {
                if (other.CompareTag("Unit"))
                    isUnitSelection = true;
                else if (other.CompareTag("UnitInteractable"))
                    IsUnitInteractableSelection = true;
            }

            //UNIT SELECTION
            if(isUnitSelection && other.CompareTag("Unit"))
            {
                if (!_selection.Contains(other.transform))
                {
                    if(other.GetComponent<Unit>()._unitType == UnitType.Enemy || other.GetComponent<Unit>()._unitType == UnitType.Animal)
                    {
                        _selection.Add(other.transform);
                        other.GetComponent<Unit>().SetOutlineState(OutlineState.AboutToBeSelected);
                    }
                }
            }
            //INTERACTABLE SELECTON
            else if(IsUnitInteractableSelection && other.CompareTag("UnitInteractable"))
            {
                if (!_selection.Contains(other.transform))
                {
                    UnitInteractable inter = other.GetComponent<UnitInteractable>();

                    if(inter._whoCanInteract == _selectionType)
                    {
                        _selection.Add(other.transform);
                        inter.SetOutlineState(OutlineState.AboutToBeSelected);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit") || other.CompareTag("UnitInteractable"))
        {
            if (_selection.Contains(other.transform))
            {
                _selection.Remove(other.transform);
                if(isUnitSelection)
                    other.GetComponent<Unit>().SetOutlineState(OutlineState.UnSelected);
                else if(IsUnitInteractableSelection)
                    other.GetComponent<UnitInteractable>().SetOutlineState(OutlineState.UnSelected);
            }
        }
    }

    public void SetSelectedVisual()
    {
        if (IsUnitInteractableSelection)
        {
            foreach (Transform item in _selection)
            {
                item.GetComponent<UnitInteractable>().SetOutlineState(OutlineState.Selected);
            }
        }
        else if (isUnitSelection)
        {
            foreach (Transform item in _selection)
            {
                item.GetComponent<Unit>().SetOutlineState(OutlineState.Selected);
            }
        }
    }
}
