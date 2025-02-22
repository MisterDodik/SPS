using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite sprite;

    [HideInInspector]public bool isSellable = true;
    [HideInInspector] public bool isValuable = false;

    public float basePrice;

}
