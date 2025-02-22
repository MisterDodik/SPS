using UnityEngine;

public class Shopman_Interact : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        InventorySystem.Instance.SellCommonItems();
    }
}