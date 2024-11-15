using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    public GameObject playerGun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerGun.GetComponent<Animator>().Play("AimDown");
        }
        if(Input.GetMouseButtonUp(1))
        {
            playerGun.GetComponent<Animator>().Play("New State");
        }
    }
}
