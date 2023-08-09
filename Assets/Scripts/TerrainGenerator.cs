using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class PerlinTerrainGenerator : PerlinGeneratorBase
{
    [Header("Assign - Tiles")]
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private List<Tile> terrainTileList;
    [SerializeField] private List<float> terrainTileRangeList;

    [Header("Assign - Noise Map")]
    [SerializeField] private int seed;
    [SerializeField] private int octaveNumber = 3;
    [SerializeField] private float noiseScale;
    [SerializeField] private Vector2Int offset;
    [SerializeField] private float lacunarity;
    [SerializeField] [Range(0f, 1f)] private float persistence;

    private Dictionary<Vector2Int, float> terrainNoiseMap;

    private void Awake()
    {
        Random.InitState(seed);
    }

    private void Update()
    {
        terrainNoiseMap = GenerateNoiseMap(offset, octaveNumber, noiseScale, persistence, lacunarity);
        GenerateTiles(terrainTilemap, terrainNoiseMap, terrainTileList, terrainTileRangeList);
    }
}