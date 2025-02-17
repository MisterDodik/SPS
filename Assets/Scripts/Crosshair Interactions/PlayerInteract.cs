using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    RaycastHit hit;

    public Transform cameraTransform;

    [SerializeField] private RawImage crosshairImage;

    private string interactAction;

    private void Start()
    {
        ControlsManager.Instance.OnInteract += ControlsManager_OnInteractPerformed;

        //---This should be changed later, when we add key bindings settings, so that it gets updated
        interactAction = ControlsManager.Instance.getKeyBinding(ControlsManager.Instance.playerActions.Interact);
    }

    private void ControlsManager_OnInteractPerformed(object sender, EventArgs e)
    {
        FindInteractable();
    }
    private void Update()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 2.5f, layerMask))
        {
            crosshairImage.color = Color.black;
            TextBubbleScript.instance.CreateBubble(hit.transform, new Vector3(0, 1.5f, 0), GetText());
        }
        else
        {
            hit.normal = Vector3.zero;  //nullifing

            crosshairImage.color = Color.white;
            TextBubbleScript.instance.DestroyBubble();
            PickPocket.Instance.EndEvent();
        }
    }
    public void FindInteractable()
    {
        if (hit.normal != Vector3.zero && hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact(GetComponent<Player>());
        }
    }

    string GetText()
    {
        return "Press " + interactAction + " to interact.";
    }
}
