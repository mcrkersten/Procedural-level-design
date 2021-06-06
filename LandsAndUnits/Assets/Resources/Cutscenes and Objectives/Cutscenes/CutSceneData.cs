using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData", order = 5), System.Serializable]
public class CutSceneData : ScriptableObject
{
    public int _sceneIndex;
    public List<DialogData> _dialogData = new List<DialogData>();
    public GameObject _cutceneTool;
}

[System.Serializable]
public class DialogData
{
    public string _speakerName;
    public List<string> _dialog = new List<string>();
    public GameObject _speaker;
    public DialogEmotion _emotion;
}

public enum DialogEmotion
{
    NEUTRAL = 0,
    AGITATED,
    HAPPY,
    SAD,
    ANGRY
}
