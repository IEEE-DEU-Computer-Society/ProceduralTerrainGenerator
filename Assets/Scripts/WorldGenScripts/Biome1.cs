using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Biome1 : MonoBehaviour
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
    
    [Header("Variables - Don't Touch")]
    public Dictionary<Vector2Int, Tile> mapDataDictionary;
    public List<Vector2Int> suitableTileList;
    public List<Vector2Int> currentBiomeList;
    public Vector2Int startingTile;
    public Tile biomeTile;
    public float spreadChance;
    public int counter;

    [Header("Blank Fill Variables - Don't Touch")]
    public List<Vector2Int> blankList;
    public Tile encounteredTile;

    private void Start()
    {
        //initializing dictionary
        mapDataDictionary = new Dictionary<Vector2Int, Tile>();
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
            biomeTile = biomeList[Random.Range(0, biomeList.Count)];
            spreadChance = spreadChanceDefault;
            counter = 0;
            
            startingTile = suitableTileList[Random.Range(0, suitableTileList.Count)];
            currentBiomeList.Add(startingTile);
            mapDataDictionary.Add(startingTile, biomeTile);
            suitableTileList.Remove(startingTile);

            while (currentBiomeList.Count < maxBiomeSize && counter < currentBiomeList.Count)
            {
                if (Random.Range(0, 101) <= spreadChance)
                {
                    spreadChance -= spreadChanceDecreaseAmount;
                }

                else
                {
                    break;
                }

                if (!blankList.Contains(currentBiomeList[counter] + Vector2Int.up) && 
                    !mapDataDictionary.ContainsKey(currentBiomeList[counter] + Vector2Int.up) && Random.Range(0, 2) == 0)
                {
                    currentBiomeList.Add(currentBiomeList[counter] + Vector2Int.up);
                    mapDataDictionary.Add(currentBiomeList[counter] + Vector2Int.up, biomeTile);
                    suitableTileList.Remove(currentBiomeList[counter] + Vector2Int.up);
                }
                if (!blankList.Contains(currentBiomeList[counter] + Vector2Int.down) && 
                    !mapDataDictionary.ContainsKey(currentBiomeList[counter] + Vector2Int.down) && Random.Range(0, 2) == 0)
                {
                    currentBiomeList.Add(currentBiomeList[counter] + Vector2Int.down);
                    mapDataDictionary.Add(currentBiomeList[counter] + Vector2Int.down, biomeTile);
                    suitableTileList.Remove(currentBiomeList[counter] + Vector2Int.down);
                }
                if (!blankList.Contains(currentBiomeList[counter] + Vector2Int.right) && 
                    !mapDataDictionary.ContainsKey(currentBiomeList[counter] + Vector2Int.right) && Random.Range(0, 2) == 0)
                {
                    currentBiomeList.Add(currentBiomeList[counter] + Vector2Int.right);
                    mapDataDictionary.Add(currentBiomeList[counter] + Vector2Int.right, biomeTile);
                    suitableTileList.Remove(currentBiomeList[counter] + Vector2Int.right);
                }
                if (!blankList.Contains(currentBiomeList[counter] + Vector2Int.left) && 
                    !mapDataDictionary.ContainsKey(currentBiomeList[counter] + Vector2Int.left) && Random.Range(0, 2) == 0)
                {
                    currentBiomeList.Add(currentBiomeList[counter] + Vector2Int.left);
                    mapDataDictionary.Add(currentBiomeList[counter] + Vector2Int.left, biomeTile);
                    suitableTileList.Remove(currentBiomeList[counter] + Vector2Int.left);
                }
                counter++;
            }
            
            if (currentBiomeList.Count < minBiomeSize)
            {
                foreach (Vector2Int item in currentBiomeList)
                {
                    mapDataDictionary.Remove(item);
                    blankList.Add(item);
                }
            }
            currentBiomeList.Clear();
        }
        //generating biomes

        //filling the blanks
        foreach (Vector2Int item in blankList)
        {
            int increase = 1;
            while (true)
            {
                if (mapDataDictionary.ContainsKey(new Vector2Int(item.x + increase, item.y)))
                {
                    encounteredTile = mapDataDictionary[new Vector2Int(item.x + increase, item.y)];
                    mapDataDictionary.Add(item, encounteredTile);
                    break;
                }
                if (mapDataDictionary.ContainsKey(new Vector2Int(item.x - increase, item.y)))
                {
                    encounteredTile = mapDataDictionary[new Vector2Int(item.x - increase, item.y)];
                    mapDataDictionary.Add(item, encounteredTile);
                    break;
                }
                if (mapDataDictionary.ContainsKey(new Vector2Int(item.x, item.y + increase)))
                {
                    encounteredTile = mapDataDictionary[new Vector2Int(item.x, item.y + increase)];
                    mapDataDictionary.Add(item, encounteredTile);
                    break;
                }
                if (mapDataDictionary.ContainsKey(new Vector2Int(item.x, item.y - increase)))
                {
                    encounteredTile = mapDataDictionary[new Vector2Int(item.x, item.y - increase)];
                    mapDataDictionary.Add(item, encounteredTile);
                    break;
                }
                increase++;
            }
        }
        blankList.Clear();
        //filling the blanks
        
        //generating textures
        foreach (var item in mapDataDictionary)
        {
            tilemap.SetTile(new Vector3Int(item.Key.x, item.Key.y), item.Value);
        }
        //generating textures
    }
}
