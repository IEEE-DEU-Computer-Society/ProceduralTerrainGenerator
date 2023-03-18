using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class PerlinBiomeGenerator : MonoBehaviour
{
    public bool one;
    public bool two;
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public Tile layerOne;
    public Tile layerTwo;
    public Tile layerThree;
    public Tile layerFour;
    public Tile layerFive;
    public Tile layerSix;
    public Tile layerSeven;
    public Tile layerEight;
    
    [Header("Assign - Texture Ranges")]
    [Range(0f, 1f)] public float limitOne;
    [Range(0f, 1f)] public float limitTwo;
    [Range(0f, 1f)] public float limitThree;
    [Range(0f, 1f)] public float limitFour;
    
    [Header("Assign - Common")]
    public Vector2Int chunkSize;
    public int seed;
    public Vector2Int manualOffset;
    public int octaveNumber = 3;
    
    [Header("Assign - First Noise Map")]
    public float firstLacunarity;
    [Range(0f, 1f)] public float firstpersPersistence;
    public float firstNoiseScale;
    
    [Header("Assign - Second Noise Map")]
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
    public Dictionary<Vector2Int, float> firstNoiseMap;     //like humidity
    public Dictionary<Vector2Int, float> secondNoiseMap;    //like temperature
    
    private void Update()
    {
        tilemap.ClearAllTiles();
        
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
                    float xValue = (x + manualOffset.x) / firstNoiseScale * frequency + offset.x;
                    float yValue = (y + manualOffset.y) / firstNoiseScale * frequency + offset.y;

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
                    float xValue = (x + manualOffset.x) / secondNoiseScale * frequency + offset.x;
                    float yValue = (y + manualOffset.y) / secondNoiseScale * frequency + offset.y;

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
        foreach (var item in firstNoiseMap)
        {
            if (one)
            {
                if (item.Value >= 0 && item.Value <= limitOne)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerFour);
                }
                else if (item.Value > limitOne && item.Value <= limitTwo)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerTwo);
                }
                
                else if (item.Value > limitTwo && item.Value <= limitThree)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerThree);
                }
                else if (item.Value > limitThree && item.Value <= limitFour)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerFour);
                }
            }
            
            else if (two)
            {
                if (secondNoiseMap[item.Key] >= 0 && secondNoiseMap[item.Key] <= limitOne)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerOne);
                }
                else if (secondNoiseMap[item.Key] > limitOne && secondNoiseMap[item.Key] <= limitTwo)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerTwo);
                }
                
                else if (secondNoiseMap[item.Key] > limitTwo && secondNoiseMap[item.Key] <= limitThree)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerThree);
                }
                else if (secondNoiseMap[item.Key] > limitThree && secondNoiseMap[item.Key] <= limitFour)
                {
                    tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), layerFour);
                }
            }

            else
            {
                if (item.Value <= limitOne && secondNoiseMap[item.Key] <= limitThree)
                {
                    selectedTile = layerFive;
                }
            
                if (item.Value <= limitOne && secondNoiseMap[item.Key] > limitThree && secondNoiseMap[item.Key] <= limitFour)
                {
                    selectedTile = layerSix;
                }

                if (item.Value > limitOne && item.Value <= limitTwo && secondNoiseMap[item.Key] <= limitThree)
                {
                    selectedTile = layerSeven;
                }
            
                if (item.Value > limitOne && item.Value <= limitTwo && 
                         secondNoiseMap[item.Key] > limitThree && secondNoiseMap[item.Key] <= limitFour)
                {
                    selectedTile = layerEight;
                }
                
                tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), selectedTile);
            }
        }
        //generating textures
    }
}
