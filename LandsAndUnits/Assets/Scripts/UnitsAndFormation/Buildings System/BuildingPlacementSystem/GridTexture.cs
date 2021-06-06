using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GridTexture
{
    public Transform parent;
    public List<GameObject> _gridGameObjects = new List<GameObject>();
    private List<GridPoint> _gridPoints = new List<GridPoint>();
    private Color _baseColor;

    public void CreatePoints()
    {
        foreach (GameObject gp in _gridGameObjects)
            _gridPoints.Add(new GridPoint(gp));
    }

    public void SetStrenght(float _amount)
    {
        Color color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, _amount);
        foreach (GridPoint point in _gridPoints)
            point.SetColor(color);
    }

    public void SetColor(Color color)
    {
        _baseColor = color;
        foreach (GridPoint point in _gridPoints)
            point.SetColor(_baseColor);
    }

    public void DisableGrid()
    {
        foreach (GridPoint point in _gridPoints)
            point.DisableRenderer();
    }

    public void EnableGrid()
    {
        foreach (GridPoint point in _gridPoints)
            point.EnableRenderer();
    }

    private class GridPoint
    {
        public GameObject _gameObject;
        public Renderer _renderer;
        public Vector2 _position;
        public GridPoint(GameObject gameObject)
        {
            _gameObject = gameObject;
            _renderer = gameObject.GetComponent<Renderer>();
        }

        public void DisableRenderer()
        {
            _renderer.enabled = false;
        }

        public void EnableRenderer()
        {
            _renderer.enabled = true;
        }

        public void SetColor(Color c)
        {
            _renderer.material.SetColor("_Color", c);
        }
    }
}
