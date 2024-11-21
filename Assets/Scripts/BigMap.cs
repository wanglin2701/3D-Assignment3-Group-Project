using UnityEngine;

public class BigMap : MonoBehaviour
{
    public GameObject bigMap; // Reference to the big map GameObject
    public Transform player;  // Reference to the player
    private bool isBigMapActive = false; // Tracks whether the big map is active

    void Start()
    {
        bigMap.SetActive(false); // Ensure the big map starts hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleBigMap();
        }
    }

    void LateUpdate()
    {
        if (isBigMapActive)
        {
            // Ensure the big map is static and doesn't follow or rotate with the player
            Vector3 newPosition = new Vector3(0, transform.position.y, 0); // Adjust position as needed
            transform.position = newPosition;
        }
    }

    private void ToggleBigMap()
    {
        isBigMapActive = !isBigMapActive; // Toggle the state
        bigMap.SetActive(isBigMapActive); // Show or hide the big map based on its state
    }
}
