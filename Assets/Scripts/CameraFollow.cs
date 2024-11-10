using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // The player object to follow
    public Vector3 offset;         // Offset position relative to the player
    public float smoothSpeed = 0.125f; // Smooth transition speed

    void LateUpdate()
    {
        // Desired position based on player's position and offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera position
        transform.position = smoothedPosition;
    }
}
