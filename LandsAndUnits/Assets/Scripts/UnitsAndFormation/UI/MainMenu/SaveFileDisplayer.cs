using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveFileDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void LoadGame(SaveFileInformation information);
    public static event LoadGame OnLoadGame;

    public delegate void NewGame(SaveFileInformation information);
    public static event NewGame OnNewGame;

    [SerializeField] private TextMeshProUGUI _saveFileName;
    [SerializeField] private TextMeshProUGUI _saveDate;
    [SerializeField] private TextMeshProUGUI _emptyFileSlot;
    public bool _isEmptySlot;
    private bool _forNewGame;

    [SerializeField] private TextMeshProUGUI _hoverText;

    [SerializeField] private Button _saveFileButton;
    
    public SaveFileInformation _information { get; private set; }

    private void Awake()
    {
        _saveFileButton.onClick.AddListener(OnLoadSaveFileButton);

        NewGamePanel.OnActivateLoadingPanelAsNewGame += ActivateNewGameDisplayer;
        MainMenuManager.OnActivateLoadingPanel += ActivateLoadGameDisplayer;
    }


    private void ActivateNewGameDisplayer()
    {
        _forNewGame = true;
    }

    private void ActivateLoadGameDisplayer()
    {
        _forNewGame = false;
        SetInformation();
    }

    public void AssignSaveFile(string id)
    {
        _information = GetComponent<SaveFileInformation>();
        _information._isSavefile = true;
        _information._saveFilePath = id;
        _information._id = GetComponent<SaveableEntity>().id;
    }

    public void SetInformation()
    {
        if (!_isEmptySlot)
        {
            _saveFileName.gameObject.SetActive(true);
            _saveDate.gameObject.SetActive(true);
            _saveFileName.text = "Game " + _information._id;
            _saveDate.text = _information._saveDate;
            _emptyFileSlot.enabled = false;
        }
        else
        {
            _saveFileName.gameObject.SetActive(false);
            _saveDate.gameObject.SetActive(false);
            _emptyFileSlot.gameObject.SetActive(true);
        }
    }

    private void OnLoadSaveFileButton()
    {
        if (_information != null)
        {
            if (_isEmptySlot && _forNewGame)
            {
                _information._seed = MapGenerator.Instance._noiseData.seed;
                OnNewGame?.Invoke(_information);
                GameManager._instance._saveFileData = _information;
            }
            else
            {
                OnLoadGame?.Invoke(_information);
                GameManager._instance._saveFileData = _information;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isEmptySlot)
        {
            _saveFileName.gameObject.SetActive(true);
            _saveDate.gameObject.SetActive(true);
        }
        else
        {
            _emptyFileSlot.gameObject.SetActive(true);
        }

        _hoverText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_forNewGame)
        {
            _saveFileName.gameObject.SetActive(false);
            _saveDate.gameObject.SetActive(false);
            _emptyFileSlot.gameObject.SetActive(false);

            _hoverText.gameObject.SetActive(true);
            if (_isEmptySlot)
            {
                _hoverText.text = "Create new game";
            }
            else
            {
                _hoverText.text = "Overwrite savefile";
            }
        }
        else
        {
            if (!_isEmptySlot)
            {
                _saveFileName.gameObject.SetActive(false);
                _saveDate.gameObject.SetActive(false);
                _hoverText.gameObject.SetActive(true);
                _hoverText.text = "Load savefile";
            }
        }
    }

    private void OnDestroy()
    {
        NewGamePanel.OnActivateLoadingPanelAsNewGame -= ActivateNewGameDisplayer;
        MainMenuManager.OnActivateLoadingPanel -= ActivateLoadGameDisplayer;
    }
}
