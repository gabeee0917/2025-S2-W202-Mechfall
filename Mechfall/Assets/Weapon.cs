using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bullet;
    public InputSystem_Actions playerControls;
    private InputAction playerShootAction;

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
        Instantiate(bullet, firepoint.position, firepoint.rotation);
    }
}
