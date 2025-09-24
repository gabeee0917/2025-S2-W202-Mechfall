using System;
using UnityEngine;
using UnityEngine.InputSystem;
// Remove after real player movment is completed

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;

    private InputSystem_Actions playerControls;
    private InputAction playerMovement;
    private Vector2 moveDirection = Vector2.zero;

    private Boolean isright = true;

    public Boolean getIsRight()
    {

        return isright;
    }

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

        playerMovement = playerControls.Player.Move;
    }

    private void OnEnable()
    {
        playerMovement.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
    }

    private void Update()
    {
        moveDirection = playerMovement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        if (isright && moveDirection.x < 0)
        {
            FlipCharacter();
        }
        if (!isright && moveDirection.x > 0)
        {
            FlipCharacter();
        }
    }
    
    void FlipCharacter()
    {
        isright = !isright;
        transform.Rotate(0, 180, 0);
    }
}
