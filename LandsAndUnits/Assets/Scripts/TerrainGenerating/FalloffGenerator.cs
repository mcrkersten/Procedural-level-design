using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int size, float offset, Texture2D falloffMapImage)
	{
		float[,] map = new float[size, size];
		// multiply heightmap values by the falloff map values  (black is 0, grey is 0.5, white is 1)
		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
			{
				map[z, x] = offset - falloffMapImage.GetPixel(x,z).grayscale;
			}
		}
		return map;
	}

	static float Evaluate(float value, float a, float b) {
		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}