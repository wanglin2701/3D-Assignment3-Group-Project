using UnityEngine;

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

    void Start()
    {
        health = maxHealth;
        if (healthBarCanvas != null)
        {
            healthBarCanvas.gameObject.SetActive(false); // Initially hide the health bar
        }
        LateUpdate();
        
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
        health -= damage;
        lerpTimer = 0f;

        // Show the health bar and reset the hide timer
        if (healthBarCanvas != null)
        {
            healthBarCanvas.gameObject.SetActive(true);
        }
        damageTimer = hideUIAfterSeconds;

        // Notify the enemy's state machine to transition to SearchState
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null && enemy.StateMachine != null)
        {
            enemy.LastKnowPos = enemy.Player.transform.position; // Update last known player position
            enemy.StateMachine.ChangeState(new SearchState());
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
                // Update the health bar position above the enemy's head
                healthBarCanvas.position = healthBarPosition.position;
            }
        }
    }

}
