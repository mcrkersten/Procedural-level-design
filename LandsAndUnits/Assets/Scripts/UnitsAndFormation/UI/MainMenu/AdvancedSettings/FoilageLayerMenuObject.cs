using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnitsAndFormation;


public class FoilageLayerMenuObject : MonoBehaviour
{
    public TextMeshProUGUI _typeTitle;
    public TMP_InputField _minInput;
    public TMP_InputField _maxInput;
    public ResourceSpawnOptions _layer;

    private void Start()
    {
        _maxInput.onEndEdit.AddListener(OnMaxInputChange);
        _minInput.onEndEdit.AddListener(OnMinInputChange);
    }

    private void OnMaxInputChange(string input)
    {
        _layer._maximum = SanitizeInput(_maxInput, input, "0123456789");
    }

    private void OnMinInputChange(string input)
    {
        _layer._minimum = SanitizeInput(_minInput, input, "0123456789");
    }

    public int SanitizeInput(TMP_InputField input, string inputString, string validCharacters)
    {
        input.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return ValidateChar(validCharacters, addedChar);
        };

        if (string.IsNullOrEmpty(inputString))
        {
            input.text = "0";
        }
        int myConvertedInt;
        try
        {
            myConvertedInt = int.Parse(inputString, System.Globalization.NumberStyles.Integer);
        }
        catch
        {
            myConvertedInt = 0;
        }
        return myConvertedInt;
    }

    private char ValidateChar(string validCharacters, char addedChar)
    {
        if (validCharacters.IndexOf(addedChar) != -1)
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }
}
