using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float fixedYPosition = 22f; // Fixed Y position for the arrow
    public Vector3 arrowOffset; // Optional offset to adjust arrow's position relative to player (e.g., above the player)

    void Update()
    {
        if (player != null)
        {
            // Get the player's position
            Vector3 playerPosition = player.position;

            // Update the arrow's position to follow the player
            // Fix the Y position to the specified value
            Vector3 newPosition = new Vector3(playerPosition.x + arrowOffset.x, fixedYPosition + arrowOffset.y, playerPosition.z + arrowOffset.z);
            transform.position = newPosition;

            // Lock the X rotation to 90 degrees and keep Y and Z rotation unaffected
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Ensure arrow is always upright (X rotation fixed at 90)
        }
    }
}
