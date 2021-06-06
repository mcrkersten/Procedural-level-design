using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute()]
public class TextureData : UpdatableData
{
    float savedMinHeight;
    float savedMaxHeight;
    public void ApplyToMaterial(Material material, TerrainType[] terrains)
    {
        for (int i = 0; i < terrains.Length; i++)
        {
            material.SetColor("C" + i.ToString(), terrains[i].colour);
            //material.SetFloat("H" + i.ToString(), MapGenerator.Instance._terrainData.meshHeightCurve.Evaluate(terrains[i].height));
        }
    }
}
 