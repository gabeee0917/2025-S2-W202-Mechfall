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
    private Boolean gunCD = false;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    public void OnEnable()
    {
        playerShootAction = playerControls.Player.Player_Shoot;
        playerShootAction.Enable();
        playerShootAction.performed += OnPlayerShoot; // Subscribe to event
    }

    public void OnDisable()
    {
        playerShootAction.performed -= OnPlayerShoot; // Unsubscribe
        playerShootAction.Disable();
    }

    private void OnPlayerShoot(InputAction.CallbackContext context)
    {
        if (!gunCD)
        {
            Instantiate(bullet, firepoint.position, firepoint.rotation);
            Invoke("ResetGunCD", gunCDT);
            gunCD = true;
        }
    }

    void ResetGunCD()
    {
        gunCD = false;
    }
}
