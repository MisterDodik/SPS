using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class ControlsManager : Singleton<ControlsManager>
{
    public event EventHandler OnSprintPerformed;
    public event EventHandler OnSprintCanceled;
    public event EventHandler OnJump;
    public event EventHandler OnInventory;
    public event EventHandler OnInteractOrWheel;
    public event EventHandler OnScamWheelActivate;      
    public event EventHandler OnScamWheelDisable;
    public event EventHandler OnShopkeeperHold;

    private InputSystem_Actions playerInput;
    public InputSystem_Actions.PlayerActions playerActions;

    [SerializeField] private PlayerController playerController;


    [HideInInspector] public bool shopInteractions = false; //when the player interacts with shopkeepers then holding interact key wont trigger scam wheel

    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerActions = playerInput.Player;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerActions.Sprint.performed += Sprint_performed;
        playerActions.Sprint.canceled += Sprint_canceled;
        playerActions.Jump.performed += Jump_performed;

        playerActions.InteractNWheel.performed += InteractOrWheel_performed;
        playerActions.InteractNWheel.canceled += ScamWheel_canceled;

        playerActions.Inventory.performed += Inventory_performed;

    }

    private void ScamWheel_canceled(InputAction.CallbackContext obj)
    {
        OnScamWheelDisable?.Invoke(this, EventArgs.Empty);
    }
    private void InteractOrWheel_performed(InputAction.CallbackContext obj)
    {
        if (obj.interaction is PressInteraction)
        {
            OnInteractOrWheel?.Invoke(this, EventArgs.Empty);
        }
        else if (obj.interaction is HoldInteraction)
        {
            if (!shopInteractions)
                OnScamWheelActivate?.Invoke(this, EventArgs.Empty);
            else
                OnShopkeeperHold?.Invoke(this, EventArgs.Empty);
        }
    }


    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        OnInventory?.Invoke(this, EventArgs.Empty);
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
