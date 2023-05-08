using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class PerlinTerrainGenerator : MonoBehaviour
{
    [Header("Assign - Biome Info")]
    public PerlinBiomeGenerator biomeGenerator;
    
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public Tile texture1;
    public Tile texture2;
    public Tile texture3;
    public Tile texture4;
    public Tile texture5;
    public Tile texture6;
    public Tile texture7; //forest
    public Tile texture8; //forest
    
    [Header("Assign - Texture Ranges")]
    [Range(0f, 1f)] public float limit1;
    [Range(0f, 1f)] public float limit2;
    [Range(0f, 1f)] public float limit3;
    [Range(0f, 1f)] public float limit4; //defines limit5
    [Range(0f, 1f)] public float limit5;

    [Header("Assign - Noise Map")]
    public Vector2Int chunkSize;
    public int seed;
    public int octaveNumber = 3;
    public float noiseScale;
    public Vector2Int manualOffset;
    public float lacunarity;
    [Range(0f, 1f)] public float persistence;

    [Header("Don't Touch - Variables")]
    public Vector2 offset;
    public float frequency;
    public float amplitude;
    public float noise;
    public float perlinValue;
    
    // NOISE MAPS - Inspector doesn't support dictionaries
    public Dictionary<Vector2Int, float> terrainNoiseMap;
    public Dictionary<Vector2Int, int> terrainData;
    public Dictionary<Vector2Int, int> biomeData;

    private void Start()
    {
        //initializing dictionaries
        terrainNoiseMap = new Dictionary<Vector2Int, float>();
        terrainData = new Dictionary<Vector2Int, int>();
        biomeData = biomeGenerator.biomeData;
        //initializing dictionaries

    }

    public void GenerateTerrain(Vector2Int leftBottom)
    {
        //generating offsets
        Random.InitState(seed);
        offset = new Vector2(Random.Range(-100000f, 100000f), Random.Range(-100000f, 100000f));
        //generating offsets

        //generating noise map
        for (int x = leftBottom.x; x < leftBottom.x + chunkSize.x; x++)
        {
            for (int y = 0; leftBottom.y < leftBottom.y + chunkSize.y; y++)
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
            if (biomeData[item.Key] == 0)
            {
                if (item.Value >= 0 && item.Value <= limit1)
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 0);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture1);
                }
                else if (item.Value > limit1 && item.Value <= limit2)
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 1);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture2);
                }
                else if (item.Value > limit2 && item.Value <= limit3)
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 2);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture3);
                }
                else if (item.Value > limit4 && item.Value <= limit5) //forest
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 3);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture7);
                }
            }

            else
            {
                if (item.Value >= 0 && item.Value <= limit1)
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 0);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture4);
                }
                else if (item.Value > limit1 && item.Value <= limit2)
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 1);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture5);
                }
                else if (item.Value > limit2 && item.Value <= limit3)
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 2);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture6);
                }
                else if (item.Value > limit4 && item.Value <= limit5) //forest
                {
                    terrainData.Add(new Vector2Int(item.Key.x, item.Key.y), 3);
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), texture8);
                }
            }
        }
        //generating textures
    }
}
