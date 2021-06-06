using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class AnimalLogic : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static AnimalLogic _instance;
    public static AnimalLogic Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AnimalLogic>();

                if (_instance == null)
                {
                    Debug.LogError("No AnimalLogic");
                }
            }
            return _instance;
        }
    }
    #endregion

    public List<AnimalGroup> _groups = new List<AnimalGroup>();
    public List<Transform> _spawnPositions = new List<Transform>();
    public GameObject _animalPrefab;
    public List<Transform> _navPoints = new List<Transform>();

    private void Awake()
    {
        AnimalGroup.OnRequestNewNavigation += NewNavigationRequest;
        foreach (Transform point in _spawnPositions)
        {
            CreateNewAnimalGroup(Random.Range(2, 4), point);
        }
    }


    private void FixedUpdate()
    {
        foreach (UnitGroup d in _groups)
        {
            d.BehaviourTick();
        }
    }

    private UnitGroup CreateNewAnimalGroup(int amountOfAnimals, Transform position)
    {
        List<Vector2> points = FormationCreator.CreateFormation(amountOfAnimals, 2f, FormationType.randomInArea);
        List<Unit> animals = CreateAnimals(points, position);
        AnimalGroup x = new AnimalGroup(animals, UnitType.Animal, GroupBehaviourState.Idle, Vector3.zero);
        _groups.Add(x);
        return x;
    }

    private List<Unit> CreateAnimals(List<Vector2> points, Transform position)
    {
        List<Unit> spawned = new List<Unit>();
        foreach (Vector2 p in points)
        {
            GameObject a = Instantiate(_animalPrefab, position.position, position.rotation, position);
            a.transform.localPosition = new Vector3(p.x, 0.1f, p.y) * 1f;
            spawned.Add(a.GetComponent<Unit>());
        }
        return spawned;
    }

    private void NewNavigationRequest(AnimalGroup theRequester)
    {
        if (theRequester._navIndex >= _navPoints.Count)
            theRequester._navIndex = 0;

        List<Vector2> test = FormationCreator.CreateFormation(theRequester._units.Count, 2f, FormationType.randomInArea);

        List<Vector3> _worldPositions = new List<Vector3>();
        foreach (Vector2 t in test)
        {
            Vector3 x = _navPoints[theRequester._navIndex].TransformPoint(new Vector3(t.x,0,t.y));
            _worldPositions.Add(x);
        }

        theRequester.UpdateNavigation(_navPoints[theRequester._navIndex++].position, _worldPositions, Vector3.zero);
    }

    [ContextMenu("test")]
    public void Temp()
    {
        NewNavigationRequest(_groups[0]);
    }
}
