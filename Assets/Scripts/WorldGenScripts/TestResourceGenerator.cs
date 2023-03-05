using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestResourceGenerator : MonoBehaviour
{
    //TODO fix: resources are not aligned to tiles and the ones on the edges are overflowing
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
    //
    public List<Vector2> nonSuitablePointsList;
    //
    
    [Header("Assign")]
    public GameObject resource;
    public int chunkRange;
    public int topRightX;
    public int topRightY;

    [Header("Auto Control")]
    public bool isAutoControl;
    public int resourceAmount13;
    public int chunkAmount13;
    
    [Header("Manual Control")]
    public int chunkDistance;
    public int resourceDistance;
    public int minChunkNumber;
    public int maxChunkNumber;
    public int minResourceNumber;
    public int maxResourceNumber;

    [Header("The numbers Mason, what do they mean?")]
    public int chunkNumber;
    public int resourceNumber;
    public List<Vector2> chunkList;
    public List<Vector2> resourceList;

    [Header("Other")]
    public Vector2 chunkPosition;
    public Vector2 resourcePosition;
    public bool isSuitable;
    //
    public int unSuitablePointNumber;
    //
    
    private void OnDrawGizmosSelected()
    {
        if (isDebug)
        {
            Gizmos.color = Color.red;
            
            //
            foreach (var center in nonSuitablePointsList)
            {
                Gizmos.DrawWireSphere(center,0.5f);
            }
            //
        }
    }
    
    private bool AreThereSuitablePosition(int x, int y, int distance, List<Vector2> list)
    {
        bool check = true;
        unSuitablePointNumber = 0;
        for (int i = -x; i <= x ; i++)
        {
            for (int j = -y; j <= y; j++)
            {
                foreach (Vector2 position in list)
                {
                    if (math.abs(position.x - i) < distance && math.abs(position.y - j) < distance)
                    {
                        unSuitablePointNumber++;
                        nonSuitablePointsList.Add(new Vector2(i,j));
                        break;
                    }
                }
            }
        }

        if ((x * 2 + 1) * (y * 2 + 1) == unSuitablePointNumber)
        {
            check = false;
        }

        return check;
    }

    private void AreaCheck(int x, int y, int distance, List<Vector2> list)
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            chunkNumber = Random.Range(minChunkNumber, maxChunkNumber + 1);
            for (int i = 0; i < chunkNumber; i++)
            {
                if (AreThereSuitablePosition(topRightX, topRightY, chunkDistance, chunkList))
                {
                    do
                    {
                        isSuitable = true;
                        chunkPosition = new Vector2(Random.Range(-topRightX, topRightX + 1),
                            Random.Range(-topRightY, topRightY + 1));

                        foreach (Vector2 position in chunkList)
                        {
                            if (math.abs(position.x - chunkPosition.x) < chunkDistance &&
                                math.abs(position.y - chunkPosition.y) < chunkDistance)
                            {
                                isSuitable = false;
                                break;
                            }
                        }
                    } while (!isSuitable);

                    if (isSuitable)
                    {
                        if (isDebug)
                        {
                            Instantiate(chunk, chunkPosition, quaternion.identity);
                        }

                        chunkList.Add(chunkPosition);
                    }

                    resourceNumber = Random.Range(minResourceNumber, maxResourceNumber + 1);
                    for (int j = 0; j < resourceNumber; j++)
                    {
                        do
                        {
                            isSuitable = true;
                            resourcePosition.x = Random.Range((int)chunkPosition.x + chunkRange,
                                (int)chunkPosition.x - chunkRange);
                            resourcePosition.y = Random.Range((int)chunkPosition.y + chunkRange,
                                (int)chunkPosition.y - chunkRange);

                            foreach (Vector2 position in resourceList)
                            {
                                if (math.abs(position.x - resourcePosition.x) < resourceDistance &&
                                    math.abs(position.y - resourcePosition.y) < resourceDistance)
                                {
                                    isSuitable = false;
                                    break;
                                }
                            }
                        } while (!isSuitable);

                        if (isSuitable)
                        {
                            Instantiate(resource, resourcePosition, quaternion.identity);
                            resourceList.Add(resourcePosition);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
