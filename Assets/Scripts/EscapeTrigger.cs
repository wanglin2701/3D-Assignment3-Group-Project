using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeTrigger : MonoBehaviour
{
    private Collider escapeCollider;

    public bool isAllCoinsCollected = false;  // Check if all coins are collected
    public bool isThreeEnemiesDefeated = false;  // Check if 3 enemies are defeated
    public bool isExitConditionMet = false;  // Check if exit condition is met

    [SerializeField] private GameObject promptText;  // Prompt for collecting coins
    [SerializeField] private GameObject exitPromptText;  // Prompt for finding the exit
    [SerializeField] private GameObject promptTextToKillEnemy;  // Prompt for killing enemies
    [SerializeField] private PlayerInventory playerInventory;

    void Start()
    {
        escapeCollider = GetComponent<Collider>();
        escapeCollider.isTrigger = false;

        // Ensure prompts are initially hidden
        if (promptText != null) promptText.SetActive(false);
        if (exitPromptText != null) exitPromptText.SetActive(false);
        if (promptTextToKillEnemy != null) promptTextToKillEnemy.SetActive(false);
    }

    void Update()
    {
        // Check if all conditions are met
        if (isAllCoinsCollected && isThreeEnemiesDefeated && !isExitConditionMet)
        {
            isExitConditionMet = true;
            Debug.Log("All conditions met! Exit is now enabled.");
            ShowExitPrompt();
            escapeCollider.isTrigger = true;  // Enable escape collider
        }
        CompleteCoinTask();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isExitConditionMet)
            {
                Debug.Log("Player collided with escape trigger. Proceeding to Level Complete scene.");
                SoundManager.instance.PlaySound("LevelComplete");
                SceneManager.LoadScene("LevelComplete");  // Load next scene
            }
            else
            {
                // Show prompts if conditions are not met
                if (!isAllCoinsCollected) ShowPrompt();  // Prompt to collect coins
                if (!isThreeEnemiesDefeated) ShowKillEnemiesPrompt();  // Prompt to kill enemies
            }
        }
    }

    private void ShowPrompt()
    {
        if (promptText != null) promptText.SetActive(true);
        StartCoroutine(HidePromptAfterDelay(2f));
    }

    private void ShowKillEnemiesPrompt()
    {
        if (promptTextToKillEnemy != null) promptTextToKillEnemy.SetActive(true);
        StartCoroutine(HidePromptAfterDelay(2f));
    }

    private void ShowExitPrompt()
    {
        if (exitPromptText != null) exitPromptText.SetActive(true);
        StartCoroutine(HidePromptAfterDelay(3f));
    }

    private IEnumerator HidePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (promptText != null) promptText.SetActive(false);
        if (promptTextToKillEnemy != null) promptTextToKillEnemy.SetActive(false);
        if (exitPromptText != null) exitPromptText.SetActive(false);
    }

    private void CompleteCoinTask()
    {
        Debug.Log("coins: " + playerInventory.NumberOfCoins);
        if (playerInventory.NumberOfCoins == 20)
        {
            isAllCoinsCollected = true;
        }
    }

    public void CompleteKillTask()
    {
        isThreeEnemiesDefeated = true;
    }
}
