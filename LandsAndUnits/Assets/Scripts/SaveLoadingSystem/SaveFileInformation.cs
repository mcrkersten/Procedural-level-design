using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveFileInformation : MonoBehaviour, ISaveable
{
    public string _saveFilePath;
    public string _saveDate;
    public int _seed;
    public string _id;
    public bool _set;
    [HideInInspector] public bool _isSavefile;

    private void Awake()
    {
        GameManager.OnScenesLoaded += SetSaveFileInformation;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.saveFilePath = _saveFilePath;
        data.saveTime = System.DateTime.Now.ToString();
        data.seed = _seed;
        data.id = _id;
        return data;
    }

    public void SetSaveFileInformation()
    {
        if (!_isSavefile)
        {
            _seed = GameManager._instance.SaveFileData.seed;
            _saveFilePath = GameManager._instance.SaveFileData.saveFilePath;
            _id = GameManager._instance.SaveFileData.id;
            GetComponent<SaveableEntity>().id = _id;
        }
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        _saveFilePath = saveData.saveFilePath;
        _saveDate = saveData.saveTime;
        _seed = saveData.seed;
        _id = saveData.id;
        _set = true;
    }

    [System.Serializable]
    private struct SaveData
    {
        public string saveFilePath;
        public string saveTime;
        public int seed;
        public string id;
    }

    private void OnDestroy()
    {
        GameManager.OnScenesLoaded -= SetSaveFileInformation;
    }
}

[System.Serializable]
public class SaveFileData {
    public string saveFilePath;
    public string saveTime;
    public int seed;
    public string id;
}
