using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : Singleton<ControlsManager>
{
    public event EventHandler OnSprintPerformed;
    public event EventHandler OnSprintCanceled;
    private InputSystem_Actions playerInput;
    private InputSystem_Actions.PlayerActions playerActions;

    [SerializeField] private PlayerController playerController;
    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerActions = playerInput.Player;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerActions.Sprint.performed += Sprint_performed;
        playerActions.Sprint.canceled += Sprint_canceled;

        //playerActions.MoveFlashlight.performed += ctx => flashlightController.onMove(ctx.ReadValue<Vector2>());
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

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
