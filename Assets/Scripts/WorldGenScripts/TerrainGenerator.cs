using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    /*
     * scale make it zoom in the right top corner, can be made center by calculating the center of the map 
     */
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public Tile blue;
    public Tile green;
    public Tile gray;
    public Tile white;

    [Header("Assign - Texture Ranges")] 
    public Vector2 blueRange;
    public Vector2 greenRange;
    public Vector2 grayRange;
    public Vector2 whiteRange;
    
    [Header("Assign - Information")]
    public int mapWidth;
    public int mapLength;
    public int noiseScale;
    public int octaveNumber;
    public int seed;
    
    [Header("Assign - Multipliers")]
    public int lacunarity; //
    public int persistence; //
    public Vector2 manualOffset;

    [Header("Don't Touch")]
    public float noise;
    public float[,] noiseMap;
    public float minNoise;
    public float maxNoise;
    public Vector2[] octaveOffsets;
    public float xOffset;
    public float yOffset;
    
    public int frequency;
    public int amplitude;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //generating offsets
            Random.InitState(seed);
            octaveOffsets = new Vector2[octaveNumber]; //*every offset same?p1
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
                    frequency = 1; //*different freq?
                    amplitude = 1; //*different amp?
                    noise = 0;
                    
                    for (int i = 0; i < octaveNumber; i++)
                    {
                        float xValue = x / noiseScale * frequency + octaveOffsets[i].x; //*every offset same?p2
                        float yValue = y / noiseScale * frequency + octaveOffsets[i].y; //*every offset same?p3

                        float perlinValue = Mathf.PerlinNoise(xValue, yValue) * 2 - 1; //*0-1?
                        noise += perlinValue + amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (x == 0 && y == 0) //*not 0-1? beginning
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
                    noiseMap[x, y] = noise; //*exclude this part
                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapLength; y++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y]);
                }
            } //*not 0-1? ending
            //generating noise map
            
            //generating textures
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapLength; y++)
                {
                    if (noiseMap[x,y] >= blueRange.x && noiseMap[x,y] < blueRange.y)
                    {
                        tilemap.SetTile(new Vector3Int(x,y,0), blue);
                    }
                    
                    else if (noiseMap[x,y] >= greenRange.x && noiseMap[x,y] < greenRange.y)
                    {
                        tilemap.SetTile(new Vector3Int(x,y,0), green);
                    }
                    
                    else if (noiseMap[x,y] >= grayRange.x && noiseMap[x,y] < grayRange.y)
                    {
                        tilemap.SetTile(new Vector3Int(x,y,0), gray);
                    }
                    
                    else if (noiseMap[x,y] >= whiteRange.x && noiseMap[x,y] < whiteRange.y)
                    {
                        tilemap.SetTile(new Vector3Int(x,y,0), white);
                    }
                }
            }
            //generating textures
        }
    }
}
