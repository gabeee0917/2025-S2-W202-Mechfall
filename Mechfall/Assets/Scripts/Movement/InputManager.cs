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
    public static bool swingPressed;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction dashAction;
    private InputAction pauseAction;
    private InputAction interactAction;
    private InputAction swingAction;

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        moveAction = PlayerInput.actions["Move"];
        jumpAction = PlayerInput.actions["Jump"];
        runAction = PlayerInput.actions["Run"];
        dashAction = PlayerInput.actions["Dash"];
        pauseAction = PlayerInput.actions["Pause"];
        interactAction = PlayerInput.actions["Interact"];
        swingAction = PlayerInput.actions["Attack"];
    }

    void Update()
    {
        //Movement
        Movement = moveAction.ReadValue<Vector2>();

        jumpPressed = jumpAction.WasPressedThisFrame();
        jumpHeld = jumpAction.IsPressed();
        jumpReleased = jumpAction.WasReleasedThisFrame();
        DashPressed = dashAction.WasPressedThisFrame();
        RunHeld = runAction.IsPressed();
        swingPressed = swingAction.WasPressedThisFrame();

        //UI
        pausePressed = pauseAction.WasPressedThisFrame();
        interactPressed = interactAction.WasPressedThisFrame();

    }
}
