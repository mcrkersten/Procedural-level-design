using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingCategory", menuName = "ScriptableObjects/BuildingCategory", order = 3)]
public class BuildingCategory : ScriptableObject
{
    public Color _color;
    public List<InteractableInformation> _items = new List<InteractableInformation>();
}
