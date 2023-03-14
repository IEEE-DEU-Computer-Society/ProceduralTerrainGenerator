using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BiomeGenerator : MonoBehaviour
{
    //TODO SOLUTION: TOO SLOW FOR SIZES ABOVE 200X200. 100x100 IDEAL AND FAST

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
    
    [Header("Blank Fill Variables - Don't Touch")]
    public List<Vector2Int> currentTilesList;
    public List<Vector2Int> blankList;
    public Dictionary<Vector2Int, Tile> mapDataList;
    public Tile encounteredTile;
    
    [Header("Variables - Don't Touch")]
    public List<Vector2Int> suitableTileList;
    public Tile biomTile;
    public Vector2Int startingTile;
    public float spreadChance;
    public List<Vector2Int> toPaintList;
    public List<Vector2Int> toAddList;
   
    private void Start()
    {
        //initializing dictionary
        mapDataList = new Dictionary<Vector2Int, Tile>();
        //initializing dictionary
        
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
        while (suitableTileList.Count != 0)
        {
            biomTile = biomeList[Random.Range(0, biomeList.Count)];
            spreadChance = spreadChanceDefault;
            startingTile = suitableTileList[Random.Range(0, suitableTileList.Count)];
            toPaintList.Add(startingTile);
            suitableTileList.Remove(startingTile);
        
            while (toPaintList.Count != 0)
            {
                Debug.Log("log1");
                foreach (Vector2Int item in toPaintList)
                {
                    Debug.Log("log2");
                    mapDataList.Add(item, biomTile);
                    currentTilesList.Add(item);
                
                    if (Random.Range(0, 101) <= spreadChance)
                    {
                        Debug.Log("log3");
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
                            toAddList.Add(item + Vector2Int.left);
                            suitableTileList.Remove(item + Vector2Int.left);
                        }
                        if (suitableTileList.Contains(item + Vector2Int.right))
                        {
                            toAddList.Add(item + Vector2Int.right);
                            suitableTileList.Remove(item + Vector2Int.right);
                        }
                    }
                }
                toPaintList.Clear();

                if (currentTilesList.Count > maxBiomeSize)
                {
                    Debug.Log("log4");
                    foreach (Vector2Int item in toAddList)
                    {
                        Debug.Log("log5");
                        suitableTileList.Add(item);
                    }
                    toAddList.Clear();
                    break;
                }
        
                foreach (Vector2Int item in toAddList)
                {
                    Debug.Log("log6");
                    toPaintList.Add(item);
                }
                toAddList.Clear();
            }
            
            if (currentTilesList.Count < minBiomeSize)
            {
                foreach (Vector2Int item in currentTilesList)
                {
                    Debug.Log("log7");
                    mapDataList.Remove(item);
                    blankList.Add(item);
                }
            }
            currentTilesList.Clear();
        }
        //generating biomes

        //filling the blanks
        foreach (Vector2Int item in blankList)
        {
            int increase = 1;
            while (true)
            {
                Debug.Log("log8");
                if (mapDataList.ContainsKey(new Vector2Int(item.x + increase, item.y)))
                {
                    encounteredTile = mapDataList[new Vector2Int(item.x + increase, item.y)];
                    mapDataList.Add(item,encounteredTile);
                    break;
                }
                if (mapDataList.ContainsKey(new Vector2Int(item.x - increase, item.y)))
                {
                    encounteredTile = mapDataList[new Vector2Int(item.x - increase, item.y)];
                    mapDataList.Add(item,encounteredTile);
                    break;
                }
                if (mapDataList.ContainsKey(new Vector2Int(item.x, item.y + increase)))
                {
                    encounteredTile = mapDataList[new Vector2Int(item.x, item.y + increase)];
                    mapDataList.Add(item,encounteredTile);
                    break;
                }
                if (mapDataList.ContainsKey(new Vector2Int(item.x, item.y - increase)))
                {
                    encounteredTile = mapDataList[new Vector2Int(item.x, item.y - increase)];
                    mapDataList.Add(item,encounteredTile);
                    break;
                }
                increase++;
            }
        }
        blankList.Clear();
        //filling the blanks
        
        //generating textures
        foreach (var item in mapDataList)
        {
            Debug.Log("log9");
            tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), item.Value);
        }
        //generating textures
    }
}
