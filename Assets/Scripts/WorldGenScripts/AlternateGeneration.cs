using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class AlternateGeneration : MonoBehaviour
{
    [Header("Assign - Textures")]
    public Tilemap tilemap;
    public List<Tile> tileList;

    [Header("Assign - Information")]
    public int mapWidth;
    public int mapLength;
    public int seed;
    public int biomeNumber;
    //public int minBiomeSize;
    //public int maxBiomeSize;
    
    [Header("Assign - Multipliers")]
    public float paintChanceDefault;
    public float paintChanceDecreaseAmount;
    public float paintChanceDecreaseChance;

    [Header("Variables - Don't Touch")]
    public float paintChance;
    public Vector2Int startingTile;
    public List<Vector2Int> suitableTileList;
    public List<Vector2Int> toPaintList;
    public List<Vector2Int> toAddList;

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
        for (int i = 0; i < biomeNumber; i++)
        {
            if (suitableTileList.Count == 0)
            {
                break;
            }

            paintChance = paintChanceDefault;
            startingTile = suitableTileList[Random.Range(0, suitableTileList.Count + 1)];
            toPaintList.Add(startingTile);
            suitableTileList.Remove(startingTile);
            
            while (toPaintList.Count != 0)
            {
                foreach (Vector2Int item in toPaintList)
                {
                    Debug.Log("forech");
                    tilemap.SetTile(new Vector3Int(item.x, item.y), tileList[i]);
                    
                    if (Random.Range(0, 101) <= paintChance)
                    {
                        paintChance -= paintChanceDecreaseAmount;
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

            foreach (Vector2Int item in toPaintList)
            {
                suitableTileList.Add(item);
            }
            toPaintList.Clear();
        }
        //generating biomes
    }
}
