using UnityEngine;

public class Shopman_Interact : MonoBehaviour, IInteractable
{

    private void Start()
    {
        ControlsManager.Instance.OnShopkeeperHold += Instance_OnShopkeeperHold;
    }

    private void Instance_OnShopkeeperHold(object sender, System.EventArgs e)
    {
        InventorySystem.Instance.SellAllItemsByType(true);  //Sells only valuable items
    }

    public void Interact(Player player)
    {
        InventorySystem.Instance.SellAllItemsByType(false); // Sells only common items
    }   
}