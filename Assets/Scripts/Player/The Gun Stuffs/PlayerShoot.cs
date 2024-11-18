using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab
    public Transform gunBarrel;      // Gun barrel position
    public float fireRate = 0.5f;    // Time between shots (cooldown)

    private float nextFireTime = 0f; // Tracks when the player can shoot again
    private Animator gunAnimator;   // Animator for the gun recoil

    [Header("Bullet UI Settings")]
    public int maxBullets = 10;     // Maximum bullets available to shoot
    private int currentBullets;     // Number of bullets left to shoot

    public UnityEngine.UI.Image[] bulletUI; // UI Images representing bullets
    public GameObject gunObject;    // The gun object to disable when no bullets are left
    public bool canShoot = false;   // Flag to check if the player can shoot (activated by power-up)

    // Track aiming state
    private bool isAiming = false;

    void Start()
    {
        // Initialize bullets
        currentBullets = maxBullets;
        UpdateBulletUI();

        // Ensure the gun is disabled at the start
        if (gunObject != null)
        {
            gunObject.SetActive(false); // Disable gun at start
        }

        // Hide the bullet UI initially
        SetBulletUIActive(false);

        // Get the Animator component from the gun object
        if (gunObject != null)
        {
            gunAnimator = gunObject.GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Check aiming state
        if (Input.GetMouseButton(1)) // Right mouse button for aiming
        {
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        // Check if the player can shoot
        if (canShoot && Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0)
        {
            Shoot();
        }
    }

    void Shoot()
{
    // Set the next fire time to enforce the fire rate cooldown
    nextFireTime = Time.time + fireRate;

    // Play the appropriate recoil animation
    if (gunAnimator != null)
    {
        if (isAiming)
        {
            // Ensure the Aim animation is playing, but it stays on the last frame (end keyframe)
            gunAnimator.CrossFade("AimDown", 0.1f, 0, 1f);  // Start at the last frame of the "Aim" animation

            // Play the recoil animation while aiming
            gunAnimator.Play("AimAndShootRecoil");
        }
        else
        {
            // If not aiming, play the regular recoil animation
            gunAnimator.Play("Recoil");
        }
    }

    // Instantiate a new bullet at the gun barrel's position and rotation
    Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);

    // Decrease bullet count and update the UI
    currentBullets--;
    UpdateBulletUI();

    // If all bullets are used, disable the gun
    if (currentBullets == 0)
    {
        gunObject.SetActive(false); // Disable the gun if no bullets are left
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

    public void SetBulletUIActive(bool isActive)
    {
        // Enable or disable the bullet UI based on whether the gun is acquired
        foreach (var bullet in bulletUI)
        {
            bullet.enabled = isActive;
        }
    }

    public void AcquireGun()
    {
        canShoot = true;                // Allow shooting
        currentBullets = maxBullets;    // Reset bullet count
        UpdateBulletUI();               // Update UI to reflect full bullets
        SetBulletUIActive(true);        // Show bullet UI

        // Ensure no animation is played when acquiring the gun
        if (gunAnimator != null)
        {
            gunAnimator.Play("Idle"); // Ensure the Animator starts in Idle state
        }
    }

    public void ResetGun()
    {
        canShoot = false;               // Disable shooting
        currentBullets = maxBullets;    // Reset bullets
        UpdateBulletUI();               // Update UI
    }
}
