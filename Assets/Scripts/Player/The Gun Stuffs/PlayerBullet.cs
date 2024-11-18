using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 20f;      // Speed of the bullet
    public float damage = 10f;     // Damage dealt by the bullet
    public float lifetime = 5f;    // Lifetime of the bullet before destruction

    void Start()
    {
        // Destroy the bullet after the lifetime expires
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Debugging log to check if the bullet hit something
        Debug.Log($"hit something");

        // Check if the object has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"enemy hit");
            // Try to get the EnemyHealth component from the collided object
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Deal damage to the enemy
                enemyHealth.TakeDamage(damage);
            }
        }

        // Destroy the bullet upon collision
        Destroy(gameObject);
    }
}


