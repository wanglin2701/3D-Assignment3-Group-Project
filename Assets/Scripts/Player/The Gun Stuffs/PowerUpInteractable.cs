using UnityEngine;

public class PowerUpInteractable : Interactable
{
    public GameObject gunObject;    // The gun object that will be enabled after power-up collection
    public PlayerShoot playerShoot; // Reference to the PlayerShoot component

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
}


