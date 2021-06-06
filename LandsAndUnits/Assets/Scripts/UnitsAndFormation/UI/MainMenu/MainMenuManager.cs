using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public delegate void ActivateLoadingPanel();
    public static event ActivateLoadingPanel OnActivateLoadingPanel;

    [SerializeField] private MenuType _currentActiveMenu;
    [SerializeField] private List<Button> _returnToMainMenu = new List<Button>();

    [Header("Mainmenu buttons")]
    [SerializeField] private Button _islandCreationButton;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private Button _advancedIslandSettingsButton;
    [SerializeField] private Button _returnToStandardIslandSettings;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _islandCreation;
    [SerializeField] private GameObject _advancedIslandSettings;
    [SerializeField] private GameObject _loadGame;
    [SerializeField] private GameObject _settings;

    private void Awake()
    {
        NewGamePanel.OnActivateLoadingPanelAsNewGame += GoToSafeFileMenu;
    }

    private void Start()
    {
        foreach (Button button in _returnToMainMenu)
        {
            button.onClick.AddListener(ReturnToMainMenu);
        }

        _islandCreationButton.onClick.AddListener(delegate { SelectMenu(MenuType.IslandCreation); });
        _loadGameButton.onClick.AddListener(delegate { SelectMenu(MenuType.LoadGame); });
        _settingsButton.onClick.AddListener(delegate { SelectMenu(MenuType.Settings); });
        _quitButton.onClick.AddListener(delegate { SelectMenu(MenuType.Quit); });

        _advancedIslandSettingsButton.onClick.AddListener(delegate { SelectMenu(MenuType.AdvancedIslandSettings); });
        _returnToStandardIslandSettings.onClick.AddListener(delegate { SelectMenu(MenuType.IslandCreation); });
    }

    private void SelectMenu(MenuType menuToShow)
    {
        switch (_currentActiveMenu)
        {
            case MenuType.Main:
                _mainMenu.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.IslandCreation:
                _islandCreation.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.LoadGame:
                _loadGame.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.Settings:
                _settings.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.Quit:
                break;
            case MenuType.AdvancedIslandSettings:
                _advancedIslandSettings.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            default:
                break;
        }

        switch (menuToShow)
        {
            case MenuType.IslandCreation:
                _islandCreation.transform.DOMoveX(160f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.LoadGame:
                SetLoadingPanelAsLoading();
                _loadGame.transform.DOMoveX(160f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.Settings:
                _settings.transform.DOMoveX(160f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.Quit:
                break;
            case MenuType.AdvancedIslandSettings:
                _advancedIslandSettings.transform.DOMoveX(160f, .5f).SetEase(Ease.InOutQuint);
                break;
            default:
                break;
        }
        _currentActiveMenu = menuToShow;
    }

    public void ReturnToMainMenu()
    {
        _mainMenu.transform.DOMoveX(160f, .5f).SetEase(Ease.InOutQuint);
        switch (_currentActiveMenu)
        {
            case MenuType.Main:
                break;
            case MenuType.IslandCreation:
                _islandCreation.transform.DOMoveX(-600, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.LoadGame:
                _loadGame.transform.DOMoveX(-600, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.Settings:
                break;
            default:
                break;
        }
        _currentActiveMenu = MenuType.Main;
    }

    private void SetLoadingPanelAsLoading()
    {
        OnActivateLoadingPanel?.Invoke();
    }

    private void GoToSafeFileMenu()
    {
        _loadGame.transform.DOMoveX(160f, .5f).SetEase(Ease.InOutQuint);
        SetLoadingPanelAsLoading();
        switch (_currentActiveMenu)
        {
            case MenuType.Main:
                _mainMenu.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            case MenuType.IslandCreation:
                _islandCreation.transform.DOMoveX(-600f, .5f).SetEase(Ease.InOutQuint);
                break;
            default:
                break;
        }
        _currentActiveMenu = MenuType.LoadGame;
    }

    private enum MenuType
    {
        Main = 0,
        IslandCreation,
        LoadGame,
        Settings,
        Quit,
        AdvancedIslandSettings
    }
}
