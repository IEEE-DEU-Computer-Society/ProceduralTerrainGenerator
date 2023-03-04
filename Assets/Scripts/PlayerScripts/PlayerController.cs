using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //assign
    public Rigidbody2D rb;
    
    //movement variables
    public Vector2 moveInput;
    public Vector2 speed = new Vector2(15f,15f);
    
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = moveInput * speed;
    }
}