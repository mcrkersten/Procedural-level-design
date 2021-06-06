using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuPanel : MonoBehaviour
{

    public delegate void PauseGame();
    public static event PauseGame OnPauseGame;

    [Header("buttons")]
    [SerializeField] private Button _saveGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _returnMainMenuButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private List<Button> _cancelButtons = new List<Button>();
    [SerializeField] private Button _closeEscapeMenuButton;

    [Header("panels")]
    [SerializeField] private GameObject _backGround;
    [SerializeField] private GameObject _escapeMenu;
    [SerializeField] private GameObject _saveGamePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _returnToMainMenuPanel;
    [SerializeField] private GameObject _quitToDesktopPanel;

    private MenuType _currentActiveMenu;

    private void Start()
    {
        _saveGameButton.onClick.AddListener(delegate { OnButtonPress(MenuType.SaveGame); });
        _settingsButton.onClick.AddListener(delegate { OnButtonPress(MenuType.Settings); });
        _returnMainMenuButton.onClick.AddListener(delegate { OnButtonPress(MenuType.ReturnMainMenu); });
        _quitButton.onClick.AddListener(delegate { OnButtonPress(MenuType.QuitToDesktop); });
        _closeEscapeMenuButton.onClick.AddListener(OnEscapeMenuButton);

        foreach (Button cancelButton in _cancelButtons)
        {
            cancelButton.onClick.AddListener(OnCancelButtonPress);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_currentActiveMenu == MenuType.EscapeMenu)
            {
                if (!_escapeMenu.activeSelf)
                {
                    _escapeMenu.SetActive(true);
                    _backGround.SetActive(true);
                    OnPauseGame?.Invoke();
                }
                else
                {
                    _escapeMenu.SetActive(false);
                    _backGround.SetActive(false);
                    OnPauseGame?.Invoke();
                }
            }
        }
    }

    private void OnEscapeMenuButton()
    {
        _escapeMenu.SetActive(false);
        _backGround.SetActive(false);
        OnPauseGame?.Invoke();
    }

    private void OnButtonPress(MenuType type)
    {
        _currentActiveMenu = type;
        switch (type)
        {
            case MenuType.SaveGame:
                _escapeMenu.SetActive(false);
                _saveGamePanel.SetActive(true);
                break;
            case MenuType.Settings:
                _escapeMenu.SetActive(false);
                _settingsPanel.SetActive(true);
                break;
            case MenuType.ReturnMainMenu:
                _escapeMenu.SetActive(false);
                _returnToMainMenuPanel.SetActive(true);
                break;
            case MenuType.QuitToDesktop:
                _escapeMenu.SetActive(false);
                _quitToDesktopPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnCancelButtonPress()
    {
        switch (_currentActiveMenu)
        {
            case MenuType.SaveGame:
                _saveGamePanel.SetActive(false);
                break;
            case MenuType.Settings:
                _settingsPanel.SetActive(false);
                break;
            case MenuType.ReturnMainMenu:
                _returnToMainMenuPanel.SetActive(false);
                break;
            case MenuType.QuitToDesktop:
                _quitToDesktopPanel.SetActive(false);
                break;
        }
        _escapeMenu.SetActive(true);
        _currentActiveMenu = MenuType.EscapeMenu;
    }

    private enum MenuType
    {
        EscapeMenu = 0,
        SaveGame,
        Settings,
        ReturnMainMenu,
        QuitToDesktop
    }
}
