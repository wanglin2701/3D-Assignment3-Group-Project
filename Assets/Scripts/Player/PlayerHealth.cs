using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image overlay; // the reddy frame
    public float duration; // how long the frame stays fully opaque
    public float fadeSpeed; // how quickly the frame will fade

    private float durationTimer; // timer to check againts the duration

    
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Check if health is 0 and transition to Game Over scene
        if (health <= 0)
        {
            GameOver();
        }

        if (health >= 100)
        {
            SoundManager.instance.StopSound("HealthRecover");
        }
    
        if(overlay.color.a > 0)
        {
            
            // if the health drop below 30, the red frame stays!
            if(health < 30)
            {
                return;
            }
            durationTimer += Time.deltaTime; // track how long the overlay has been visible
            if(durationTimer > duration) // if longer than certain time, it fades.
            {
                //fade the image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed; // make overlay transparent
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
       //Debug.Log(health);
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction) // Checks if the back health bar (delayed visual) is greater than the health fraction.
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if(fillF < hFraction) // Checks if the front health bar is less than the health fraction.
        {
            backHealthBar.color = Color.green; 
            backHealthBar.fillAmount = hFraction; // back go first
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        SoundManager.instance.PlaySound("PlayerHurt");
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)
    {
        SoundManager.instance.StopSound("OpenDoor");
        health += healAmount;
        SoundManager.instance.PlaySound("HealthRecover");
        lerpTimer = 0f;
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Transitioning to Game Over Scene.");
        SceneManager.LoadScene("GameOver"); // Load the Game Over scene
    }
}
