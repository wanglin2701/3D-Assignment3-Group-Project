using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public float swayAmount = 0.05f; // Amount of sway
    public float swaySpeed = 2f;     // Speed of sway

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation; // Save the initial camera rotation
    }

    void Update()
    {
        float swayX = Input.GetAxis("Mouse X") * swayAmount;
        float swayY = Input.GetAxis("Mouse Y") * swayAmount;

        // Calculate the sway rotation
        Quaternion targetRotation = originalRotation * Quaternion.Euler(-swayY, swayX, 0);

        // Smoothly interpolate to the target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swaySpeed);
    }
}

