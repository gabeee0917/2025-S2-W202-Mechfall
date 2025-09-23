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
    public static bool DashPressed;
    public static bool pausePressed;
    public static bool interactPressed;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction dashAction;
    private InputAction pauseAction;
    private InputAction interactAction;

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions["Move"];
        jumpAction = PlayerInput.actions["Jump"];
        runAction = PlayerInput.actions["Run"];
        dashAction = PlayerInput.actions["Dash"];
        pauseAction = PlayerInput.actions["Pause"];
        interactAction = PlayerInput.actions["Interact"];
    }

    void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();

        jumpPressed = jumpAction.WasPressedThisFrame();
        jumpHeld = jumpAction.IsPressed();
        jumpReleased = jumpAction.WasReleasedThisFrame();
        DashPressed = dashAction.WasPressedThisFrame();
        RunHeld = runAction.IsPressed();

        pausePressed = pauseAction.WasPressedThisFrame();
        interactPressed = interactAction.WasPressedThisFrame();
    }
}
