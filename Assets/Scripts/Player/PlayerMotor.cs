using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool isGrounded;
    public float speed = 5f;            // Normal walking speed
    public float sprintSpeed = 10f;    // Sprinting speed
    public float crouchSpeed = 2f;     // Crouching speed
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    private float currentSpeed;        // Tracks the player's current speed
    private InputManager inputManager; // Reference to the InputManager

    public float crouchHeight = 1f;    // Height of the character when crouching
    private float originalHeight;      // Original height of the character
    public float heightTransitionSpeed = 8f; // Speed of height transition

    private float targetHeight;        // Target height for smooth transitions

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>(); // Get the InputManager reference
        currentSpeed = speed;                        // Initialize the speed to walking speed
        originalHeight = controller.height;          // Store the original height of the player
        targetHeight = originalHeight;               // Set initial target height
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        // Determine the player's speed and target height based on their current state
        if (inputManager.isCrouching)
        {
            currentSpeed = crouchSpeed;
            targetHeight = crouchHeight; // Set target height to crouch
        }
        else if (inputManager.isSprinting && !inputManager.isCrouching) // Ensure no conflict with crouch
        {
            currentSpeed = sprintSpeed;
            targetHeight = originalHeight; // Set target height to stand
        }
        else
        {
            currentSpeed = speed;
            targetHeight = originalHeight; // Set target height to stand
        }

        // Smoothly transition to the target height
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * heightTransitionSpeed);
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
        if (isGrounded && !inputManager.isCrouching) // Prevent jumping while crouching
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
