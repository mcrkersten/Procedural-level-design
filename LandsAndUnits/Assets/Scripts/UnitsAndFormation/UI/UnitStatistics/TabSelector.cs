using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSelector : MonoBehaviour
{

    [SerializeField] private GameObject _tab1;
    [SerializeField] private GameObject _tab2;
    [SerializeField] private GameObject _tab3;

    public void Awake()
    {
        TabSelectorButton.OnTabClick += TabClick;
    }
    private void TabClick(int tab)
    {
        switch (tab)
        {
            case 0:
                _tab1.SetActive(true);
                _tab2.SetActive(false);
                _tab3.SetActive(false);
                break;
            case 1:
                _tab1.SetActive(false);
                _tab2.SetActive(true);
                _tab3.SetActive(false);
                break;
            case 2:
                _tab1.SetActive(false);
                _tab2.SetActive(false);
                _tab3.SetActive(true);
                break;
        }
    }

    private void OnDestroy()
    {
        TabSelectorButton.OnTabClick -= TabClick;
    }
}
