using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    //TODO feature: auto chunk number depending on map size - few, normal, many
    //TODO feature: auto resource number depending on chunk size - few, normal, many
    
    /* WHAT THIS SCRIPT DO
     * creates square chunks in a designated area
     * number and size of the chunks are configurable
     * prevents overlapping, distance is configurable
     * 
     * creates resources in chunks
     * prevents overlapping, distance is configurable
     * number of resources in a chunk are configurable
     */
    
    /* NOTES AND WARNINGS
     * chunkRange means distance from chunk center. 2 tiles from the center means 4x4 sized chunk
     * chunkDistance/resourceDistance is minimum distance between chunk centers, must include chunkRange
     * topRightX/Y means the top right coordinates of the designated area, must exclude chunkRange
     * center of the ground/spawn area must be 0,0
     */
    
    [Header("Debug")]
    public bool isDebug;
    public GameObject chunk;
    
    [Header("Assign")]
    public GameObject resource;
    public int chunkRange;
    public int topRightX;
    public int topRightY;

    [Header("Auto Control")]
    public bool isAutoControl;
    public int resourceAmount14;
    public int chunkAmount14;
    
    [Header("Manual Control")]
    public int chunkDistance;
    public int resourceDistance;
    public int minChunkNumber;
    public int maxChunkNumber;
    public int minResourceNumber;
    public int maxResourceNumber;

    [Header("Variables - Don't Touch")]
    public int chunkNumber;
    public int resourceNumber;
    public List<Vector2> suitableChunkPointList;
    public List<Vector2> suitableResourcePointList;
    public List<Vector2> toDeletedList;

    [Header("The numbers Mason, what do they mean?")]
    public int selectedPoint;
    public Vector2 chunkPosition;
    public Vector2 resourcePosition;
    
    private void OnDrawGizmosSelected()
    {
        if (isDebug)
        {
            foreach (Vector2 point in suitableChunkPointList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(point, 0.5f);
            }
        }
    }

    private void AutoGen()
    {
        
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = -topRightX; i < topRightX; i++)
            {
                for (int j = -topRightY; j < topRightY; j++)
                {
                    suitableChunkPointList.Add(new Vector2(i,j));
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            chunkNumber = Random.Range(minChunkNumber, maxChunkNumber + 1);
            for (int i = 0; i < chunkNumber; i++)
            {
                if (suitableChunkPointList.Count == 0)
                {
                    break;
                }
                
                selectedPoint = Random.Range(0, suitableChunkPointList.Count);
                chunkPosition = suitableChunkPointList[selectedPoint];
                Instantiate(chunk, chunkPosition, quaternion.identity);
                
                foreach (Vector2 point in suitableChunkPointList)
                {
                    if (math.abs(point.x - chunkPosition.x) < chunkDistance && math.abs(point.y - chunkPosition.y) < chunkDistance)
                    {
                        toDeletedList.Add(point);
                    }
                }

                foreach (Vector2 point in toDeletedList)
                {
                    suitableChunkPointList.Remove(point);
                }
                toDeletedList.Clear();

                suitableResourcePointList.Clear();
                for (float j = chunkPosition.x - chunkRange + 0.5f; j <= chunkPosition.x + chunkRange - 0.5f; j++)
                {
                    for (float k = chunkPosition.y - chunkRange + 0.5f; k <= chunkPosition.y + chunkRange - 0.5f; k++)
                    {
                        suitableResourcePointList.Add(new Vector2(j,k));
                    }
                }
                
                resourceNumber = Random.Range(minResourceNumber, maxResourceNumber);
                for (int j = 0; j < resourceNumber; j++)
                {
                    if (suitableResourcePointList.Count == 0)
                    {
                        break;
                    }
                    
                    selectedPoint = Random.Range(0, suitableResourcePointList.Count);
                    resourcePosition = suitableResourcePointList[selectedPoint];
                    Instantiate(resource, resourcePosition, quaternion.identity);
                
                    foreach (Vector2 point in suitableResourcePointList)
                    {
                        if (math.abs(point.x - resourcePosition.x) < resourceDistance && math.abs(point.y - resourcePosition.y) < resourceDistance)
                        {
                            toDeletedList.Add(point);
                        }
                    }
                
                    foreach (Vector2 point in toDeletedList)
                    {
                        suitableResourcePointList.Remove(point);
                    }
                    toDeletedList.Clear();
                }
            }
        }
    }
}
