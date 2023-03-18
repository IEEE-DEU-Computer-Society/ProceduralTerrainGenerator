using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public class BuggyController : MonoBehaviour
{

    //assign
    public Rigidbody2D rbbuggy;

    //movement variables
    public Vector2 moveInput;
    public Vector2 speed = new Vector2(15f,15f);

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rbbuggy.velocity = moveInput * speed;
    }
}