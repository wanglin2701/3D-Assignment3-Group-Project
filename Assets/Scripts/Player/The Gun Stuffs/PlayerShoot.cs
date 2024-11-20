using UnityEngine;
using System.Collections;

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

    [Header("Camera Settings")]
    public Camera playerCamera;     // Reference to the player's camera
    public float zoomFOV = 40f;     // Field of View when zoomed in
    public float normalFOV = 60f;   // Default Field of View
    public float zoomSpeed = 10f;   // Speed at which the camera zooms in/out

    [Header("Damage Crosshair Settings")]
    public GameObject damageCrosshair;


    // Track aiming state
    [SerializeField]
    private bool isAiming = false;
    private bool isShooting = false;

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

        if (damageCrosshair != null)
        {
            damageCrosshair.SetActive(false);
        }
    }

    void Update()
    {
        // Check aiming state
        if (Input.GetMouseButton(1)) // Right mouse button for aiming
        {
            isAiming = true;
            float targetFOV = isAiming ? zoomFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        // Set animator parameters based on states
        if (gunAnimator != null)
        {
            // Update the "isRightMouseHolding" parameter in the animator
            //if(isAiming) gunAnimator.SetTrigger("Test");
            gunAnimator.SetBool("isRightMouseHolding", isAiming);
        }

        // Check if the player can shoot
        if (canShoot && Input.GetMouseButton(0) && Time.time >= nextFireTime && currentBullets > 0)
        {
            Shoot();
            
        }
    }

    void Shoot()
    {
        SoundManager.instance.PlaySound("PlayerAttack");
        // Set the next fire time to enforce the fire rate cooldown
        nextFireTime = Time.time + fireRate;

        // Update the "isShooting" parameter in the animator
        if (gunAnimator != null)
        {
            isShooting = true;
            gunAnimator.SetTrigger("isShooting");

            // Play the appropriate recoil animation if needed
            if (isAiming)
            {
        
            }
            else
            {
                // Play Recoil animation if not aiming
                gunAnimator.CrossFade("Recoil", 0.1f);  // Use CrossFade for smooth transitions
            }

            // Reset the isShooting flag after a small delay to avoid sticking to the shooting animation
            StartCoroutine(ResetShootingFlag());
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

    IEnumerator ResetShootingFlag()
    {
        // Wait for the recoil animation to finish
        yield return new WaitForSeconds(0.3f);  // Adjust this time to match the length of your recoil animation
        isShooting = false;
        gunAnimator.SetBool("isShooting", false);
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
        SoundManager.instance.PlaySound("WeaponInteract");
        canShoot = true;                // Allow shooting
        currentBullets = maxBullets;    // Reset bullet count
        UpdateBulletUI();               // Update UI to reflect full bullets
        SetBulletUIActive(true);        // Show bullet UI

        // Ensure the correct animation state when acquiring the gun
        if (gunAnimator != null)
        {
            isAiming = false; 
        }
    }

    public void ResetGun()
    {
        canShoot = false;               // Disable shooting
        currentBullets = maxBullets;    // Reset bullets
        UpdateBulletUI();               // Update UI
    }

     public void ShowDamageCrosshair()
    {
        // This method will be called from the PlayerBullet script to show the crosshair
        if (damageCrosshair != null)
        {
            damageCrosshair.SetActive(true);
            StartCoroutine(HideDamageCrosshair());
        }
    }

    private IEnumerator HideDamageCrosshair()
    {
        // Hide the damage crosshair after 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        damageCrosshair.SetActive(false);
    }
}
