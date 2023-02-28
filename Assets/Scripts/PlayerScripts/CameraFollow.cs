using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //assign
    public GameObject player;
    
    //variables
    public Vector3 newPosition;
    public float cameraDelay = 5f;
    
    void Update()
    {
        newPosition = new Vector3(player.transform.position.x,player.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraDelay * Time.deltaTime);
    }
}
