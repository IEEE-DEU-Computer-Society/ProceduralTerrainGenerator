using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinGeneratorBase : MonoBehaviour
{
    protected Dictionary<Vector2Int, float> GenerateNoiseMap(Vector2Int offset, float octaveNumber, float noiseScale, float persistence, float lacunarity)
    {
        Dictionary<Vector2Int, float> noiseMap = new Dictionary<Vector2Int, float>();
        
        for (int x = 0; x <= 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                float frequency = 1;
                float amplitude = 1;
                float noise = 0;

                for (int i = 0; i < octaveNumber; i++)
                {
                    float xValue = (x + offset.x) / noiseScale * frequency;
                    float yValue = (y + offset.y) / noiseScale * frequency;

                    float perlinValue = Mathf.PerlinNoise(xValue, yValue);
                    noise += perlinValue * amplitude;
                    
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                
                noiseMap.Add(new Vector2Int(x, y), Mathf.Clamp(noise, 0f, 1f));
            }
        }
        
        return noiseMap;
    }
    
    protected void GenerateTiles(Tilemap tilemap, Dictionary<Vector2Int, float> noiseMap, List<Tile> tileList, List<float> tileRangeList)
    {
        foreach (var i in noiseMap)
        {
            //First Layer
            if (i.Value >= 0 && i.Value <= tileRangeList[0])
            {
                tilemap.SetTile(new Vector3Int(i.Key.x, i.Key.y), tileList[0]);
            }

            //Last Layer
            else if (i.Value > tileRangeList[^1] && i.Value <= 1)
            {
                tilemap.SetTile(new Vector3Int(i.Key.x, i.Key.y), tileList[^1]);
            }
            
            //Middle Layers
            for (int j = 1 ; j < tileList.Count - 1; j++)
            {
                if (i.Value > tileRangeList[j - 1] && i.Value <= tileRangeList[j])
                {
                    tilemap.SetTile(new Vector3Int(i.Key.x, i.Key.y), tileList[j]);
                }
            }
        }
    }
}
