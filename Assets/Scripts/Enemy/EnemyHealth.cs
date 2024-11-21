using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float damageTimer;

    [Header("Health Bar Settings")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Transform healthBarCanvas; // Assign the Canvas containing the health bar
    public Transform healthBarPosition; // Optional: Specify where the health bar should appear above the enemy's head
    public UnityEngine.UI.Image frontHealthBar; // Foreground of the health bar
    public UnityEngine.UI.Image backHealthBar;  // Background of the health bar
    public float hideUIAfterSeconds = 2f; // Time before hiding the health bar

    private bool isDead = false; // Flag to prevent multiple death triggers

    void Start()
    {
        health = maxHealth;
        if (healthBarCanvas != null)
        {
            healthBarCanvas.gameObject.SetActive(false); // Initially hide the health bar
        }
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Hide the health bar if the enemy hasn't received damage for a while
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0 && healthBarCanvas != null)
            {
                healthBarCanvas.gameObject.SetActive(false);
            }
        }

        // Optional: Update health bar position above the enemy
        if (healthBarCanvas != null && healthBarPosition != null)
        {
            healthBarCanvas.position = healthBarPosition.position;
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.color = Color.red;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Prevent further damage if already dead

        health -= damage;
        lerpTimer = 0f;

        // Show the health bar and reset the hide timer if the enemy is alive
        if (healthBarCanvas != null && !isDead)
        {
            healthBarCanvas.gameObject.SetActive(true);
        }
        damageTimer = hideUIAfterSeconds;

        // Notify the enemy's state machine to transition to SearchState
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null && enemy.StateMachine != null)
        {
            // Update last known position of the player (if your search state uses this)
            enemy.LastKnowPos = enemy.Player.transform.position; 
            enemy.StateMachine.ChangeState(new SearchState()); // Change to SearchState
        }

        // Check if the enemy is dead
        if (health <= 0 && !isDead)
        {
            isDead = true;
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateEnemyCount();
            }
            TriggerDeathAnimation(); // Start the death animation

            // Hide the health bar when the enemy dies
            if (healthBarCanvas != null)
            {
                healthBarCanvas.gameObject.SetActive(false); // Hide the health bar
            }
            
            // Optionally, disable the enemy's components after death (like colliders, animators, etc.)
            DisableEnemyComponents();
        }
    }

    void DisableEnemyComponents()
    {
        // Disable the collider to prevent the enemy from interacting with the player
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Optionally disable other components
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false; // Optionally disable the animator if you no longer want animations after death
        }
    }



    void LateUpdate()
    {
        if (Camera.main != null)
        {
            // Ensure the health bar canvas is always facing the camera
            healthBarCanvas.LookAt(Camera.main.transform);
            healthBarCanvas.Rotate(0, 180, 0); // Flip to face the camera if needed
            
            // Adjust position to be above the enemy's head
            if (healthBarPosition != null)
            {
                healthBarCanvas.position = healthBarPosition.position;
            }
        }
    }

    // Coroutine to handle the death animation and disappearance
    void TriggerDeathAnimation()
    {
        // Start the rotation animation
        StartCoroutine(RotateAndDisappear());
    }

    IEnumerator RotateAndDisappear()
    {
        float duration = 1f; // Set the duration of the rotation animation
        float timeElapsed = 0f;
        float startRotation = transform.eulerAngles.z;
        float endRotation = -75f;

        // Animate the rotation from 0 to -75
        while (timeElapsed < duration)
        {
            float currentRotation = Mathf.Lerp(startRotation, endRotation, timeElapsed / duration);
            transform.eulerAngles = new Vector3(0, 0, currentRotation); // Only rotate on the Z axis
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Finalize the rotation and set the Z rotation to -75
        transform.eulerAngles = new Vector3(0, 0, endRotation);

        // After animation is complete, deactivate the enemy object
        gameObject.SetActive(false); // Deactivate the enemy GameObject
        
    }
}
