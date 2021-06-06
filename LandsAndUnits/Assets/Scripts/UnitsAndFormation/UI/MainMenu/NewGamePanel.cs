using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGamePanel : MonoBehaviour
{

    public delegate void ActivateLoadingPanelAsNewGame();
    public static event ActivateLoadingPanelAsNewGame OnActivateLoadingPanelAsNewGame;

    private GameManager _gameManager;
    [SerializeField] NoiseData _noiseData;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _randomizeButton;
    [SerializeField] Button _startNewGameButton;

    private void Start()
    {
        _gameManager = GameManager._instance;
        _startNewGameButton.onClick.AddListener(OpenSaveFilesPanel);
        _randomizeButton.onClick.AddListener(GenerateRandomSeed);
        _inputField.text = _noiseData.seed.ToString();
    }

    public void OnInputfield(string input)
    {
        SetInputField(input,"0123456789");
    }

    private void GenerateRandomSeed()
    {
        int randomSeed = Random.Range(0, int.MaxValue);
        _inputField.text = randomSeed.ToString();
        _noiseData.seed = randomSeed;
        OnDoneEdit();
    }

    private void OpenSaveFilesPanel()
    {
        OnActivateLoadingPanelAsNewGame?.Invoke();
    }

    public void SetInputField(string inputString, string validCharacters)
    {
        _inputField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return ValidateChar(validCharacters, addedChar);
        };

        if (string.IsNullOrEmpty(inputString))
        {
            _inputField.text = "0";
        }

        int myConvertedInt = int.Parse(inputString, System.Globalization.NumberStyles.Integer);
        _noiseData.seed = myConvertedInt;
    }

    public void OnDoneEdit()
    {
        _noiseData.NotifyOfUpdatedValues();
    }

    private char ValidateChar(string validCharacters, char addedChar)
    {
        if(validCharacters.IndexOf(addedChar) != -1)
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }
}
