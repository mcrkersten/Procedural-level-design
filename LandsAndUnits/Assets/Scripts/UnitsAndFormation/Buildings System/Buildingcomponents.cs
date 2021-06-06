using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnitsAndFormation;

public class Buildingcomponents : MonoBehaviour
{
    public InteractableInformation _interactableInformation;
    public ResourceInformation _resourceInformation;
    public Transform _targetPoint;
    public GameObject _fence;
    public GameObject _model;
    public GameObject _unValidModel;
    public Collider _constructionCollider;
    public Collider _buildingCollider;

    public Construction _construction;
    private InputManager _inputManager;
    public SphereCollider _triggerCollider;
    public bool _isEvenGrid;
    public List<Vector2> _buildingPoints = new List<Vector2>();

    private List<Vector2> _localSnapPoints = new List<Vector2>();

    private List<Vector3> _worldSnapPoints = new List<Vector3>();
    public List<Vector3> WorldPositions { 
        private set { 
            _worldSnapPoints = value; 
        } 
        get { 
            return _worldSnapPoints; 
        } 
    }

    private void Start()
    {
        _inputManager = InputManager.Instance;
    }
    private void Update()
    {
        PopulateWorldSnapPoints();

        if(transform.parent == null)
        {
            if (_inputManager._inputState == InputState.BUILDING_PLACEMENT)
                _triggerCollider.enabled = true;
            else
                _triggerCollider.enabled = false;
        }

        foreach (Vector3 local in WorldPositions)
        {
            Debug.DrawLine(local, new Vector3(local.x, 5, local.z), Color.red);
        }
        foreach (Vector2 local in _buildingPoints)
        {
            Vector3 world = this.transform.TransformPoint(new Vector3(local.x, 0, local.y));
            //Debug.DrawLine(world, new Vector3(world.x, 5, world.z), Color.yellow);
        }
    }

    public void PopulateLocalSnapPoints()
    {
        //Create LocalSnapPoints
        List<Vector2> total = new List<Vector2>();
        total = CreateLocalSnapPoints();

        //Remove duplicates
        total = total.Distinct().ToList();

        //Remove points that sit on buildingPoints
        foreach (Vector2 p in _buildingPoints)
            total.Remove(p);

        _localSnapPoints = total;
    }

    public List<Vector2> CreateLocalSnapPoints()
    {
        List<Vector2> total = new List<Vector2>();
        foreach (Vector2 p in _buildingPoints)
        {
            List<Vector2> temp = new List<Vector2>();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    float xx = 0;
                    if (_isEvenGrid)
                    {
                        if (x < 0)
                            xx = x + .5f;
                        else
                            xx = x - .5f;
                    }
                    else
                    {
                        xx = x;
                    }

                    float yy = 0;
                    if (_isEvenGrid)
                    {
                        if (y < 0)
                            yy = y + .5f;
                        else
                            yy = y - .5f;
                    }
                    else
                    {
                        yy = y;
                    }

                    temp.Add(new Vector2(xx, yy));
                }
            }
            total.AddRange(temp);
        }
        return total;
    }

    public void PopulateWorldSnapPoints()
    {
        WorldPositions = new List<Vector3>();
        foreach (Vector2 local in _localSnapPoints)
        {
            Vector3 pos = this.transform.TransformPoint(new Vector3(local.x, 0, local.y));
            WorldPositions.Add(pos);
            Debug.DrawLine(pos, new Vector3(pos.x, 5, pos.z), Color.red);
        }
    }
}
