using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class MissleScript : MonoBehaviour
{
    public Transform enemy;

    private Rigidbody2D rgb;

    public float speed = 5f;

    public float rotatespeed = 200f;
    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)enemy.position - rgb.position;
        
        direction.Normalize();

        float rotateMissle = Vector3.Cross(direction, transform.up).z;

        rgb.angularVelocity = -rotateMissle * rotatespeed;
        
        rgb.velocity = transform.up * speed;
        
    }
} 
