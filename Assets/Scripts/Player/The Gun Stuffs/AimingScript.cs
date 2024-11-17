using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    public GameObject playerGun;  // The player's gun GameObject
    private Animator gunAnimator; // Animator for the gun

    // Start is called before the first frame update
    void Start()
    {
        gunAnimator = playerGun.GetComponent<Animator>(); // Get the Animator component
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Start aiming
        {
            gunAnimator.SetFloat("AimSpeed", 1); // Normal speed
            gunAnimator.Play("AimDown", -1, 0);  // Play from the start
        }
        else if (Input.GetMouseButtonUp(1)) // Reverse aiming animation
        {
            gunAnimator.SetFloat("AimSpeed", -1); // Reverse speed
            gunAnimator.Play("AimDown", -1, 1);  // Play from the end
        }
    }
}
