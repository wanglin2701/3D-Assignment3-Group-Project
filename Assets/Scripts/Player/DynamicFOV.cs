using UnityEngine;

public class DynamicFOV : MonoBehaviour
{
    public Camera playerCamera;
    public InputManager inputManager; // Reference to the InputManager
    public float normalFOV = 60f;
    public float sprintFOV = 80f;
    public float smoothFactor = 10f;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Fallback to the main camera if not set in the Inspector
        }

        if (inputManager == null)
        {
            inputManager = GetComponent<InputManager>(); // Fallback to find InputManager on the same GameObject
        }
    }

    void Update()
    {
        if (inputManager == null)
        {
            Debug.LogError("InputManager is not assigned!");
            return;
        }

        // Calculate the target FOV based on sprinting state
        float targetFOV = inputManager.isSprinting ? sprintFOV : normalFOV;

        // Smoothly transition the FOV
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * smoothFactor);
    }
}
