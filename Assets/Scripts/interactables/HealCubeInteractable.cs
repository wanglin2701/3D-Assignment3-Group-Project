using UnityEngine;

public class HealCubeInteractable : Interactable
{
    [SerializeField] private float healAmount = 20f; // Set the heal amount for the cube

    protected override void Interact()
    {
        // Find the PlayerHealth script and restore health
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(healAmount);
        }

        // Destroy the heal cube after interaction
        Destroy(gameObject);
    }
}
