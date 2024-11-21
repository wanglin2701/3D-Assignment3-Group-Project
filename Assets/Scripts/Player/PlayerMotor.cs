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
    private Transform cameraTransform; // Reference to the camera transform for swaying

    public float crouchHeight = 1f;    // Height of the character when crouching
    private float originalHeight;      // Original height of the character
    public float heightTransitionSpeed = 8f; // Speed of height transition

    private float targetHeight;        // Target height for smooth transitions

    [Header("Character movement camera animation")]
    // Sway settings for idle
    public float swayAmount = 0.1f;    // Amount of sway during idle
    public float swaySpeed = 5f;       // Speed of sway while idle

    // Sway settings for walking and sprinting
    public float walkSwayAmount = 0.1f; // Amount of sway during walking
    public float walkSwaySpeed = 5f;    // Speed of sway while walking
    public float sprintSwayAmountMultiplier = 2f; // Multiplier for sway amount when sprinting
    public float sprintSwaySpeedMultiplier = 1.5f;  // Multiplier for sway speed when sprinting

    [Header("Gun Animation")]
    public Animator gunAnimator;      // Reference to the gun's Animator

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>(); // Get the InputManager reference
        cameraTransform = Camera.main.transform; // Assign the main camera's transform
        currentSpeed = speed;                        // Initialize the speed to walking speed
        originalHeight = controller.height;          // Store the original height of the player
        targetHeight = originalHeight;               // Set initial target height

        // Set initial camera height to 1.2
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, 1.2f, cameraTransform.localPosition.z);
    }

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
            Debug.Log("A");     
            currentSpeed = sprintSpeed;
            targetHeight = originalHeight; // Set target height to stand

            // Trigger the gun bounce animation
            if (gunAnimator != null)
            {
                gunAnimator.SetBool("isSprinting", true);
            }
        }
        else
        {
            currentSpeed = speed;
            targetHeight = originalHeight; // Set target height to stand

            // Stop the gun bounce animation
            if (gunAnimator != null)
            {
                gunAnimator.SetBool("isSprinting", false);
            }
        }

        // Smoothly transition to the target height
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * heightTransitionSpeed);

        // Call the camera sway function
        ApplyCameraSway();
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
            SoundManager.instance.PlaySound("Jumping");
        }
    }

    // Function to apply camera sway effect
    private void ApplyCameraSway()
    {
        // Determine the sway intensity based on whether sprinting or walking
        float swayIntensity = 0f;
        float swaySpeedMultiplier = 1f;

        // Handle the sway for sprinting
        if (inputManager.isSprinting)
        {
            swayIntensity = walkSwayAmount * sprintSwayAmountMultiplier;
            swaySpeedMultiplier = sprintSwaySpeedMultiplier;
        }
        // Handle the sway for walking
        else if (inputManager.isWalking)
        {
            swayIntensity = walkSwayAmount;
            swaySpeedMultiplier = walkSwaySpeed; // Use the walk sway speed directly
        }
        // Handle the idle sway (when not moving)
        else
        {
            swayIntensity = swayAmount;
            swaySpeedMultiplier = 1f;
        }

        // Calculate the sway based on the player's movement speed and direction
        // Apply sway using sine for X and cosine for Y to simulate a sway effect
        float swayX = Mathf.Sin(Time.time * swaySpeedMultiplier) * swayIntensity;
        float swayY = Mathf.Cos(Time.time * swaySpeedMultiplier) * swayIntensity;

        // Apply the sway to the camera's position (you can tweak this to make the sway effect stronger or weaker)
        cameraTransform.localPosition = new Vector3(swayX, 1.2f + swayY, cameraTransform.localPosition.z);
    }
}
