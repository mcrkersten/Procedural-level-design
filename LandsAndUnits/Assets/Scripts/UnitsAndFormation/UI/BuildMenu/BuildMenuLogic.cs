using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject _leftButtonPrefab;
    [SerializeField] private GameObject _rightButtonPrefab;

    [SerializeField] private List<MenuTab> _menuTabs = new List<MenuTab>();
    [SerializeField] private List<BuildingCategory> _buildingCatagories = new List<BuildingCategory>();

    [SerializeField] private GameObject _buttonHolder;
    private List<BuildMenuButton> _buttons = new List<BuildMenuButton>();
    private int _currentTab;

    private void Start()
    {
        MenuDeactivator.OnCloseMenu += Deactivate;
        BuildMenuButton.OnActivateBuilding += Deactivate;
        MenuTrigger.OnOpenBuildMenu += Activate;
        CancelButton.OnCancelBuildButtonClick += Activate;
        OnMenuTabClick(0);
        Deactivate();
    }

    public void OnMenuTabClick(int number)
    {
        for (int i = 0; i < _menuTabs.Count; i++)
        {
            if (i == number)
            {
                _currentTab = i;
                UpdateButton(_buildingCatagories[i]);
                _menuTabs[i].ActivateTab();
            }
            else
            {
                _menuTabs[i].DeactivateTab();
            }
        }
    }

    private void UpdateButton(BuildingCategory n)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            Destroy(_buttons[i].gameObject);
        }
        _buttons = new List<BuildMenuButton>();

        int xx = 0;
        foreach (InteractableInformation item in n._items)
        {
            BuildMenuButton b;
            GameObject x;
            if (n._items.Count > 5)
            {
                if(xx < 2)
                {
                    x = Instantiate(_rightButtonPrefab, _buttonHolder.transform);
                    b = x.GetComponent<BuildMenuButton>();
                    b.MenuItem = item;
                    _buttons.Add(b);
                    xx++;
                    continue;
                }
            }
            else if (n._items.Count > 4)
            {
                if (xx < 1)
                {
                    x = Instantiate(_rightButtonPrefab, _buttonHolder.transform);
                    b = x.GetComponent<BuildMenuButton>();
                    b.MenuItem = item;
                    _buttons.Add(b);
                    xx++;
                    continue;
                }
            }

            x = Instantiate(_leftButtonPrefab, _buttonHolder.transform);
            b = x.GetComponent<BuildMenuButton>();
            b.MenuItem = item;
            _buttons.Add(b);
            xx++;
        }
    }

    private void Deactivate()
    {
        this.transform.gameObject.SetActive(false);
    }

    private void Activate()
    {
        this.transform.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        MenuDeactivator.OnCloseMenu -= Deactivate;
        BuildMenuButton.OnActivateBuilding -= Deactivate;
        CancelButton.OnCancelBuildButtonClick -= Activate;
        MenuTrigger.OnOpenBuildMenu -= Activate;
    }
}
