using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class AlternateGeneration : MonoBehaviour
{
    /* WHAT THIS SCRIPT DO
     * chooses a random tile and randomly expands from that tile
     * has an expansion chance which is decreasing everytime
     * when expansion stops, a new biome starts from a random tile
     *
     * biomes sizes are configurable
     */
    
    /* PSEUDOCODE
     * get biomes
     * get map size
     * get spread chance and it's decrease amount
     * get biome size limits
     * get seed
     * create suitableTileList and toPaintList
     * create toAddList because C# doesn't allow to change a list in a foreach loop that is using that list
     * 
     * A - foreach biome in the biomeList
     * choose a random tile from suitableTileList and add it to toPaintList
     * delete selected tile from suitableTileList
     * 
     * B - foreach tile in the toPaintList
     * paint the tile
     * select a random int between 0-100
     * if that number is smaller than spread chance, (which means spreading)
     * -> add tile's suitable neighbors to toAddList
     * -> remove them from suitableTileList
     * -> decrease spread chance and go B
     * else, go to B (which means not spreading)
     * B loop ending
     * 
     * clear toPaintList and add tiles from toAddList to toPaintList
     * if toPaintList is empty, go to A
     * else, go to B
     * A loops ending
     */
    
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public List<Tile> biomeList;

    [Header("Assign - Information")]
    public int mapWidth;
    public int mapLength;
    public int seed;
    public int minBiomeSize;
    public int maxBiomeSize;
    
    [Header("Assign - Multipliers")]
    public float spreadChanceDefault;
    public float spreadChanceDecreaseAmount;
    
    [Header("Variables - Don't Touch")]
    public float spreadChance;
    public Vector2Int startingTile;
    public List<Vector2Int> suitableTileList;
    public List<Vector2Int> toPaintList;
    public List<Vector2Int> toAddList;

    [Header("Map Data - Don't Touch")]
    public List<Dictionary<Vector2Int, Tile>> mapDataList;
    public List<Vector2Int> currentBiomList;

    private void OnDrawGizmos()
    {
        foreach (Vector2Int item in suitableTileList)
        {
            Gizmos.DrawWireSphere(new Vector3(item.x + 0.5f,item.y + 0.5f), 0.5f);
        }
    }

    private void Start()
    {
        //scanning the map
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                suitableTileList.Add(new Vector2Int(x, y));
            }
        }
        //scanning the map
        
        //generating biomes
        Random.InitState(seed);
        foreach (Tile biomTile in biomeList) 
        {
            if (suitableTileList.Count == 0)
            {
                break;
            }

            spreadChance = spreadChanceDefault;
            startingTile = suitableTileList[Random.Range(0, suitableTileList.Count + 1)];
            toPaintList.Add(startingTile);
            suitableTileList.Remove(startingTile);
            
            while (toPaintList.Count != 0)
            {
                foreach (Vector2Int item in toPaintList)
                {
                    Debug.Log("foric acid");
                    tilemap.SetTile(new Vector3Int(item.x, item.y), biomTile);
                    
                    if (Random.Range(0, 101) <= spreadChance)
                    {
                        spreadChance -= spreadChanceDecreaseAmount;
                        if (suitableTileList.Contains(item + Vector2Int.up))
                        {
                            toAddList.Add(item + Vector2Int.up);
                            suitableTileList.Remove(item + Vector2Int.up);
                        }
                        if (suitableTileList.Contains(item + Vector2Int.down))
                        {
                            toAddList.Add(item + Vector2Int.down);;
                            suitableTileList.Remove(item + Vector2Int.down);
                        }
                        if (suitableTileList.Contains(item + Vector2Int.left))
                        {
                            toAddList.Add(item + Vector2Int.left);;
                            suitableTileList.Remove(item + Vector2Int.left);
                        }
                        if (suitableTileList.Contains(item + Vector2Int.right))
                        {
                            toAddList.Add(item + Vector2Int.right);;
                            suitableTileList.Remove(item + Vector2Int.right);
                        }
                    }
                }
                toPaintList.Clear();
            
                foreach (Vector2Int item in toAddList)
                {
                    toPaintList.Add(item);
                }
                toAddList.Clear();
            }
        }
        //generating biomes
    }
}
