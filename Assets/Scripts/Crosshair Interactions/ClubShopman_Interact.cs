using UnityEngine;

public class ClubShopman_Interact : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        InventorySystem.Instance.SellValuables();
    }
}
