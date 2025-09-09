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
    private Boolean gunCD = true;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    public void OnEnable()
    {
        // Subscribe to the player shoot action
        playerShootAction = playerControls.Player.Player_Shoot;
        playerShootAction.Enable();
        // Get when the player shoot button is pressed down
        playerShootAction.started += OnPlayerShoot;
        // Get when the player shoot action is released
        playerShootAction.canceled += OnPlayerShootStop;
    }

    public void OnDisable()
    {
        // Unsubscibe from the actions making sure to stop listening whne thye are uneeded
        playerShootAction.started -= OnPlayerShoot;
        playerShootAction.canceled -= OnPlayerShootStop;
        playerShootAction.Disable();
    }

    private void OnPlayerShoot(InputAction.CallbackContext context)
    {
        // Check if the gun is on cooldown
        if (gunCD)
        {
            // Set the gun to fireing and start invoking the shoot action
            isFiring = true;
            InvokeRepeating(nameof(Fire), 0f, gunCDT);
            
            // Make the gun abke to shoot after the cooldown
            gunCD = false;
            Invoke(nameof(CanShoot), gunCDT);
        }
    }

    private void OnPlayerShootStop(InputAction.CallbackContext context)
    {
        // On release of the shoot key cancel the invoke of the shoot action
        isFiring = false;
        CancelInvoke(nameof(Fire));
    }

    void Fire()
    {
        // Create a bullet firing from the firepoint;
        Instantiate(bullet, firepoint.position, firepoint.rotation);

    }

    void CanShoot()
    {
        gunCD = true;
    }
}
