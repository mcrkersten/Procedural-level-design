using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class WorkObjectCreator : MonoBehaviour
{
    public List<ResourceInformation> _resourceTypes = new List<ResourceInformation>();

    [Header("Stock")]
    [SerializeField] private GameObject _stockDivision;
    [SerializeField] private GameObject _stockObject;
    private List<StockObject> _instantiated_StockObject = new List<StockObject>();

    [Header("Progress")]
    [SerializeField] private GameObject _progressDivision;
    [SerializeField] private GameObject _sliderObject;
    [SerializeField] private GameObject _costObject;
    private List<StockObject> _instantiated_CostObjects = new List<StockObject>();
    private List<Slider> _instantiated_SliderObjects = new List<Slider>();

    private List<StockObject> _instantiated_CreatedStockObject = new List<StockObject>();

    // Update is called once per frame
    public void CreateStockDivision(Building interactable)
    {
        if (interactable._stockpiles.Count > 0)
        {
            //Create parent to hold created objects
            GameObject divisionGameObject = Instantiate(_stockDivision, transform);

            //Creates stock object
            foreach (StockpileInformation stockpile in interactable._stockpiles)
            {
                GameObject instantiatedObject = Instantiate(_stockObject, divisionGameObject.transform);
                StockObject stock = instantiatedObject.GetComponent<StockObject>();
                stock._icon.sprite = _resourceTypes.First(resource => resource._resourceType == stockpile._resourceType)._icon ?? null;
                stock._amount.text = stockpile._currentStockAmount.ToString();
                stock._toolTip.defaultContent = stockpile._resourceType.ToString();
                _instantiated_StockObject.Add(stock);
            }
        }
    }

    public void UpdateStockDivision(Building interactable)
    {
        int i = 0;
        foreach (StockObject stockpile in _instantiated_StockObject)
        {
            stockpile._amount.text = interactable._stockpiles[i]._currentStockAmount.ToString();
            i++;
        }
    }

    public void CreateProgressDivision(Workplace interactable)
    {
        //Creates parent to hold created objects
        GameObject divisionGameObject = Instantiate(_progressDivision, transform);

        //Creates slider object;
        GameObject slider = Instantiate(_sliderObject, divisionGameObject.transform);
        _instantiated_SliderObjects.Add(slider.GetComponent<Slider>());

        //Creates cost Object
        foreach (ResourceInformation toProduce in interactable._resourcesToProduce)
        {
            foreach (CostInformation item in toProduce._resourcesNeededToProduce)
            {
                GameObject instantiatedObject = Instantiate(_costObject, divisionGameObject.transform);
                StockObject cost = instantiatedObject.GetComponent<StockObject>();
                cost._icon.sprite = _resourceTypes.First(resource => resource._resourceType == item._resourceInformation._resourceType)._icon ?? null;
                cost._amount.text = item._amount.ToString();
                cost._toolTip.defaultContent = item._resourceInformation.ToString();
                _instantiated_CostObjects.Add(cost);
            }
        }
    }

    public void UpdateProgressDivision(Workplace interactable)
    {
        int i = 0;
        if (interactable._workOrders.Count == 0)
            _instantiated_SliderObjects[i].value = Mathf.Lerp(_instantiated_SliderObjects[i].value, 1F, Time.deltaTime * 3);

        foreach (WorkOrder workOrder in interactable._workOrders)
        {
            _instantiated_SliderObjects[i].value = Mathf.Lerp(_instantiated_SliderObjects[i].value, workOrder._progress, Time.deltaTime * 3);
            i++;
        }
    }

    public void CreateProducedStockDivision(Workplace interactable)
    {
        if (interactable._producedStockStockpiles.Count > 0)
        {
            //Create parent to hold created objects
            GameObject divisionGameObject = Instantiate(_stockDivision, transform);

            //Creates stock object
            foreach (StockpileInformation stockpile in interactable._producedStockStockpiles)
            {
                GameObject instantiatedObject = Instantiate(_stockObject, divisionGameObject.transform);
                StockObject stock = instantiatedObject.GetComponent<StockObject>();
                stock._icon.sprite = _resourceTypes.First(resource => resource._resourceType == stockpile._resourceType)._icon ?? null;
                stock._amount.text = stockpile._currentStockAmount.ToString();
                stock._toolTip.defaultContent = stockpile._resourceType.ToString();
                _instantiated_CreatedStockObject.Add(stock);
            }
        }
    }

    public void UpdateCreatedStockDivision(Workplace interactable)
    {
        int i = 0;
        foreach (StockObject stockpile in _instantiated_CreatedStockObject)
        {
            stockpile._amount.text = interactable._producedStockStockpiles[i]._currentStockAmount.ToString();
            i++;
        }
    }
}
