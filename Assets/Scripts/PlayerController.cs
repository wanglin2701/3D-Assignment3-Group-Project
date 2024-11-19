using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;         // Speed for forward movement
    public float horizontalSpeed = 5f; // Speed for left/right movement
    public float jumpForce = 5f;      // Jump force
    private bool isGrounded = true;   // Check if player is on the ground

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Forward movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal"); // A = -1, D = 1
        Vector3 horizontalMove = new Vector3(horizontalInput * horizontalSpeed * Time.deltaTime, 0, 0);
        transform.Translate(horizontalMove);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Player is mid-air
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if player lands back on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
