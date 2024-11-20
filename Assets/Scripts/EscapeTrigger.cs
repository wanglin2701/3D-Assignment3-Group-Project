using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeTrigger : MonoBehaviour
{
    private Collider escapeCollider;
    private bool allCoinsCollected = false;

    [SerializeField] private GameObject promptText; // Reference to the prompt UI
    [SerializeField] private GameObject exitPromptText; // Prompt for finding the exit
    private Coroutine hidePromptCoroutine;

    private void Start()
    {
        escapeCollider = GetComponent<Collider>();
        escapeCollider.isTrigger = false;

        // Ensure all prompts are initially hidden
        if (promptText != null)
        {
            promptText.SetActive(false);
        }
        if (exitPromptText != null)
        {
            exitPromptText.SetActive(false);
        }
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();

            if (playerInventory != null && playerInventory.AllCoinsCollected)
            {
                if (!allCoinsCollected) // Trigger the prompt only once
                {
                    ShowExitPrompt();
                }
                allCoinsCollected = true;
                escapeCollider.isTrigger = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (allCoinsCollected)
            {
                Debug.Log("All coins collected! Level Complete!");
                SoundManager.instance.PlaySound("LevelComplete");
                SceneManager.LoadScene("LevelComplete");
            }
            else
            {
                Debug.Log("Not all coins collected! Showing prompt.");
                ShowPrompt();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the escape collider. Starting hide prompt timer.");
            if (hidePromptCoroutine != null)
            {
                StopCoroutine(hidePromptCoroutine);
            }
            hidePromptCoroutine = StartCoroutine(HidePromptAfterDelay(1f)); // Hide prompt after 1 second
        }
    }

    private void ShowPrompt()
    {
        if (promptText != null)
        {
            promptText.SetActive(true);
        }
    }

    private void ShowExitPrompt()
    {
        if (exitPromptText != null)
        {
            exitPromptText.SetActive(true);
            StartCoroutine(HideExitPromptAfterDelay(3f)); // Display the prompt for 3 seconds
        }
    }

    private IEnumerator HidePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (promptText != null)
        {
            promptText.SetActive(false);
        }
    }

    private IEnumerator HideExitPromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (exitPromptText != null)
        {
            exitPromptText.SetActive(false);
        }
    }
}
