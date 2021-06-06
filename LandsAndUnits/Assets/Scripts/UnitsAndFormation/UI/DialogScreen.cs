using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speaker;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private TextMeshProUGUI _clickToContinueText;
    [SerializeField] private GameObject _canvas;

    public delegate void FinishedDialogObject();
    public static event FinishedDialogObject OnDialogFinished;

    private GameManager _gameManager;
    private bool _canClickToContinue;
    private DialogData _currentDialog;
    private int _currentSentenceIndex = 0;

    private void Start()
    {
        _gameManager = GameManager._instance;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_gameManager._gameState == GameState.CUTSCENE)
            {
                if (_canClickToContinue)
                {
                    _currentSentenceIndex++;
                    _canClickToContinue = false;
                    if (_currentSentenceIndex < _currentDialog._dialog.Count)
                    {
                        NextSentence(_currentDialog._dialog[_currentSentenceIndex]);
                    }
                    else
                    {
                        EndDialog();
                    }
                }
            }
        }
    }

    public void StartDialog(DialogData data)
    {
        _canClickToContinue = false;
        _currentDialog = data;
        _speaker.text = _currentDialog._speakerName;
        _dialogText.text = _currentDialog._dialog[0];
        _canvas.SetActive(true);
        StartCoroutine(WaitForClickToContinue(1.1f));
    }

    public void EndDialog()
    {
        _canvas.SetActive(false);
        _currentDialog = null;
        _dialogText.text = "";
        _speaker.text = "";
        _currentSentenceIndex = 0;

        OnDialogFinished?.Invoke();
    }


    private void NextSentence(string dialog)
    {
        _dialogText.text = dialog;
        StartCoroutine(WaitForClickToContinue(.1f));
    }

    private IEnumerator WaitForClickToContinue(float time)
    {
        _clickToContinueText.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        _canClickToContinue = true;
        _clickToContinueText.gameObject.SetActive(true);
    }
}