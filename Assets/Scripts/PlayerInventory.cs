using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    
    public int NumberOfCoins { get; private set; }

    public UnityEvent<PlayerInventory> OnCoinsCollected;

    public int TotalCoins = 20;
    public bool AllCoinsCollected => NumberOfCoins >= TotalCoins; // Check if all coins are collected
    
    public void CoinsCollected()
    {
        NumberOfCoins++;
        OnCoinsCollected.Invoke(this);
    }
}
