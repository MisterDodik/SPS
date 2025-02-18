using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : Singleton<InventorySystem>
{
    List<InventorySlot> container = new List<InventorySlot>();          // Actual objects that the player owns
    List<InventorySlot> addedItems = new List<InventorySlot>();         // Keeps track what items have already been added to the inventory

    GameObject Inventory;
    Transform InventorySlots;

    bool isInventoryOpen = false;

    public GameObject itemPrefab;

    private void Start()
    {
        Inventory = UIManager.Instance.GetUI<CanvasGameplay>().GetInventoryObject();
        InventorySlots = UIManager.Instance.GetUI<CanvasGameplay>().GetInventoryContent().transform;
        
        ControlsManager.Instance.OnInventory += ToggleInventory;
    }

    private void ToggleInventory(object sender, EventArgs e)
    {
        isInventoryOpen = !isInventoryOpen;

        Cursor.lockState = isInventoryOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;
        Inventory.SetActive(isInventoryOpen);

        showItems();
    }


    void showItems()
    {
        // Remove items that no longer exist
        for (int i = addedItems.Count - 1; i >= 0; i--)
        {
            if (!container.Contains(addedItems[i]) || addedItems[i].amount == 0)
            {
                Destroy(InventorySlots.GetChild(i).gameObject);
                addedItems.RemoveAt(i);
            }
        }

        // Update existing items or add new ones
        for (int i = 0; i < container.Count; i++)
        {
            InventorySlot slot = container[i];

            if (addedItems.Contains(slot))
            {
                // Update existing item
                Transform itemSlot = InventorySlots.GetChild(i);
                UpdateItemSlotUI(itemSlot, slot, i);
            }
            else
            {
                // Add new item
                GameObject item = Instantiate(itemPrefab, InventorySlots);
                UpdateItemSlotUI(item.transform, slot, i);
                addedItems.Add(slot);
            }
        }

    }
    private void UpdateItemSlotUI(Transform slot, InventorySlot inventorySlot, int index)
    {
        slot.GetComponent<RectTransform>().anchoredPosition = GetItemPos(index);
        slot.GetComponentInChildren<Image>().sprite = inventorySlot.item.sprite;
        slot.GetComponentInChildren<TextMeshProUGUI>().text = $"{inventorySlot.amount} X {inventorySlot.item.name}";
    }

    Vector2 GetItemPos(int index)
    {
        return new Vector2(50, -50 * (index + 1));
    }



    public void AddItem(Item item, int amount)
    {
        bool isPresent = false;

        for(int i=0; i< container.Count; i++)
        {
            if (container[i].item == item)
            {
                isPresent = true;
                container[i].amount += amount;
                break;
            }
        }

        if(!isPresent)
        {
            container.Add(new InventorySlot(item, amount));
        }
    }


    public void SellItem(Item item)
    {
        // ---Prototype implementation
        /*
        InventorySlot existingSlot = container.Find(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.amount--;

            if (existingSlot.amount <= 0)
            {
                container.Remove(existingSlot);
            }
        }
        showItems();
        */
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;

    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
