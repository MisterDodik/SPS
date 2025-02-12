using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask layerMask;

    public Transform cameraTransform;

    [SerializeField] private RawImage crosshairImage;
    private void Start()
    {
        ControlsManager.Instance.OnInteract += Instance_OnInteractPerformed;
    }

    private void Instance_OnInteractPerformed(object sender, EventArgs e)
    {
        FindInteractable();
    }
    private void Update()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, 2, layerMask))
        {
            crosshairImage.color = Color.black;
        }
        else
        {
            crosshairImage.color = Color.white;
        }
    }
    public void FindInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 2, layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteract interactable))
            {
                interactable.Interact();
            }
        }
    }
}
