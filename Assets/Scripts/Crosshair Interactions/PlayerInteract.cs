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

    ScamBase currentScam;
    private void Start()
    {
        ControlsManager.Instance.OnInteractOrWheel += ControlsManager_OnInteractPerformed;

        //---This should be changed later, when we add key bindings settings, so that it gets updated
        interactAction = ControlsManager.Instance.getKeyBinding(ControlsManager.Instance.playerActions.InteractNWheel);
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
            TextBubbleScript.instance.CreateBubble(hit.transform, new Vector3(0, 1.5f, 0), GetText(hit.collider.tag));
        }
        else
        {
            hit.normal = Vector3.zero;  //nullifing

            crosshairImage.color = Color.white;
            TextBubbleScript.instance.DestroyBubble();

            //resetting the current scam event
            currentScam = ScamWheel.Instance.scam;
            if (currentScam != null)
            {
                currentScam.ResetDifficulty();
                currentScam.EndEvent();
            }
        }
    }
    public void FindInteractable()
    {
        if (hit.normal != Vector3.zero && hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact(GetComponent<Player>());
        }
    }

    string GetText(string hitTag)
    {
        if(hitTag == "Shopman")
            return "Press " + interactAction + " to sell all the common items.";
        else if (hitTag == "ClubShopman")
            return "Press " + interactAction + " to sell all the valuables.";
        return "Press " + interactAction + " to interact.";
    }
}
