using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    public static bool jumpPressed;
    public static bool jumpHeld;
    public static bool jumpReleased;
    public static bool RunHeld;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
    }

    void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        jumpPressed = _jumpAction.WasPressedThisFrame();
        jumpHeld = _jumpAction.IsPressed();
        jumpReleased = _jumpAction.WasReleasedThisFrame();

        RunHeld = _runAction.IsPressed();
    }
}
