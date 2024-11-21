using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeTrigger : MonoBehaviour
{
    private Collider escapeCollider;

    // Exposed booleans to check conditions in the Inspector
    public bool isAllCoinsCollected = false;  // To check if all coins are collected
    public bool isThreeEnemiesDefeated = false; // To check if 3 enemies are defeated
    public bool isExitConditionMet = false;  // To check if exit condition is met

    [SerializeField] private GameObject promptText; // Prompt for collecting coins
    [SerializeField] private GameObject exitPromptText; // Prompt for finding the exit
    [SerializeField] private GameObject promptTextToKillEnemy; // Prompt for killing enemies
    private Coroutine hidePromptCoroutine;

    private void Start()
    {
        escapeCollider = GetComponent<Collider>();
        escapeCollider.isTrigger = false;

        // Ensure all prompts are initially hidden
        if (promptText != null) promptText.SetActive(false);
        if (exitPromptText != null) exitPromptText.SetActive(false);
        if (promptTextToKillEnemy != null) promptTextToKillEnemy.SetActive(false);
    }

    private void Update()
    {
        // Only check for the exit condition after both coin collection and enemy kills are fulfilled
        if (isAllCoinsCollected && isThreeEnemiesDefeated && !isExitConditionMet)
        {
            isExitConditionMet = true;  // If both conditions are met, set exit condition
            ShowExitPrompt();
            escapeCollider.isTrigger = true; // Enable the escape collider for the player
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();

            // If both conditions are met (either by manual setting in the inspector or in-game), proceed to level complete
            if (isAllCoinsCollected && isThreeEnemiesDefeated)
            {
                Debug.Log("All conditions met! Level Complete!");
                SoundManager.instance.PlaySound("LevelComplete");
                SceneManager.LoadScene("LevelComplete");
            }
            else
            {
                // Show relevant prompts if conditions are not met
                if (!isAllCoinsCollected) ShowPrompt(); // Prompt to collect coins
                if (!isThreeEnemiesDefeated) ShowKillEnemiesPrompt(); // Prompt to kill enemies
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the escape collider. Starting hide prompt timer.");
            if (hidePromptCoroutine != null) StopCoroutine(hidePromptCoroutine);
            hidePromptCoroutine = StartCoroutine(HidePromptAfterDelay(1f)); // Hide prompt after 1 second
        }
    }

    private void ShowPrompt()
    {
        if (promptText != null) promptText.SetActive(true);
        StartCoroutine(HidePromptAfterDelay(2f)); // Show prompt for 2 seconds
    }

    private void ShowKillEnemiesPrompt()
    {
        if (promptTextToKillEnemy != null) promptTextToKillEnemy.SetActive(true);
        StartCoroutine(HideKillEnemiesPromptAfterDelay(2f)); // Show prompt for 2 seconds
    }

    private void ShowExitPrompt()
    {
        if (exitPromptText != null) exitPromptText.SetActive(true);
        StartCoroutine(HideExitPromptAfterDelay(3f)); // Show exit prompt for 3 seconds
    }

    private IEnumerator HidePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (promptText != null) promptText.SetActive(false);
    }

    private IEnumerator HideKillEnemiesPromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (promptTextToKillEnemy != null) promptTextToKillEnemy.SetActive(false);
    }

    private IEnumerator HideExitPromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (exitPromptText != null) exitPromptText.SetActive(false);
    }
}
