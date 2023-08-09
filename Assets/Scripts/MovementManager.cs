using System;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static event Action OnMovement;

    [Header("Assign")]
    [SerializeField] private int defaultMoveSpeed = 1;
    [SerializeField] private int fastMoveSpeed = 4;
    [SerializeField] private float zoomSpeed = 1f;

    private int previousScrollDelta;
    private int moveSpeed;

    private void Update()
    {
        HandleZoom();
        HandleFastMode();
        HandleInput();
    }

    private void HandleZoom()
    {
        if (previousScrollDelta != (int)Input.mouseScrollDelta.y)
        {
            TerrainGenerator.Singleton.noiseScale += Input.mouseScrollDelta.y * zoomSpeed;
            OnMovement?.Invoke();
        }

        previousScrollDelta = (int)Input.mouseScrollDelta.y;
    }
    
    private void HandleFastMode()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) moveSpeed = fastMoveSpeed;
        else moveSpeed = defaultMoveSpeed;
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            TerrainGenerator.Singleton.offset.y += moveSpeed;
            OnMovement?.Invoke();
        }
        
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            TerrainGenerator.Singleton.offset.y -= moveSpeed;
            OnMovement?.Invoke();
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            TerrainGenerator.Singleton.offset.x += moveSpeed;
            OnMovement?.Invoke();
        }
        
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            TerrainGenerator.Singleton.offset.x -= moveSpeed;
            OnMovement?.Invoke();
        }
    }
}
