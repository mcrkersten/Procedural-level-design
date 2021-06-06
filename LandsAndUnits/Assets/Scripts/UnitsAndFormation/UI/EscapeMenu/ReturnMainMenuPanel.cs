using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReturnMainMenuPanel : MonoBehaviour
{
    [SerializeField] Button _returnToMenuButton;

    private void Awake()
    {
        _returnToMenuButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GameManager._instance.LoadGame(ScenesIndexes.MAIN_MENU);
    }
}
