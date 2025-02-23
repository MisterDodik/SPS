using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnSelect : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public InventorySlot thisSlot;
    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySystem.Instance.selectItem(thisSlot);
    }
}
