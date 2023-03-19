using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class PerlinTerrainGenerator : MonoBehaviour
{
    [Header("Assign - Textures 1-2")]
    public Tilemap tilemap;
    public Tile texture1;
    public Tile texture2;
    public Tile texture3;
    public Tile texture4;
    
    [Header("Assign - Texture Ranges 1")]
    [Range(0f, 1f)] public float limit1;
    [Range(0f, 1f)] public float limit2;
    [Range(0f, 1f)] public float limit3;
    [Range(0f, 1f)] public float limit4;
    
    
    [Header("Assign - Common")]
    public Vector2Int chunkSize;
    public int seed;
    public int octaveNumber = 3;
    
    [Header("Assign - First Noise Map")]
    public Vector2Int manualOffset;
    public float lacunarity;
    [Range(0f, 1f)] public float persistence;
    public float noiseScale;

    [Header("Don't Touch - Variables")]
    public Vector2 offset;
    public float frequency;
    public float amplitude;
    public float noise;
    public float perlinValue;
    public Tile selectedTile;
    
    // NOISE MAPS - Inspector doesn't support dictionaries
    public Dictionary<Vector2Int, float> terrainNoiseMap;
    
    private void Update()
    {
        //reset
        tilemap.ClearAllTiles();
        //reset
        
        //initializing dictionary
        terrainNoiseMap = new Dictionary<Vector2Int, float>();
        //initializing dictionary
        
        //generating offsets
        Random.InitState(seed);
        offset = new Vector2(Random.Range(-100000f, 100000f), Random.Range(-100000f, 100000f));
        //generating offsets

        //generating noise map
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                frequency = 1;
                amplitude = 1;
                noise = 0;

                for (int i = 0; i < octaveNumber; i++)
                {
                    float xValue = (x + manualOffset.x) / noiseScale * frequency + offset.x;
                    float yValue = (y + manualOffset.y) / noiseScale * frequency + offset.y;

                    perlinValue = Mathf.PerlinNoise(xValue, yValue);
                    noise += perlinValue * amplitude;
                    
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                terrainNoiseMap.Add(new Vector2Int(x, y), Mathf.Clamp(noise, 0f, 1f));
            }
        }
        //generating noise map
        
        //generating textures
        foreach (var item in terrainNoiseMap)
        {
            if (item.Value >= 0 && item.Value <= limit1)
            {
                tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture1);
            }
            else if (item.Value > limit1 && item.Value <= limit2)
            {
                tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture2);
            }
            else if (item.Value > limit2 && item.Value <= limit3)
            {
                tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture3);
            }
            else if (item.Value > limit3 && item.Value <= limit4)
            {
                tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture4);
            }
        }
        //generating textures
    }
}
