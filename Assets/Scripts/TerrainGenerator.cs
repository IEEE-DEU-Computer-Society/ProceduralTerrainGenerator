using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TerrainGenerator : PerlinGeneratorBase
{
    public static TerrainGenerator Singleton;
    
    [Header("Assign - Tiles")]
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private List<Tile> terrainTileList;
    [SerializeField] private List<float> terrainTileRangeList;

    [Header("Assign - Noise Map")]
    public int octaveNumber = 3;
    public float noiseScale;
    public Vector2Int offset;
    public float lacunarity;
    [Range(0f, 1f)] public float persistence;

    private Dictionary<Vector2Int, float> terrainNoiseMap;
    private Vector2 randomize;

    private void Awake()
    {
        Singleton = GetComponent<TerrainGenerator>();
        ChangeSeed(0);
        
        MovementManager.OnMovement += GenerateTerrain;
        UIManager.OnUIChange += GenerateTerrain;
    }

    private void GenerateTerrain()
    {
        terrainNoiseMap = GenerateNoiseMap(offset, randomize,octaveNumber, noiseScale, persistence, lacunarity);
        GenerateTiles(terrainTilemap, terrainNoiseMap, terrainTileList, terrainTileRangeList);
    }

    public void ChangeSeed(int newSeed)
    {
        Random.InitState(newSeed);
        randomize = new Vector2(Random.Range(-100000f, 100000f), Random.Range(-100000f, 100000f));
    }
}