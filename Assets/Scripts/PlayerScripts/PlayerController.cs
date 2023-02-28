using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //assign
    public Rigidbody2D rb;
    
    //movement variables
    public float horizontalMoveInput;
    public float verticalMoveInput;
    public float speed = 15f;
    
    void Update()
    {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
        verticalMoveInput = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(speed * horizontalMoveInput, speed * verticalMoveInput);
    }
}
