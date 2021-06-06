using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using UnityEngine.Playables;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour
{
    private int _currentDialog = 1;
    private int _currentScene = -1;
    private int _dialogIndex = 0;

    public static CutsceneManager _instance;
    private GameManager _gameManager;
    [SerializeField] private GameObject _wreck;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private List<GameObject> _unitsToSpawn = new List<GameObject>();
    private CameraController _cameraController;
    private List<GameObject> _spawnedUnits = new List<GameObject>();

    [SerializeField] private List<CutSceneData> _sceneData = new List<CutSceneData>();
    private Transform _scenePosition;

    private void Awake()
    {
        _instance = this;
        DialogScreen.OnDialogFinished += ContinueDialog;
    }

    private void Start()
    {
        _gameManager = GameManager._instance;
    }

    public void StartCutscene(int index)
    {
        StartCoroutine(PlayCutscene(index));
    }

    private IEnumerator PlayCutscene(int index)
    {
        if(_currentScene < index)
        {
            TipsAndGuidesManager._instance.CloseMenu();
            _currentScene = index;

            if (GameManager._instance._tutorialEnabled)
                _gameManager.SetGameState(GameState.CUTSCENE);

            if (index == 0)
            {
                _dialogIndex = 0;
                PrepareCutscene01();
                yield return new WaitForSeconds(4f);
                if(GameManager._instance._tutorialEnabled)
                    ActivateDialog(0, 0);
                yield break;
            }
            if (GameManager._instance._tutorialEnabled)
            {
                if (index == 1)
                {
                    _dialogIndex = 0;
                    PrepareCutscene02();
                    yield return new WaitForSeconds(1f);
                    ActivateDialog(1, 0);
                    yield break;
                }
            }
        }
    }

    private void ContinueDialog()
    {
        if(_currentScene < _sceneData.Count)
        {
            if(_currentDialog < _sceneData[_currentScene]._dialogData.Count)
            {
                ActivateDialog(_currentScene, _currentDialog);
                _currentDialog++;
                return;
            }
            else if(_currentScene == 0)
            {
                TipsAndGuidesManager._instance.ActivateGuideSeries(Guide.DEMOLISH);
            }
            else if (_currentScene == 1)
            {
                TipsAndGuidesManager._instance.ActivateGuideSeries(Guide.CAMERA_CONTROLS);
            }

            _gameManager.SetGameState(GameState.GAME);
            _currentDialog = 1;
        }
    }

    private void PrepareCutscene01()
    {
        GameObject sceneObject = Instantiate(_sceneData[0]._cutceneTool);
        _scenePosition = sceneObject.transform;
        _cameraController = Camera.main.transform.GetComponent<CameraController>();

        SpawnStartPosition(sceneObject);
        SpawnFirstUnits(sceneObject);
        foreach (GameObject unit in _spawnedUnits)
        {
            switch (unit.GetComponent<Unit>()._unitType)
            {
                case UnitType.Harvester:
                    _sceneData[0]._dialogData[5]._speaker = unit.gameObject;
                    break;
                case UnitType.Builder:
                    _sceneData[0]._dialogData[1]._speaker = unit.gameObject;
                    _sceneData[0]._dialogData[3]._speaker = unit.gameObject;
                    _sceneData[0]._dialogData[6]._speaker = unit.gameObject;
                    break;
                case UnitType.Worker:
                    _sceneData[0]._dialogData[0]._speaker = unit.gameObject;
                    _sceneData[0]._dialogData[2]._speaker = unit.gameObject;
                    _sceneData[0]._dialogData[4]._speaker = unit.gameObject;
                    break;
            }
        }
    }

    private void PrepareCutscene02()
    {
        GameObject sceneObject = Instantiate(_sceneData[1]._cutceneTool, _scenePosition.position, _scenePosition.rotation, _scenePosition);
        sceneObject.transform.localPosition = new Vector3(0, 0, -2f);
        CutsceneTool sceneTool = sceneObject.GetComponent<CutsceneTool>();
        int i = 0;
        foreach (GameObject unit in _spawnedUnits)
        {
            RaycastHit ray01;
            if (Physics.Raycast(sceneTool._cutsceneUnitPositions[i].position + new Vector3(0, 10, 0), Vector3.down, out ray01, Mathf.Infinity, _layerMask))
            {
                List<Vector3> t = new List<Vector3>();
                t.Add(ray01.point);
                GroupManager.Instance._selectedUnits = new List<Unit>();
                GroupManager.Instance._selectedUnits.Add(unit.GetComponent<Unit>());
                GroupManager.Instance.PlayerMovementInput(sceneTool._cutsceneCenterPoint.position, t, sceneTool._cutsceneUnitPositions[i].eulerAngles);
            }
            i++;

            switch (unit.GetComponent<Unit>()._unitType)
            {
                case UnitType.Harvester:
                    _sceneData[1]._dialogData[1]._speaker = unit.gameObject;
                    _sceneData[1]._dialogData[3]._speaker = unit.gameObject;
                    break;
                case UnitType.Builder:
                    _sceneData[1]._dialogData[0]._speaker = unit.gameObject;
                    break;
                case UnitType.Worker:
                    _sceneData[1]._dialogData[2]._speaker = unit.gameObject;
                    break;
            }
        }
        Destroy(sceneTool);
    }

    private void ActivateDialog(int sceneIndex, int dialogIndex)
    {
        if(_dialogIndex < _sceneData[sceneIndex]._dialogData.Count)
        {
            GameManager._instance._dialogScreen.StartDialog(_sceneData[sceneIndex]._dialogData[dialogIndex]);
            _cameraController.TransformToTarget(TagFinder.FindComponentInChildWithTag<Transform>(_sceneData[sceneIndex]._dialogData[dialogIndex]._speaker, "CameraFocusPoint").transform, Ease.InOutSine, 1f);

            switch (_sceneData[sceneIndex]._dialogData[dialogIndex]._emotion)
            {
                case DialogEmotion.AGITATED:
                    _sceneData[sceneIndex]._dialogData[dialogIndex]._speaker.GetComponent<AnimationStateController>().TriggerResponse(Response.Frustration);
                    break;
                case DialogEmotion.HAPPY:
                    break;
                case DialogEmotion.NEUTRAL:
                    break;
                case DialogEmotion.SAD:
                    break;
                case DialogEmotion.ANGRY:
                    _sceneData[sceneIndex]._dialogData[dialogIndex]._speaker.GetComponent<AnimationStateController>().TriggerResponse(Response.Angry);
                    break;
            }

            _dialogIndex++;
        }
    }

    private void SpawnStartPosition(GameObject parentTransform)
    {
        List<Cell> correctType = CollectPossibleSpawnCells(parentTransform);

        int randomCellIndex = Random.Range(0, correctType.Count);
        Vector3 rayOffset = new Vector3(0, 10f, 0);
        Vector3 rayDirection = parentTransform.transform.TransformDirection(Vector3.down);
        Vector3 rayPosition = correctType[randomCellIndex]._worldPosition + rayOffset;

        RaycastHit ray;
        if (Physics.Raycast(rayPosition, rayDirection, out ray, Mathf.Infinity, _layerMask))
        {
            parentTransform.transform.position = ray.point;

            //Rotate spawn-parent to coast
            Vector3 left = Vector3.Cross(ray.normal, Vector3.up);
            Vector3 slope = Vector3.Cross(ray.normal, left);
            parentTransform.transform.LookAt(parentTransform.transform.position + slope, Vector3.up);
        }

        Instantiate(_wreck, parentTransform.transform.position, parentTransform.transform.rotation);

        _cameraController.UnparentCameraAfterSpawn();
    }

    private List<Cell> CollectPossibleSpawnCells(GameObject parentTransform)
    {
        List<Cell> correctType = new List<Cell>();
        List<Cell> allCells = GridController.Instance.GenesisField._allCells;

        foreach (Cell cell in allCells)
        {
            if (cell._type == CellType.shallowWater)
            {
                Vector3 rayOffset = new Vector3(0, 10f, 0);
                Vector3 rayDirection = parentTransform.transform.TransformDirection(Vector3.down);
                Vector3 rayPosition = cell._worldPosition + rayOffset;

                RaycastHit ray01;
                if (Physics.Raycast(rayPosition, rayDirection, out ray01, Mathf.Infinity, _layerMask))
                {
                    if (ray01.point.y > .5f)
                    {
                        correctType.Add(cell);
                    }
                }
            }
        }
        return correctType;
    }

    private void SpawnFirstUnits(GameObject toSpawnUnder)
    {
        float pos = -.8f;
        foreach (GameObject unit in _unitsToSpawn)
        {
            Vector3 position = new Vector3(pos, 1.5f, 1f);
            pos += .7f;
            GameObject u = Instantiate(unit, Vector3.zero, Quaternion.identity, toSpawnUnder.transform);
            u.transform.localPosition = position;
            u.transform.rotation = toSpawnUnder.transform.transform.rotation;
            u.transform.Rotate(new Vector3(0f, 180f, 0f));
            _spawnedUnits.Add(u);

            //Walk unit
            PlayableDirector playableDirector = u.GetComponent<PlayableDirector>();
            playableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
            float time = Random.Range(.8f, 1.2f);
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(time);
            playableDirector.Play();
            u.transform.DOLocalMoveZ(Random.Range(-1.5f, -2.5f), ((float)u.GetComponent<PlayableDirector>().duration * time) - (.65f * time)).SetEase(Ease.OutSine);
        }
    }

    private void UnParentAllChildren()
    {
        int x = transform.childCount - 1;
        for (int i = x; i >= 0; i--)
            transform.GetChild(i).parent = null;
    }
    private void OnDestroy()
    {
        DialogScreen.OnDialogFinished -= ContinueDialog;
    }
}

public static class TagFinder
{
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }
}
