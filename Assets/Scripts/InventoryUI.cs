using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI EnemyText;
    public EscapeTrigger escapeTrigger;

    public int enemyKillCount = 0;    // Counter for killed enemies
    
    void Start()
    {
        coinsText = GetComponent<TextMeshProUGUI>();
        EnemyText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCoinsText(PlayerInventory playerInventory)
    {
        coinsText.text = playerInventory.NumberOfCoins.ToString();
    }

    public void UpdateEnemyCount()
    {
        Debug.Log("killed" + enemyKillCount);
        // Increment the kill count and update the enemy text
        enemyKillCount++;
        EnemyText.text = $"{enemyKillCount}";
        if (enemyKillCount >= 3)
        {
            escapeTrigger.CompleteKillTask();
        }
        else
        {
            
        }

    }

    public int GetEnemyKillCount()
    {
        return enemyKillCount;
    }
}
