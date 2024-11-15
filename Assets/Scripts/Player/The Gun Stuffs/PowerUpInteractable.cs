using UnityEngine;

public class PowerUpInteractable : Interactable
{
    public GameObject gunObject;    // The gun object that will be enabled after power-up collection
    public PlayerShoot playerShoot; // Reference to the PlayerShoot component

    protected override void Interact()
    {
        base.Interact();

        // Enable the gun object for the player once the power-up is collected
        if (gunObject != null)
        {
            gunObject.SetActive(true);  // Enable the gun object

            // Call the method to enable shooting and the bullet UI
            if (playerShoot != null)
            {
                playerShoot.AcquireGun();  // Enable the ability to shoot and show bullet UI
            }
        }

        // Optionally destroy or deactivate the power-up after interaction
        Destroy(gameObject);  // Destroy the power-up object
    }
}
