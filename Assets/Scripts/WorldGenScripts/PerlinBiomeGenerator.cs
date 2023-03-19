using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

//[ExecuteAlways]
public class PerlinBiomeGenerator : MonoBehaviour
{
    public bool one;
    public bool two;
    [Header("Assign - Textures 1-2")]
    public Tilemap tilemap;
    public Tile texture1;
    public Tile texture2;
    public Tile texture3;
    
    [Header("Assign - Textures Common")]
    public Tile texture4;
    public Tile texture5;
    public Tile texture6;
    public Tile texture7;
    public Tile texture8;
    public Tile texture9;
    public Tile texture10;
    public Tile texture11;
    public Tile texture12;
    
    [Header("Assign - Texture Ranges 1")]
    [Range(0f, 1f)] public float limitOne1;
    [Range(0f, 1f)] public float limitOne2;
    [Range(0f, 1f)] public float limitOne3;
    
    [Header("Assign - Texture Ranges 2")]
    [Range(0f, 1f)] public float limitTwo1;
    [Range(0f, 1f)] public float limitTwo2;
    [Range(0f, 1f)] public float limitTwo3;
    
    [Header("Assign - Common")]
    public Vector2Int chunkSize;
    public int seed;
    public int octaveNumber = 3;
    
    [Header("Assign - First Noise Map")]
    public Vector2Int manualOffset1;
    public float firstLacunarity;
    [Range(0f, 1f)] public float firstpersPersistence;
    public float firstNoiseScale;
    
    [Header("Assign - Second Noise Map")]
    public Vector2Int manualOffset2;
    public float secondLacunarity;
    [Range(0f, 1f)] public float secondPersistence;
    public float secondNoiseScale;

    [Header("Don't Touch - Variables")]
    public Vector2 offset;
    public float frequency;
    public float amplitude;
    public float noise;
    public float perlinValue;
    public Tile selectedTile;
    
    // NOISE MAPS - Inspector doesn't support dictionaries
    public Dictionary<Vector2Int, float> firstNoiseMap;
    public Dictionary<Vector2Int, float> secondNoiseMap;
    
    private void Start()
    {
        //reset
        tilemap.ClearAllTiles();
        //reset
        
        //initializing dictionaries
        firstNoiseMap = new Dictionary<Vector2Int, float>();
        secondNoiseMap = new Dictionary<Vector2Int, float>();
        //initializing dictionaries
        
        //generating offsets
        Random.InitState(seed);
        offset = new Vector2(Random.Range(-100000f, 100000f), Random.Range(-100000f, 100000f));
        //generating offsets

        //first noise map
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                frequency = 1;
                amplitude = 1;
                noise = 0;

                for (int i = 0; i < octaveNumber; i++)
                {
                    float xValue = (x + manualOffset1.x) / firstNoiseScale * frequency + offset.x;
                    float yValue = (y + manualOffset1.y) / firstNoiseScale * frequency + offset.y;

                    perlinValue = Mathf.PerlinNoise(xValue, yValue);
                    noise += perlinValue * amplitude;
                    
                    amplitude *= firstpersPersistence;
                    frequency *= firstLacunarity;
                }
                firstNoiseMap.Add(new Vector2Int(x, y), Mathf.Clamp(noise, 0f, 1f));
            }
        }
        //first noise map
        
        //second noise map
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                frequency = 1;
                amplitude = 1;
                noise = 0;

                for (int i = 0; i < octaveNumber; i++)
                {
                    float xValue = (x + manualOffset2.x) / secondNoiseScale * frequency + offset.x;
                    float yValue = (y + manualOffset2.y) / secondNoiseScale * frequency + offset.y;

                    perlinValue = Mathf.PerlinNoise(xValue, yValue);
                    noise += perlinValue * amplitude;
                    
                    amplitude *= secondPersistence;
                    frequency *= secondLacunarity;
                }
                secondNoiseMap.Add(new Vector2Int(x, y), Mathf.Clamp(noise, 0f, 1f));
            }
        }
        //second noise map

        //generating textures
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                if (one)
                {
                    if (firstNoiseMap[new Vector2Int(x, y)]  >= 0 && firstNoiseMap[new Vector2Int(x, y)]  <= limitOne1)
                    {
                        tilemap.SetTile(new Vector3Int(x, y), texture1);
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)]  > limitOne1 && firstNoiseMap[new Vector2Int(x, y)]  <= limitOne2)
                    {
                        tilemap.SetTile(new Vector3Int(x, y), texture2);
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)]  > limitOne2 && firstNoiseMap[new Vector2Int(x, y)]  <= limitOne3)
                    {
                        tilemap.SetTile(new Vector3Int(x, y), texture3);
                    }
                }
            
                else if (two)
                {
                    if (secondNoiseMap[new Vector2Int(x, y)] >= 0 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo1)
                    {
                        tilemap.SetTile(new Vector3Int(x, y), texture1);
                    }
                    else if (secondNoiseMap[new Vector2Int(x, y)] > limitTwo1 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo2)
                    {
                        tilemap.SetTile(new Vector3Int(x, y), texture2);
                    }
                    else if (secondNoiseMap[new Vector2Int(x, y)] > limitTwo2 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo3)
                    {
                        tilemap.SetTile(new Vector3Int(x, y), texture3);
                    }
                }

                else
                {
                    if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne1 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo1)
                    {
                        selectedTile = texture4;
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne1 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo2)
                    {
                        selectedTile = texture5;
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne1 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo3)
                    {
                        selectedTile = texture6;
                    }
                    //
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne2 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo1)
                    {
                        selectedTile = texture7;
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne2 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo2)
                    {
                        selectedTile = texture8;
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne2 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo3)
                    {
                        selectedTile = texture9;
                    }
                    //
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne3 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo1)
                    {
                        selectedTile = texture10;
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne3 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo2)
                    {
                        selectedTile = texture11;
                    }
                    else if (firstNoiseMap[new Vector2Int(x, y)] <= limitOne3 && secondNoiseMap[new Vector2Int(x, y)] <= limitTwo3)
                    {
                        selectedTile = texture12;
                    }
                
                    tilemap.SetTile(new Vector3Int(x, y), selectedTile);
                }
            }
        }
        //generating textures
    }
}
