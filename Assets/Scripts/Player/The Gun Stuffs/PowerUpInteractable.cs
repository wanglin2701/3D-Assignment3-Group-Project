using UnityEngine;

public class PowerUpInteractable : Interactable
{
    public GameObject gunObject;     // The gun object that will be enabled after power-up collection
    public PlayerShoot playerShoot; // Reference to the PlayerShoot component

    [Header("Floating and Spinning Settings")]
    public float floatAmplitude = 0.5f;   // Height of the floating effect
    public float floatSpeed = 1f;         // Speed of the floating effect
    public float spinSpeed = 50f;         // Speed of the spinning effect

    private Vector3 startPosition;

    private void Start()
    {
        // Save the initial position of the power-up
        startPosition = transform.position;
    }

    private void Update()
    {
        // Apply floating effect
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPosition + new Vector3(0f, floatOffset, 0f);

        // Apply spinning effect
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }

    protected override void Interact()
    {
        base.Interact();

        // Ensure the PlayerShoot state is reset when picking up the new collectable
        if (playerShoot != null)
        {
            playerShoot.ResetGun();      // Reset the gun state
            playerShoot.AcquireGun();    // Re-acquire the gun with a full bullet count
        }

        // Enable the gun object for the player
        if (gunObject != null)
        {
            gunObject.SetActive(true);   // Enable the gun object
        }

        // Optionally destroy or deactivate the power-up after interaction
        Destroy(gameObject);             // Destroy the power-up object
    }

    public void GetGunModel()
    {
        gunObject.SetActive(true);
    }
}
