using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCoins { get; private set; }

    public UnityEvent<PlayerInventory> OnCoinsCollected;

    public int TotalCoins = 20;

    [Header("Debug")]
    public bool cheatAllCoinsCollected = false; // Toggle this to cheat and collect all coins

    // Check if all coins are collected
    public bool AllCoinsCollected => cheatAllCoinsCollected || NumberOfCoins >= TotalCoins;

    public void CoinsCollected()
    {
        if (!cheatAllCoinsCollected) // Only increment if not cheating
        {
            NumberOfCoins++;
            OnCoinsCollected.Invoke(this);
        }
    }

    private void Update()
    {
        // Keep the number of coins in sync with the cheat toggle
        if (cheatAllCoinsCollected)
        {
            NumberOfCoins = TotalCoins;
        }
    }
}
