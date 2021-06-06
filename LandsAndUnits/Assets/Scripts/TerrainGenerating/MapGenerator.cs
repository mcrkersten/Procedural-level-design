using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitsAndFormation;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
	#region SINGLETON PATTERN
	private static MapGenerator _instance;
	public static MapGenerator Instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<MapGenerator>();

				if (_instance == null)
				{
					Debug.LogError("No MapGenerator");
				}
			}

			return _instance;
		}
	}
	#endregion

	public Texture2D falloffMapImage;
	public float _offset;
	public float _offset2;
	public Vector2 _damp;

	[Header("")]
	public TerrainData _terrainData;
	public NoiseData _noiseData;
	public TextureData _textureData;
	public Material _terrainMaterial;

	public UnitsAndFormation.CellType[,] cellTypes;

	public const int mapChunkSize = 100;

	[Range(0,6)]
	public int levelOfDetail;

	public bool autoUpdate;

	public TerrainType[] regions;

	float[,] falloffMap;
	float[,] generatedMap;
	Color[] colourMap;

	public GameObject MeshObject;

	public delegate void MapGenerated();
	public static event MapGenerated OnMapGenerated;

	public int flowfieldNumber = 0;
	public void Awake()
    {
		OnValidate();
		falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, _offset, falloffMapImage);
		_noiseData.OnValuesUpdated += OnValuesUpdated;
		GameManager.OnScenesLoaded += BuildMapAfterLoadScene;

		MeshObject.AddComponent<MeshCollider>();
	}

	private void BuildMapAfterLoadScene()
    {
		if(GameManager._instance.SaveFileData != null)
			_noiseData.seed = GameManager._instance.SaveFileData.seed;
		GenerateMap();
	}

	private void OnValuesUpdated()
    {
		GenerateMap();
	}

	private void OnTextureValuesUpdated()
    {
		_textureData.ApplyToMaterial(_terrainMaterial, regions);
    }

    public void GenerateMap() {

		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, _noiseData.seed, _noiseData.noiseScale, _noiseData.octaves, _noiseData.persistance, _noiseData.lacunarity, _noiseData.offset);
		noiseMap = Falloff(noiseMap);
		generatedMap = noiseMap;

		cellTypes = new UnitsAndFormation.CellType[mapChunkSize, mapChunkSize];
		colourMap = new Color[mapChunkSize * mapChunkSize];

		//Create cellTypes and ColourMap
        for (int yy = 0; yy < mapChunkSize; yy++) {
            for (int xx = 0; xx < mapChunkSize; xx++)
			{
				float currentHeight = noiseMap[xx, yy];
				for (int i = 0; i < regions.Length; i++)
				{
					if (currentHeight < regions[i].height)
					{
						colourMap[yy * mapChunkSize + xx] = regions[i].colour;
						cellTypes[xx, yy] = regions[i].type;
						break;
					}
				}
			}
        }


		DrawMesh();
		//Creates Genesisfield
		OnMapGenerated?.Invoke();

	}

	private float[,] Falloff(float[,] map)
	{
		for (int y = 0; y < mapChunkSize; y++)
		{
			for (int x = 0; x < mapChunkSize; x++)
			{
				if (_terrainData.useFalloff)
				{
					map[x, y] = Mathf.Clamp01(map[x, y] - Normalize((falloffMap[x, y] * _offset2), _damp.x, _damp.y));
				}
			}
		}
		return map;
	}

	public void UpdateMap(List<Cell> points, EditType type, float height = default)
    {
		float average = 0;
        switch (type)
        {
            case EditType.Smoothing:
				foreach (Cell c in points)
					average += generatedMap[c._gridIndex.x, c._gridIndex.y];
				average = average / points.Count;

				foreach (Cell c in points)
					generatedMap[c._gridIndex.x, c._gridIndex.y] = average;
				break;
            case EditType.Add:
                break;
            case EditType.Remove:
                break;
        }


		DrawMesh();
	}

	private float Normalize(float value, float min, float max)
	{
	float normalized = (value - min) / (max - min);
		return normalized;
	}

	private void DrawMesh()
    {

		MapDisplay display = FindObjectOfType<MapDisplay>();
		display.DrawMesh(MeshGenerator.GenerateTerrainMesh(generatedMap, _terrainData.meshHeightMultiplier, _terrainData.meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		_textureData.ApplyToMaterial(_terrainMaterial, regions);

		if (MeshObject.GetComponent<MeshCollider>() != null)
		{
			MeshObject.GetComponent<MeshCollider>().sharedMesh = null;
			MeshObject.GetComponent<MeshCollider>().sharedMesh = display.meshFilter.mesh;
		}
	}

	void OnValidate() {
		if(_terrainData != null)
        {
			_terrainData.OnValuesUpdated -= OnValuesUpdated;
			_terrainData.OnValuesUpdated += OnValuesUpdated;
        }

		if (_noiseData != null)
		{
			_noiseData.OnValuesUpdated -= OnValuesUpdated;
			_noiseData.OnValuesUpdated += OnValuesUpdated;
		}

		if (_textureData != null)
		{
			_textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			_textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}

		falloffMap = falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, _offset, falloffMapImage);
	}

    private void OnDestroy()
    {
		GameManager.OnScenesLoaded -= BuildMapAfterLoadScene;
		_noiseData.OnValuesUpdated -= OnValuesUpdated;
	}

}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
	public UnitsAndFormation.CellType type;
}

public enum EditType
{
	Smoothing = 0,
	Add,
	Remove
}