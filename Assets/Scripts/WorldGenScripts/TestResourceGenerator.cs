using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestResourceGenerator : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug;
    public GameObject chunk;

    [Header("Assign")]
    public GameObject resource;
    public int topRightX = 20;
    public int topRightY = 20;
    public int minChunkNumber = 1;
    public int maxChunkNumber = 1;
    public int chunkRange = 2;
    public int chunkDistance = 5;
    public int minResourceNumber = 3;
    public int maxResourceNumber = 3;
    public int resourceDistance = 2;

    [Header("Variables")]
    public int chunkNumber;
    public int resourceNumber;
    public Vector2 chunkPosition;
    public Vector2 resourcePosition;
    public List<Vector2> chunkList;
    public List<Vector2> resourceList;
    public bool isSuitable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
}
