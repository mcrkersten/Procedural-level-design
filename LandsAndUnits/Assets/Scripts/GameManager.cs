using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitsAndFormation;
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public DialogScreen _dialogScreen;
    public ScenesIndexes _currentScene { private set; get; }

    public GameState _gameState { private set; get; }
    private bool _allowGameStateChange = true;
    private bool _isNewGame;
    public bool _tutorialEnabled;
    #region Loading screen variables
    public CanvasGroup _loadingScreen;
    private float totalSceneProgress;
    private float totalSpawnProgress;
    private int currentSteps = 0;
    private int totalSteps = 4;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public ProgressBar _progressBar;

    public delegate void LoadSceneComplete();
    public static event LoadSceneComplete  OnScenesLoaded;
    #endregion


    public SaveFileInformation _saveFileData { 
        set {
            SaveFileData = new SaveFileData { saveFilePath = value._saveFilePath,
            saveTime = value._saveDate,
            seed = value._seed,
            id = value._id};
        } 
    }

    public SaveFileData SaveFileData { private set; get; }

    public bool _isSaveFile { get; private set; }

    public void Awake()
    {
        _instance = this;
        LoadMainMenu();

        SaveFileDisplayer.OnLoadGame += OnLoadGame;
        SaveFileDisplayer.OnNewGame += OnNewGame;

        MapGenerator.OnMapGenerated += OnMapGenerated;
        FlowField.OnGenesisFieldCreated += OnGenesisFieldCreated;

        SavingLoading.OnSavegameLoaded += OnStepCompleted;
        FoilageGenerator.OnFoilageGenerated += OnStepCompleted;

        EscapeMenuPanel.OnPauseGame += PauseGame;
    }

    #region GameState
    public void SetGameState(GameState newState)
    {
        if(_allowGameStateChange)
            _gameState = newState;
    }

    private void PauseGame()
    {
        switch (_gameState)
        {
            case GameState.GAME:
                SetGameState(GameState.PAUSED);
                break;
            case GameState.PAUSED:
                SetGameState(GameState.GAME);
                break;
            case GameState.CUTSCENE:
                break;
            default:
                break;
        }
    }

    #endregion

    #region GameLoading
    private void OnMapGenerated()
    {
        GridController.Instance.CreateGenesisField();
        currentSteps++;
    }

    private void OnGenesisFieldCreated()
    {
        if (!_isSaveFile)
            FoilageGenerator.Instance.InstantiateAllObjects();
        else
            SavingLoading._instance.Load();
        currentSteps++;
    }

    private void OnStepCompleted()
    {
        currentSteps++;
    }

    private void LoadMainMenu()
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.MAIN_MENU, LoadSceneMode.Additive));
        _currentScene = ScenesIndexes.MAIN_MENU;
        StartCoroutine(GetSceneLoadProgress(true));
    }

    private void LoadScenes(ScenesIndexes sceneIndex)
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)_currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive));

        //If not loading to mainmenu
        if(sceneIndex != ScenesIndexes.MAIN_MENU)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.ESCAPE_MENU, LoadSceneMode.Additive));
        }
        else
        {
            _isSaveFile = false;
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)ScenesIndexes.ESCAPE_MENU));
        }

        _currentScene = sceneIndex;

        StartCoroutine(GetSceneLoadProgress());
        StartCoroutine(GetTotalProgress());
    }

    public void LoadGame(ScenesIndexes sceneIndex)
    {
        StartCoroutine(ActivateLoadingScreen(sceneIndex));
        SetGameState(GameState.GAME);
    }

    public IEnumerator ActivateLoadingScreen(ScenesIndexes sceneIndex)
    {
        while (_loadingScreen.alpha != 1f)
        {
            _loadingScreen.alpha += .05f;
            yield return new WaitForFixedUpdate();
        }

        LoadScenes(sceneIndex);
    }

    public IEnumerator GetSceneLoadProgress(bool isMainMenuLoad = default)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }
                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                yield return null;
            }
        }
        currentSteps++;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)_currentScene));
        OnScenesLoaded?.Invoke();
    }

    public IEnumerator GetTotalProgress()
    {
        while(currentSteps < totalSteps)
        {
            float progress = (currentSteps / totalSteps) * 100f;
            _progressBar.current = Mathf.RoundToInt(Mathf.Round((totalSceneProgress + progress) / 2));
            yield return null;
        }
        if (_isNewGame)
            CutsceneManager._instance.StartCutscene(0);
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(DeactivateLoadingScreen(5f));
    }

    public IEnumerator DeactivateLoadingScreen(float time)
    {
        float passedTime = 0f;
        while (passedTime < time)
        {
            passedTime += Time.deltaTime;


            _progressBar.current = _progressBar.maximum;
            _loadingScreen.alpha = 1f - passedTime / time;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnNewGame(SaveFileInformation information)
    {
        currentSteps = 0;
        _isNewGame = true;
        SavingLoading._saveFile = information;
        LoadGame(ScenesIndexes.GAME);
    }

    private void OnLoadGame(SaveFileInformation information)
    {
        currentSteps = 0;
        _isSaveFile = true;
        SavingLoading._saveFile = information;
        LoadGame(ScenesIndexes.GAME);
    }
    #endregion
}

public enum ScenesIndexes
{
    MANAGER = 0,
    MAIN_MENU = 1,
    ESCAPE_MENU = 2,
    GAME = 3,
}

public enum GameState
{
    GAME = 0,
    PAUSED,
    CUTSCENE,
}
