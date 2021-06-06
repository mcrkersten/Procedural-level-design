using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGamePanel : MonoBehaviour
{
    public delegate void SaveGame();
    public static event SaveGame OnSaveGame;

    [SerializeField] private Button _saveGameButton;

    private void Start()
    {
        _saveGameButton.onClick.AddListener(OnSaveButton);
    }

    private void OnSaveButton()
    {
        OnSaveGame?.Invoke();
    }
}
