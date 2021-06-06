using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

public class ShipWreck : Building
{
    protected override void Awake()
    {
        _interactableID = GetComponent<Buildingcomponents>()._interactableInformation._interactableID;
        _isShipWreck = true;
        if (_targetTransform == null)
            _targetTransform = this.gameObject.transform;

        base.Awake();

        _type = InteractableType.Other;
        base.OnPlacement();
    }
}
