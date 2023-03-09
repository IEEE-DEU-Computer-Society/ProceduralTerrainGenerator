using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    /*
     * scale make it zoom in the right top corner, can be made center by calculating the center of the map 
     */
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public Tile blue;
    public Tile red;
    public Tile green;
    public Tile pink;
    public Tile gray;
    public Tile purple;
    public Tile white;
    public Tile black;
    public Tile brown;

    [Header("Assign - Texture Ranges")]
    [Range(0f ,1f)] public float blueLimit;
    [Range(0f, 1f)] public float redLimit;
    [Range(0f, 1f)] public float greenLimit;
    [Range(0f, 1f)] public float pinkLimit;
    [Range(0f, 1f)] public float grayLimit;
    [Range(0f, 1f)] public float purpleLimit;
    [Range(0f, 1f)] public float whiteLimit;
    [Range(0f, 1f)] public float blackLimit;
    [Range(0f, 1f)] public float brownLimit;
    
    [Header("Assign - Information")]
    public int mapWidth;
    public int mapLength;
    public int seed;
    public int octaveNumber;
    
    [Header("Assign - Multipliers")]
    public Vector2 manualOffset;
    public float noiseScale;
    [Range(0f, 1f)] public float lacunarity;
    [Range(0f, 1f)] public float persistence;

    [Header("Don't Touch")]
    public float noise;
    public float[,] noiseMap;
    public float minNoise;
    public float maxNoise;
    public Vector2[] octaveOffsets;
    public float xOffset;
    public float yOffset;
    public float frequency;
    public float amplitude;

    private void Update()
    { 
        //reset
        tilemap.ClearAllTiles();
        //reset
        
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
                frequency = 1;
                amplitude = 1;
                noise = 0;
                    
                for (int i = 0; i < octaveNumber; i++)
                {
                    float xValue = x / noiseScale * frequency + octaveOffsets[i].x; //*every offset same?p2
                    float yValue = y / noiseScale * frequency + octaveOffsets[i].y; //*every offset same?p3

                    float perlinValue = Mathf.PerlinNoise(xValue, yValue)  * 2 - 1 ; //*0-1?
                    noise += perlinValue * amplitude;

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
                noiseMap[x, y] = noise; //*exclude this line
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
                if (noiseMap[x,y] >= 0 && noiseMap[x,y] <= blueLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), blue);
                }
                
                else if (noiseMap[x,y] > blueLimit && noiseMap[x,y] <= redLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), red);
                }
                    
                else if (noiseMap[x,y] > redLimit && noiseMap[x,y] <= greenLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), green);
                }
                
                else if (noiseMap[x,y] > greenLimit && noiseMap[x,y] <= pinkLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), pink);
                }
                    
                else if (noiseMap[x,y] > pinkLimit && noiseMap[x,y] <= grayLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), gray);
                }
                
                else if (noiseMap[x,y] > grayLimit && noiseMap[x,y] <= purpleLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), purple);
                }
                    
                else if (noiseMap[x,y] > purpleLimit && noiseMap[x,y] <= whiteLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), white);
                }
                
                else if (noiseMap[x,y] > whiteLimit && noiseMap[x,y] <= blackLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), black);
                }
                
                else if (noiseMap[x,y] > blackLimit && noiseMap[x,y] <= brownLimit)
                {
                    tilemap.SetTile(new Vector3Int(x,y,0), brown);
                }
            }
        }
        //generating textures
    }
}
