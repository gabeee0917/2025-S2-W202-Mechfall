using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bullet;
    public InputSystem_Actions playerControls;
    public float gunCDT = 1f;
    private InputAction playerShootAction;
    private Boolean isFiring = true;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    public void OnEnable()
    {
        playerShootAction = playerControls.Player.Player_Shoot;
        playerShootAction.Enable();
        playerShootAction.started += OnPlayerShoot;
        playerShootAction.canceled += OnPlayerShootStop;
    }

    public void OnDisable()
    {
        playerShootAction.started -= OnPlayerShoot;
        playerShootAction.canceled -= OnPlayerShootStop;
        playerShootAction.Disable();
    }

    private void OnPlayerShoot(InputAction.CallbackContext context)
    {
        isFiring = true;
        InvokeRepeating(nameof(Fire), 0f, gunCDT);
    }
    
    private void OnPlayerShootStop(InputAction.CallbackContext context)
    {
        isFiring = false;
        CancelInvoke(nameof(Fire));
    }

    void Fire()
    {
        Instantiate(bullet, firepoint.position, firepoint.rotation);

    }
    
}
