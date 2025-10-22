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

    public GameObject swordHitBox;
    public Transform swordPoint;

    private InputAction playerShootAction;
    private InputAction playerSwordAction;
    private Boolean isFiring = true;
    private Boolean gunCD = true;
    public Animator animator;
    private void Awake()
    {
        playerControls = new InputSystem_Actions();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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

        // Subscribe to the player attack action
        playerSwordAction = playerControls.Player.Attack;
        playerSwordAction.Enable();
        playerSwordAction.performed += OnPlayerSword;
    }

    public void OnDisable()
    {
        // Unsubscibe from the actions making sure to stop listening whne thye are uneeded
        playerShootAction.started -= OnPlayerShoot;
        playerShootAction.canceled -= OnPlayerShootStop;
        playerShootAction.Disable();

        // Unsubscribe from the sword action
        playerSwordAction.performed -= OnPlayerSword;
        playerSwordAction.Disable();
    }

    private void OnPlayerSword(InputAction.CallbackContext context)
    {
        // Create sword at player
        Instantiate(swordHitBox, swordPoint.position, swordPoint.rotation);
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
        animator.SetTrigger("Shoot");
        // Create a bullet firing from the firepoint;
        Instantiate(bullet, firepoint.position, firepoint.rotation);
        SoundManager.instance.PlayShoot();
    }

    void CanShoot()
    {
        gunCD = true;
    }
}
