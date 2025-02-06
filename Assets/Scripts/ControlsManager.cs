using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    private InputSystem_Actions.PlayerActions playerActions;

    public GameObject player;
    PlayerController playerController;
    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerActions = playerInput.Player;

        playerController = player.GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //playerActions.MoveFlashlight.performed += ctx => flashlightController.onMove(ctx.ReadValue<Vector2>());
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
