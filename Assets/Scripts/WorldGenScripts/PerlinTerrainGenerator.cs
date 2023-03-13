using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[ExecuteInEditMode] //UPDATE FOR EDIT / START FOR PLAY
public class PerlinTerrainGenerator : MonoBehaviour
{
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public Tile layerOne;
    public Tile layerTwo;
    public Tile layerThree;
    public Tile layerFour;
    public Tile layerFive;

    [Header("Assign - Texture Ranges")]
    [Range(0f, 1f)] public float layerOneLimit;
    [Range(0f, 1f)] public float layerTwoLimit;
    [Range(0f, 1f)] public float layerThreeLimit;
    [Range(0f, 1f)] public float layerFourLimit;
    [Range(0f, 1f)] public float layerFiveLimit;

    [Header("Alternate Algorithm")]
    public bool alternateAlgorithm;
    
    [Header("Assign - Information")]
    public int mapWidth;
    public int mapLength;
    public int seed;
    public int octaveNumber;
    
    [Header("Assign - Multipliers")]
    public Vector2 manualOffset;
    public float noiseScale;
    public float lacunarity;
    [Range(0f, 1f)] public float persistence;

    [Header("Variables - Don't Touch")]
    public float noise;
    public float[,] noiseMap;
    public float perlinValue;
    public float minNoise;
    public float maxNoise;
    public Vector2[] octaveOffsets;
    public float xOffset;
    public float yOffset;
    public float frequency;
    public float amplitude;

    private void Update() //UPDATE FOR EDIT / START FOR PLAY
    { 
        //reset
        tilemap.ClearAllTiles();
        //reset
        
        //generating offsets
        Random.InitState(seed);
        octaveOffsets = new Vector2[octaveNumber];
        for (int i = 0; i < octaveNumber; i++)
        {
            xOffset = Random.Range(-100000f, 100000f) + manualOffset.x;
            yOffset = Random.Range(-100000f, 100000f) + manualOffset.y;
            octaveOffsets[i] = new Vector2(xOffset, yOffset);
        }
        //generating offsets
            
        //generating noise map
        noiseMap = new float[mapWidth,mapLength];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                frequency = 1;
                amplitude = 1;
                noise = 0;
                    
                for (int i = 0; i < octaveNumber; i++)
                {
                    float xValue = x / noiseScale * frequency + octaveOffsets[i].x;
                    float yValue = y / noiseScale * frequency + octaveOffsets[i].y;

                    if (alternateAlgorithm)
                    {
                        perlinValue = Mathf.PerlinNoise(xValue, yValue) * 2 - 1;
                    }

                    else
                    {
                        perlinValue = Mathf.PerlinNoise(xValue, yValue);
                    }
                    
                    noise += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (alternateAlgorithm)
                {
                    if (x == 0 && y == 0)
                    {
                        minNoise = noise;
                        maxNoise = noise;
                    }

                    else
                    {
                        if (noise < minNoise)
                        {
                            minNoise = noise;
                        }
                        
                        else if (noise > maxNoise)
                        {
                            maxNoise = noise;
                        }
                    }
                    noiseMap[x, y] = noise;
                }

                else
                {
                    noiseMap[x, y] = Mathf.Clamp(noise, 0f, 1f);
                }
            }
        }

        if (alternateAlgorithm)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapLength; y++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y]);
                }
            }
        }
        //generating noise map
            
        //generating textures
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                if (noiseMap[x,y] > 0 && noiseMap[x,y] <= layerOneLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), layerOne);
                }
                else if (noiseMap[x,y] > layerOneLimit && noiseMap[x,y] <= layerTwoLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), layerTwo);
                }
                
                else if (noiseMap[x,y] > layerTwoLimit && noiseMap[x,y] <= layerThreeLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), layerThree);
                }
                else if (noiseMap[x,y] > layerThreeLimit && noiseMap[x,y] <= layerFourLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), layerFour);
                }
                else if (noiseMap[x,y] > layerFourLimit && noiseMap[x,y] <= layerFiveLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), layerFive);
                }
            }
        }
        //generating textures
    }
}
