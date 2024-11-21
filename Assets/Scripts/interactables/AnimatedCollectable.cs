using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCollectable : MonoBehaviour
{
    [Header("Floating Animation Settings")]
    public float floatAmplitude = 0.5f; 
    public float floatSpeed = 1f;       
    public float spinSpeed = 50f;      

    private Vector3 startPos;           

    private void Start()
    {
        
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
}
