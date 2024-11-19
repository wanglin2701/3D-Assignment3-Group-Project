using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI coinsText;
    
    void Start()
    {
        coinsText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCoinsText(PlayerInventory playerInventory)
    {
        coinsText.text = playerInventory.NumberOfCoins.ToString();
    }
}
