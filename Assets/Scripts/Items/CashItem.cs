using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Cash")]
public class CashItem : Item
{
    private void Awake()
    {
        isSellable = false;
    }
}
