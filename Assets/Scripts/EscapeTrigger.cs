using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeTrigger : MonoBehaviour
{
    private Collider escapeCollider;
    private bool allCoinsCollected = false;

    private void Start()
    {
        escapeCollider = GetComponent<Collider>();
        escapeCollider.isTrigger = false; 
    }

    private void Update()
    {
        // Ensure the player has collected all coins
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();

            if (playerInventory != null && playerInventory.AllCoinsCollected)
            {
                allCoinsCollected = true; // Mark all coins as collected
                escapeCollider.isTrigger = true; 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the collision is with the Player
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

            if (playerInventory != null && allCoinsCollected)
            {
                Debug.Log("All coins collected! Level Complete!");
                SceneManager.LoadScene("LevelComplete");
            }
            else
            {
                Debug.Log("You need to collect all coins before escaping!");
            }
        }
    }
}
