using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    //assign
    public GameObject resource;
    
    //variables
    public Vector2 firstPoint;
    public float chunkSize = 3f;
    public Vector2 spawnPoint1;
    public Vector2 spawnPoint2;
    
    public float spawnDistance = 1.5f;
    public float minX, minY, maxX, maxY;
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            firstPoint = new Vector2(0f + Random.Range(-17f, 17f),0f + Random.Range(-8f, 8f));
        }
        
        if (Input.GetKey(KeyCode.K))
        {
            spawnPoint1 = firstPoint + new Vector2(Random.Range(-chunkSize, chunkSize),
                Random.Range(-chunkSize, chunkSize));
        }

        minX = spawnPoint1.x - spawnDistance;
        maxX = spawnPoint1.x + spawnDistance;

        minY = spawnPoint1.y - spawnDistance;
        maxY = spawnPoint1.y + spawnDistance;

        if (Input.GetKey(KeyCode.L))
        {
            do
            {
                spawnPoint2 = firstPoint + new Vector2(Random.Range(-chunkSize, chunkSize),
                    Random.Range(-chunkSize, chunkSize));
                
            } while (spawnPoint2.x >= minX && spawnPoint2.x <= maxX && spawnPoint2.y >= minY && spawnPoint2.y <= maxY);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(firstPoint, new Vector3(chunkSize * 2,chunkSize * 2));
        Gizmos.DrawSphere(spawnPoint1,0.5f);
        Gizmos.DrawSphere(spawnPoint2, 0.5f);
    }
}
