using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : Singleton<InventorySystem>
{
    [HideInInspector] public List<InventorySlot> container = new List<InventorySlot>();          // Actual objects that the player owns
    List<InventorySlot> addedItems = new List<InventorySlot>();         // Keeps track what items have already been added to the inventory

    GameObject Inventory;
    Transform InventorySlots;

    bool isInventoryOpen = false;

    public GameObject itemPrefab;

    public Item cashIcon;

    private InventorySlot selectedItem;
    bool isSomethingSelected = false;

    [SerializeField] private float clubScamChance = 40;     
    [SerializeField] private float clubPriceMultiplier = 2.5f;

    private void Start()
    {
        Inventory = UIManager.Instance.GetUI<CanvasGameplay>().GetInventoryObject();
        InventorySlots = UIManager.Instance.GetUI<CanvasGameplay>().GetInventoryContent().transform;
        
        ControlsManager.Instance.OnInventory += ToggleInventory;
    }

    private void ToggleInventory(object sender, EventArgs e)
    {
        isInventoryOpen = !isInventoryOpen;

        showItems();

        Cursor.lockState = isInventoryOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;
        Inventory.SetActive(isInventoryOpen);

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

                ItemOnSelect itemScript = item.AddComponent<ItemOnSelect>();
                itemScript.thisSlot = slot;
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
        return new Vector2(100, -50 * (index + 1));
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

    public void selectItem(InventorySlot currentItem)
    {
        isSomethingSelected=true;
        ScamWheel.Instance.selectedScam = ScamType.Null;

        Player.Instance.updateSelectedItem(currentItem.item.sprite, currentItem.item.name);
        selectedItem = currentItem;
    }
    public void deselectItem()
    {
        isSomethingSelected = false;
        Player.Instance.updateSelectedItem(null, " ");
        selectedItem = null;
    }

    public void SellAllItemsByType(bool sellValuables)
    {
        float total = 0;
        for(int i=0; i< addedItems.Count; i++)
        {
            InventorySlot slot = addedItems[i];

            if (!slot.item.isValuable == sellValuables)
                continue;           

            int amount = slot.amount;
            float price = slot.item.basePrice;
            total += amount * price;

            slot.amount = 0;
            container.Remove(slot);
        }

        if (total == 0)
        {
            print("No items were sold");
            return;
        }

        showItems();            // updates inventory ui
        Player.Instance.ChangeMoney(total);
        ScamBase.Instance.showStolenItem(cashIcon, total);          //shows money icon in the bottom left corner

        deselectItem();
    }

    public void SellValuables(bool sellAll)
    {
        if (!isSomethingSelected)
        {
            NotifyPlayerText.Instance.NotifyPlayer(new Color(1, 0.2f, 0), new Color(1, 0, 0), "No item selected!", false);
            return;
        }
        float total = 0;    

        bool isPlayerScammed = UnityEngine.Random.Range(0, 100) < clubScamChance ? true : false; 

        float price = selectedItem.item.basePrice * clubPriceMultiplier;
        int amount = selectedItem.amount;
        if (sellAll)
        {
            total += amount * price;
            selectedItem.amount = 0;
            container.Remove(selectedItem);
            deselectItem();
        }
        else
        {
            total += price;
            amount--;
            selectedItem.amount = amount;
            if (selectedItem.amount == 0)
            {
                container.Remove(selectedItem);
                deselectItem();
            }
        }
        if(isPlayerScammed)
        {
            total = 0;
            NotifyPlayerText.Instance.NotifyPlayer(new Color(1, 0.3f, 0), new Color(1, 0, 0), "You got scammed!", false);
        }
        if (total == 0)
            return;

        showItems();            // updates inventory ui
        Player.Instance.ChangeMoney(total);
        ScamBase.Instance.showStolenItem(cashIcon, total);          //shows money icon in the bottom left corner
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
