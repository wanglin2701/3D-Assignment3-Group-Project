using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null && playerInventory.AllCoinsCollected)
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
