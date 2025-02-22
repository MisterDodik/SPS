using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Valuable")]
public class ValuableItems : Item
{
    private void Awake()
    {
        isValuable = true;
    }
}
