using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public Transform gunBarrel;    // Gun barrel position
    public float fireRate = 0.5f;  // Time between shots

    private float nextFireTime = 0f;

    void Update()
    {
        // Shoot on left mouse button click
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
    }
}


