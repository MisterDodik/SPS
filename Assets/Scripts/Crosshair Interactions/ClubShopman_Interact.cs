using UnityEngine;

public class ClubShopman_Interact : MonoBehaviour, IInteractable
{
    private void Start()
    {
        ControlsManager.Instance.OnShopkeeperHold += Instance_OnShopkeeperHold;
    }

    private void Instance_OnShopkeeperHold(object sender, System.EventArgs e)
    {
        InventorySystem.Instance.SellValuables(true);  //Sells only all valuables of type x
    }

    public void Interact(Player player)
    {
        InventorySystem.Instance.SellValuables(false); // Sells only 1 item
    }
}
