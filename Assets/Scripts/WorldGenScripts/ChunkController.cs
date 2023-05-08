using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [Header("Assign")] 
    public Transform player;
    public PerlinBiomeGenerator biomeGenerator;
    public PerlinTerrainGenerator terrainGenerator;
    public PerlinFloraGenerator floraGenerator;
    public PerlinResourceGenerator resourceGenerator;

    [Header("Variables")]
    public Vector2 playerPosition;
    public Vector2Int playerChunk;
    
    //CHUNK DATA - Inspector doesn't support dictionaries
    public List<Vector2Int> chunkData;

    private void Update()
    {
        //find player chunk
        playerPosition = player.position;
        playerChunk = new Vector2Int((int)(playerPosition.x / 64), (int)(playerPosition.y / 64));
        //find player chunk

        if (!chunkData.Contains(playerChunk))
        {
            biomeGenerator.GenerateBiome(new Vector2Int(playerChunk.x * 64, playerChunk.y * 64));
            terrainGenerator.GenerateTerrain(new Vector2Int(playerChunk.x * 64, playerChunk.y * 64));
            chunkData.Add(playerChunk);
        }
    }
}
