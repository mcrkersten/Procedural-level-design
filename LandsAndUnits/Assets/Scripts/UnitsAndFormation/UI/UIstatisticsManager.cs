using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnitsAndFormation;

public class UIstatisticsManager : MonoBehaviour
{
    private ResourceManager _resourceManager;

    [SerializeField]
    private TextMeshProUGUI _wood;
    [SerializeField]
    private TextMeshProUGUI _stone;
    [SerializeField]
    private TextMeshProUGUI _food;

    private void Awake()
    {
        ResourceManager.OnResourceUserInterfaceUpdate += OnResourceUpdate;
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
    }

    private void OnResourceUpdate(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood:
                _wood.text = AddZeros(_resourceManager._woodAmount.ToString());
                break;
            case ResourceType.Stone:
                _stone.text = AddZeros(_resourceManager._stoneAmount.ToString());
                break;
            case ResourceType.Wheat:
                _food.text = AddZeros(_resourceManager._foodAmount.ToString());
                break;
            default:
                break;
        }
    }

    private string AddZeros(string number)
    {
        string add = "";
        if (number.Length == 1)
            add += "000";
        else if (number.Length == 2)
            add += "00";
        else if (number.Length == 3)
            add += "0";

        add += number;
        return add;
    }

    private void OnDestroy()
    {
        ResourceManager.OnResourceUserInterfaceUpdate -= OnResourceUpdate;
    }
}
