using UnityEngine;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float damageTimer;

    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Canvas healthUICanvas; // Assign the Canvas containing the health bar in the Inspector.
    public float hideUIAfterSeconds = 2f; // Time before hiding the UI.

    void Start()
    {
        health = maxHealth;
        healthUICanvas.enabled = false; // Initially, hide the health UI.
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Hide the health UI after not taking damage for a certain duration.
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0)
            {
                healthUICanvas.enabled = false;
            }
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
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete; // Accelerate over time.
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;

        // Show the health UI and reset the hide timer.
        healthUICanvas.enabled = true;
        damageTimer = hideUIAfterSeconds;
    }
}
