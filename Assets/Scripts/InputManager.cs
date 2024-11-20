using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    public bool isSprinting;
    public bool isCrouching;
    public bool isWalking;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        onFoot.Jump.performed += ctx => motor.Jump();
        look = GetComponent<PlayerLook>();
        onFoot.SprintStart.performed += x => SprintPressed();
        onFoot.SprintFinish.performed += x => SprintRelease();
        onFoot.CrouchStart.performed += z => CrouchPressed();
        onFoot.CrouchFinished.performed += z => CrouchReleased();
        onFoot.Movement.performed += a => WalkPressed();
        onFoot.MovementStop.performed += a => WalkReleased();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tell the playermotor to mvoe using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void SprintPressed()
    {
        isSprinting = true;
    }

    private void SprintRelease()
    {
        isSprinting = false;
    }

    private void CrouchPressed()
    {
        isCrouching = true;
    }

    private void CrouchReleased()
    {
        isCrouching = false;
    }

    private void WalkPressed()
    {
        isWalking = true;
        SoundManager.instance.PlaySound("Walking");
    }
    
    private void WalkReleased()
    {
        isWalking = false;
        SoundManager.instance.StopSound("Walking");
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
