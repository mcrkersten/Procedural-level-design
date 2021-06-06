using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectiveBuilder : MonoBehaviour
{

    public delegate void CompleteAllObjectives();
    public static event CompleteAllObjectives OnCompleteAllObjectives;

    [SerializeField] private GameObject _objectivePrefab;
    [SerializeField] private List<ObjectiveComponent> _objectives = new List<ObjectiveComponent>();
    private int _guideLenght;
    private int _objectivesFinished;

    public void CreateObjectiveList(TipAndGuideData data)
    {
        _guideLenght = data._data.Count;
        foreach (GuidePoint point in data._data)
        {
            GameObject x = Instantiate(_objectivePrefab, this.transform);
            ObjectiveComponent oc = x.GetComponent<ObjectiveComponent>();
            oc._text.text = point._objective;
            oc._guide = point._guide;
            oc._index = point._index;
            _objectives.Add(oc);
        }

        this.transform.DOMoveX(30f, 1f).SetEase(Ease.OutBack);
    }

    public void OnCompleteObjective(Guide guide, int index)
    {
        foreach (ObjectiveComponent oc in _objectives)
        {
            if(oc._guide == guide && oc._index == index)
            {
                oc.OnComplete();
                _objectivesFinished++;
                if(_guideLenght == _objectivesFinished)
                    OnCompleteAllObjectivesOfGuide();
                break;
            }
        }
    }

    private void OnCompleteAllObjectivesOfGuide()
    {
        this.transform.DOMoveX(-430f, 1f).SetEase(Ease.InBack).OnComplete(DeleteAllInstantiatedObjectives);
        _objectivesFinished = 0;
    }

    private void DeleteAllInstantiatedObjectives()
    {
        ObjectiveComponent[] toDestroy = _objectives.ToArray();
        for (int i = 0; i < toDestroy.Length; i++)
            Destroy(toDestroy[i].gameObject);

        _objectives = new List<ObjectiveComponent>();
    }
}
