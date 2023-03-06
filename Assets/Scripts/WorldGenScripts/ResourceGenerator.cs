using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    //TODO make the probabilities of the resources selectable from the inspector
    //TODO terrain recognition
    
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
    public List<GameObject> resourcePrefabs;
    public int chunkRange;
    public int topRightX;
    public int topRightY;
    
    [Header("Assign")]
    public int chunkDistance;
    public int resourceDistance;
    public int minChunkNumber;
    public int maxChunkNumber;
    public int minResourceNumber;
    public int maxResourceNumber;

    [Header("The numbers Mason, what do they mean? - Don't Touch")]
    public GameObject resource;
    public int resourceCode;
    public int chunkNumber;
    public int resourceNumber;
    public List<Vector2> suitableChunkPointList;
    public List<Vector2> suitableResourcePointList;
    public List<Vector2> toDeletedList;
    public int selectedPoint;
    public Vector2 chunkPosition;
    public Vector2 resourcePosition;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Vector2 point in suitableChunkPointList)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))    //SCAN THE MAP FIRST AND ONCE
        {
            for (int i = -topRightX; i < topRightX; i++)
            {
                for (int j = -topRightY; j < topRightY; j++)
                {
                    suitableChunkPointList.Add(new Vector2(i,j));
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) //SPAWN RESOURCES
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
                if (isDebug)
                {
                    Instantiate(chunk, chunkPosition, quaternion.identity);
                }
                
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

                resourceCode = Random.Range(0, 10);
                if (resourceCode <= 9 && resourceCode >= 4)
                {
                    resource = resourcePrefabs[0];
                }
                
                else if(resourceCode <= 3 && resourceCode >= 1)
                {
                    resource = resourcePrefabs[1];
                }

                else
                {
                    resource = resourcePrefabs[2];
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
