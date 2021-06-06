using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using TMPro;

public class LoadGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _loadObjectPrefab;
    private List<GameObject> _instantiatedLoadObjects = new List<GameObject>();
    public int _possibleSaveGames;

    private void Awake()
    {
        NewGamePanel.OnActivateLoadingPanelAsNewGame += SetTitleAsNewGame;
        MainMenuManager.OnActivateLoadingPanel += SetTitleAsLoadGame;
    }
    private void Start()
    {
        for (int i = 0; i < _possibleSaveGames; i++)
        {
            GameObject saveFile = Instantiate(_loadObjectPrefab, this.transform);
            SaveFileDisplayer sd = saveFile.GetComponent<SaveFileDisplayer>();
            saveFile.GetComponent<SaveableEntity>().id = i.ToString();
            sd.AssignSaveFile("savefile" + i.ToString() + ".save");

            if (Directory.Exists(Application.persistentDataPath))
            {
                string saves = Application.persistentDataPath;
                DirectoryInfo d = new DirectoryInfo(saves);

                if(File.Exists(Application.persistentDataPath +"/"+ sd._information._saveFilePath))
                {
                    foreach (var file in d.GetFiles(sd._information._saveFilePath))
                    {
                        RestoreState(LoadFile(file));
                    }
                }
                else
                {
                    sd._isEmptySlot = true;
                }
            }
            else
            {
                File.Create(Application.persistentDataPath);
                return;
            }
        }
    }

    private void SetTitleAsNewGame()
    {
        _titleText.text = "Select file";
    }

    private void SetTitleAsLoadGame()
    {
        _titleText.text = "Load file";
    }

    private Dictionary<string, object> LoadFile(FileInfo file)
    {
        if (!file.Exists)
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = file.Open(FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
