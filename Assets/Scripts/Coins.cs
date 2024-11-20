using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [Header("Floating Animation Settings")]
    public float floatAmplitude = 0.5f; // The height of the floating effect
    public float floatSpeed = 1f;       // The speed of the floating effect
    public float spinSpeed = 50f;       // The speed of the spinning effect

    private Vector3 startPos;           // Original position of the coin

    private void Start()
    {
        // Record the starting position of the coin
        startPos = transform.position;
    }

    private void Update()
    {
        // Apply floating effect (up and down movement)
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Apply spinning effect (rotate around Y-axis)
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.CoinsCollected();
            gameObject.SetActive(false);
        }
    }
}
