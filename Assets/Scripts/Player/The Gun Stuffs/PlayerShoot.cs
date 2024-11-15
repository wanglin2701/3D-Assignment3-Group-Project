using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public Transform gunBarrel;    // Gun barrel position
    public float fireRate = 0.5f;  // Time between shots

    private float nextFireTime = 0f;

    [Header("Bullet UI Settings")]
    public int maxBullets = 10;  // Maximum bullets available to shoot
    private int currentBullets;   // Number of bullets left to shoot

    public UnityEngine.UI.Image[] bulletUI;  // UI Images representing bullets
    public GameObject gunObject;  // The gun object to disable when no bullets are left

    void Start()
    {
        // Initialize bullets
        currentBullets = maxBullets;
        UpdateBulletUI();
    }

    void Update()
    {
        // Shoot on left mouse button click and if enough time has passed for the next shot
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0)
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
            gunObject.SetActive(false);
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
}



