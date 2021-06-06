using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnitsAndFormation;
public class UnitStatisticsCanvas : MonoBehaviour
{
    public Unit _selectedUnit { private set; get; }
    [SerializeField] private StatisticsCanvas _statisticsCanvas;
    [SerializeField] private Image _typeIcon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _occupation;

    [SerializeField] private Slider _energySlider;
    [SerializeField] private Image _energySliderFill;
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Image _hungerSliderFill;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Image _healthSliderFill;

    [SerializeField] private TextMeshProUGUI _energyPercentage;
    [SerializeField] private TextMeshProUGUI _hungerPercentage;
    [SerializeField] private TextMeshProUGUI _healthPercentage;

    [SerializeField] private Button _selectHouse;
    [SerializeField] private GameObject _homelessIcon;

    [SerializeField] private Button _freeWillButton;
    [SerializeField] private Image _boolean;
    [SerializeField] private Sprite _trueSprite;
    [SerializeField] private Sprite _falseSprite;

    private void Start()
    {
        //buttons
        _selectHouse.onClick.AddListener(OnHouseSelection);
        _freeWillButton.onClick.AddListener(ChangeFreeWill);
    }

    private void Update()
    {
        if (_selectedUnit != null && _selectedUnit._talkCloud != null)
            SetSliders();
    }

    public void OnEnable()
    {
        _selectedUnit = _statisticsCanvas._selectedUnit;
        _typeIcon.sprite = UnitTypeDatabase.GetGetUnitTypeIcon(_selectedUnit._unitType);
        _name.text = _selectedUnit._unitBrain._name;
        _occupation.text = _selectedUnit._unitType.ToString();

        SetHomeIcon();
        SetFreeWillButton();
    }

    private void SetHomeIcon()
    {
        if (_selectedUnit._unitBrain._memory._home != null)
        {
            _selectHouse.gameObject.SetActive(true);
            _homelessIcon.SetActive(false);
        }
        else
        {
            _selectHouse.gameObject.SetActive(false);
            _homelessIcon.SetActive(true);
        }
    }

    private void ChangeFreeWill()
    {
        _selectedUnit._unitBrain._memory._hasFreeWill = !_selectedUnit._unitBrain._memory._hasFreeWill;
        SetFreeWillButton();
    }

    private void SetFreeWillButton()
    {
        if (_selectedUnit._unitBrain._memory._hasFreeWill)
            _boolean.sprite = _trueSprite;
        else
            _boolean.sprite = _falseSprite;
    }

    private void SetSliders()
    {
        float p1 = (float)_selectedUnit._unitBrain._health._energy / UnitTypeDatabase.GetWellbeing(_selectedUnit._unitType)._maxEnergy;
        _energySliderFill.color = WarningDatabase.GetThresholdMinToMax(UnitWarningType.Energy, Mathf.Round(p1 * 100f))._color;
        _energyPercentage.text = Mathf.Round(p1 * 100f).ToString() + "%";
        _energySlider.value = p1;


        float p2 = (float)_selectedUnit._unitBrain._health._hunger / UnitTypeDatabase.GetWellbeing(_selectedUnit._unitType)._maxHunger;
        _hungerSliderFill.color = WarningDatabase.GetThresholdMaxToMin(UnitWarningType.Food, Mathf.Round(p2 * 100f))._color;
        _hungerPercentage.text = Mathf.Round(p2 * 100f).ToString() + "%";
        _hungerSlider.value = p2;

        float p3 = (float)_selectedUnit._unitBrain._health._health / UnitTypeDatabase.GetWellbeing(_selectedUnit._unitType)._maxHealth;
        _healthSliderFill.color = WarningDatabase.GetThresholdMinToMax(UnitWarningType.Health, Mathf.Round(p3 * 100f))._color;
        _healthPercentage.text = Mathf.Round(p3 * 100f).ToString() + "%";
        _healthSlider.value = p3;
    }

    private void OnHouseSelection()
    {
        _selectedUnit._unitBrain._memory._home.OnMouseUp();
        CameraController._instance.CenterCameraOnObject(_selectedUnit._unitBrain._memory._home.gameObject);
    }

    private void OnDestroy()
    {
        _selectHouse.onClick.RemoveAllListeners();
        _freeWillButton.onClick.RemoveAllListeners();
    }
}
