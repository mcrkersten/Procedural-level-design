using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingLoading : MonoBehaviour
{
    public static SavingLoading _instance;
    public static SaveFileInformation _saveFile;
    private int _loadCycles = 4;
    public bool _savefileLoaded;

    public delegate void SavegameLoaded();
    public static event SavegameLoaded OnSavegameLoaded;

    public delegate void PrepareToSave();
    public static event PrepareToSave OnPrepareToSave;

    private void Awake()
    {
        _instance = this;
        SaveFileDisplayer.OnLoadGame += OnLoadGame;
        SaveFileDisplayer.OnNewGame += OnLoadGame;

        SaveGamePanel.OnSaveGame += Save;
    }

    private void OnLoadGame(SaveFileInformation information)
    {
        _saveFile = information;
    }

    private void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }


    public void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    private void SaveFile(object state)
    {
        Debug.Log(_saveFile._saveFilePath);
        using(var stream = File.Open(Application.persistentDataPath + "/" + _saveFile._saveFilePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + _saveFile._saveFilePath))
        {
            Debug.Log("File not here");
            return new Dictionary<string, object>();
        }

        using(FileStream stream = File.Open(Application.persistentDataPath + "/" + _saveFile._saveFilePath, FileMode.Open))
        {
            Debug.Log("File here");
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        OnPrepareToSave?.Invoke();
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        for (int i = 0; i < _loadCycles; i++)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object value) && saveable._loadCycle == i)
                {
                    saveable.RestoreState(value);
                }
            }
        }
        OnSavegameLoaded?.Invoke();
    }
}
