using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab
    public Transform gunBarrel;     // Gun barrel position
    public float fireRate = 0.5f;   // Time between shots

    private float nextFireTime = 0f;

    [Header("Bullet UI Settings")]
    public int maxBullets = 10;     // Maximum bullets available to shoot
    private int currentBullets;     // Number of bullets left to shoot

    public UnityEngine.UI.Image[] bulletUI;  // UI Images representing bullets
    public GameObject gunObject;   // The gun object to disable when no bullets are left
    public bool canShoot = false;  // Flag to check if the player can shoot (activated by power-up)

    void Start()
    {
        // Initialize bullets
        currentBullets = maxBullets;
        UpdateBulletUI();

        // Ensure the gun is disabled at the start
        if (gunObject != null)
        {
            gunObject.SetActive(false);  // Disable gun at start
        }

        // Hide the bullet UI initially
        SetBulletUIActive(false);
    }

    void Update()
    {
        // Check if the player can shoot (i.e., gunObject is enabled and power-up has been collected)
        if (canShoot && Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Instantiate a new bullet at the gun barrel's position and rotation
        Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);

        // Decrease bullet count and update the UI
        currentBullets--;
        UpdateBulletUI();

        // If all bullets are used, disable the gun
        if (currentBullets == 0)
        {
            gunObject.SetActive(false);  // Disable the gun if no bullets are left
        }
    }

    void UpdateBulletUI()
    {
        // Update bullet UI to reflect the current number of bullets
        for (int i = 0; i < bulletUI.Length; i++)
        {
            if (i < currentBullets)
            {
                bulletUI[i].enabled = true;  // Show the bullet icon
            }
            else
            {
                bulletUI[i].enabled = false; // Hide the bullet icon
            }
        }
    }

    // Call this method to enable the bullet UI once the gun is acquired
    public void SetBulletUIActive(bool isActive)
    {
        // Enable or disable the bullet UI based on whether the gun is acquired
        foreach (var bullet in bulletUI)
        {
            bullet.enabled = isActive;
        }
    }

    // Call this method when the player acquires the gun
    public void AcquireGun()
    {
        // Enable the gun and allow shooting
        if (gunObject != null)
        {
            gunObject.SetActive(true);  // Enable the gun
        }

        canShoot = true;  // Allow shooting
        SetBulletUIActive(true);  // Show the bullet UI immediately upon gun acquisition
    }
}
