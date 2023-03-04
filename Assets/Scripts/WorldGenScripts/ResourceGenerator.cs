using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    //TODO fix: when there is no suitable space to place a chunk, unity crash
    //TODO fix: resources are not aligned to tiles and the ones on the edges are overflowing

    /* WHAT THIS CODE DO
     * creates random square chunks in designated area
     * number of chunks and chunk size is configurable
     * prevents overlapping, distance is configurable
     * 
     * creates random resources in chunks
     * prevents overlapping, distance is configurable
     * resources in a chunk are configurable
     */
    
    /* NOTES
     * topRightX/Y defines the spawn area of the middle point of the chunk, must include chunkRange
     * chunkRange means distance from center. 2 tiles from the center means 4x4 sized chunk
     * chunkDistance/resourceDistance is distance between centers, must also include chunkRange
     * center of the ground must be 0,0
     * random.range for int excludes maximum value
     */

    [Header("Debug")]
    public bool isDebug;
    public GameObject chunk;

    [Header("Assign")]
    public GameObject resource;
    public int topRightX;
    public int topRightY;
    public int minChunkNumber;
    public int maxChunkNumber;
    public int chunkRange;
    public int chunkDistance;
    public int minResourceNumber;
    public int maxResourceNumber;
    public int resourceDistance;

    [Header("Variables")]
    public int chunkNumber;
    public int resourceNumber;
    public Vector2 chunkPosition;
    public Vector2 resourcePosition;
    public List<Vector2> chunkList;
    public List<Vector2> resourceList;
    public bool isSuitable;

    void Start()
    {
        chunkNumber = Random.Range(minChunkNumber, maxChunkNumber + 1);
        for (int i = 0; i < chunkNumber; i++)
        {
            do
            {
                isSuitable = true;
                chunkPosition = new Vector2(Random.Range(-topRightX, topRightX + 1), Random.Range(-topRightY, topRightY + 1));
                
                foreach (var position in chunkList)
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
                    resourcePosition.x = Random.Range((int)chunkPosition.x + chunkRange, (int)chunkPosition.x - chunkRange);
                    resourcePosition.y = Random.Range((int)chunkPosition.y + chunkRange, (int)chunkPosition.y - chunkRange);
                    
                    foreach (var position in resourceList)
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
    }
}
