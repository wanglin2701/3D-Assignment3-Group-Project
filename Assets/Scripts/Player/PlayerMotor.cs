using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool isGrounded;
    public float speed = 5f;             // Normal walking speed
    public float sprintSpeed = 10f;     // Sprinting speed
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    
    private float currentSpeed;         // Tracks the player's current speed
    private InputManager inputManager;  // Reference to the InputManager

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>(); // Get the InputManager reference
        currentSpeed = speed; // Initialize the speed to walking speed
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        // Adjust speed based on whether the player is sprinting
        currentSpeed = inputManager.isSprinting ? sprintSpeed : speed;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // Move the player based on the current speed
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
