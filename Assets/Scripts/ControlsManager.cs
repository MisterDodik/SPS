using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : Singleton<ControlsManager>
{
    public event EventHandler OnSprintPerformed;
    public event EventHandler OnSprintCanceled;
    public event EventHandler OnJump;
    public event EventHandler OnInteract;
    public event EventHandler OnInventory;
    public event EventHandler OnScamWheelActivate;
    public event EventHandler OnScamWheelDisable;


    private InputSystem_Actions playerInput;
    public InputSystem_Actions.PlayerActions playerActions;

    [SerializeField] private PlayerController playerController;
    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerActions = playerInput.Player;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerActions.Sprint.performed += Sprint_performed;
        playerActions.Sprint.canceled += Sprint_canceled;
        playerActions.Jump.performed += Jump_performed;

        playerActions.Interact.started += Interact_started;
        playerActions.Inventory.performed += Inventory_performed;

        playerActions.ScamWheel.performed += ScamWheel_performed;
        playerActions.ScamWheel.canceled += ScamWheel_canceled;

    }

    private void ScamWheel_performed(InputAction.CallbackContext obj)
    {
        OnScamWheelActivate?.Invoke(this, EventArgs.Empty);
    }
    private void ScamWheel_canceled(InputAction.CallbackContext obj)
    {
        OnScamWheelDisable?.Invoke(this, EventArgs.Empty);
    }

    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        OnInventory?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_started(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_canceled(InputAction.CallbackContext obj)
    {
        OnSprintCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_performed(InputAction.CallbackContext obj)
    {
        OnSprintPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        playerController.onMove(playerActions.Move.ReadValue<Vector2>());
    }

    public string getKeyBinding(InputAction action)
    {       
        return (action.GetBindingDisplayString(0, InputBinding.DisplayStringOptions.DontIncludeInteractions));
    }
    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
