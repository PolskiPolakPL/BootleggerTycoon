using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Slots Parents")]
    [SerializeField] Transform hotbarSlotsParent;
    [SerializeField] Transform backpackSlotsParent;

    // Slots Lists
    public List<ItemSlot> hotbarSlots { get; private set; } = new List<ItemSlot>();
    public List<ItemSlot> backpackSlots { get; private set; } = new List<ItemSlot>();
    public List<ItemSlot> playerInventorySlots { get; private set; } = new List<ItemSlot>();

    [Header("Player Hand")]
    [SerializeField] Transform playerHand;
    [SerializeField] KeyCode dropKey = KeyCode.G;
    [SerializeField] float throwingForce = 5;

    //events
    public event Action<ItemData, int> OnItemAdded;
    public event Action<ItemData, int> OnItemRemoved;

    public bool IsHotbarActive = true;
    int selectedIndex = 0;
    public ItemSlot selectedSlot {  get; private set; }
    [field: SerializeField] public MoveItemScript moveItemScr { get; private set; }

    // Singleton Instance
    public static InventorySystem Instance {  get; private set; }
    private void Awake()
    {
        if(Instance && Instance!=this)
            Destroy(gameObject);
        else
            Instance = this;

        InitializeLists();

    }

    void InitializeLists()
    {
        if(!hotbarSlotsParent && !backpackSlotsParent)
        {
            Debug.LogWarning($"[{this}]: None of the slots parent was attached! Attach at least one slot parent.");
            return;
        }
        if (hotbarSlotsParent)
        {
            hotbarSlots.AddRange(hotbarSlotsParent.GetComponentsInChildren<ItemSlot>());
            playerInventorySlots.AddRange(hotbarSlots);
        }
        if(backpackSlotsParent)
        {
            backpackSlots.AddRange(backpackSlotsParent.GetComponentsInChildren<ItemSlot>());
            playerInventorySlots.AddRange(backpackSlots);
        }
    }

    private void Update()
    {
        HandleHotbarSelection();
        HandleItemDropping();
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        //Try putting item in selected slot
        if(TryAddToSelectedSlot(item, amount, out int remainingAmount))
        {
            OnItemAdded?.Invoke(item, amount - remainingAmount);
            return true;
        }

        amount = remainingAmount;
        //Try putting item in any slot with the same item
        if(TryFillItemSlots(item, amount, out remainingAmount))
        {
            OnItemAdded?.Invoke(item, amount - remainingAmount);
            return true;
        }

        amount = remainingAmount;
        // Add item to empty Slot
        if (TryFillEmptySlots(item, amount, out remainingAmount))
        {
            OnItemAdded?.Invoke(item, amount - remainingAmount);
            return true;
        }

        //Inventory full
        Debug.Log($"Inventory is full! Could not add {remainingAmount} of {item.DisplayName}");
        return false;
    }

    #region Adding item to Inventory
    bool TryAddToSelectedSlot(ItemData item, int amount, out int remainder)
    {
        // define remainder
        remainder = amount;

        if (!selectedSlot.HasItem()) // EMPTY SLOT
            remainder = FillEmptySlot(item, amount, selectedSlot);

        else if (selectedSlot.GetItem() == item) // SLOT HAS CORRECT ITEM
            remainder = AddAmountToSlot(amount, selectedSlot);

        if(remainder <= 0)
        {
            EquipHandItem();
            return true;
        }
        return false;
    }
    bool TryFillItemSlots(ItemData item, int amount, out int remainder)
    {
        // define remainder
        remainder = amount;

        foreach (ItemSlot slot in playerInventorySlots)
        {
            // IF Slot HAS CORRECT Item
            if (slot.HasItem() && slot.GetItem() == item)
            {
                // Fill that Item Slot
                remainder = AddAmountToSlot(remainder, slot);

                if (remainder <= 0)
                    return true;
            }
        }
        // if remainder is above 0
        return false;
    }
    bool TryFillEmptySlots(ItemData item, int amount, out int remainder)
    {
        // define remainder
        remainder = amount;

        foreach (ItemSlot slot in playerInventorySlots)
        {
            if (!slot.HasItem()) // IF Slot IS EMPTY
            {
                // Fill Empty Slot
                remainder = FillEmptySlot(item, remainder, slot);

                if (remainder <= 0)
                    return true;
            }
        }
        // if remainder is above 0
        return false;
    }

    int AddAmountToSlot(int amount, ItemSlot slot)
    {
        // define starting value
        int remainingAmount = amount;

        // define how much Slot has and its max capacity
        int currentSlotAmount = slot.GetAmount();
        int maxStackSize = slot.GetItem().StackSize;
        // IF Slot HAS "room for more"
        int freeSpace = maxStackSize - currentSlotAmount;
        if (freeSpace > 0)
        {
            // add as much as you can
            int addAmount = Mathf.Min(freeSpace, amount);
            slot.AddAmount(addAmount);

            // update remaining
            remainingAmount -= addAmount;
        }
        return remainingAmount;
    }
    int FillEmptySlot(ItemData item, int amount, ItemSlot slot)
    {

        // add new Item (as much as you can)
        int fillAmount = Mathf.Min(item.StackSize, amount);
        slot.SetItem(item, fillAmount);

        // return remaining
        return amount - fillAmount;
    }
    #endregion

    void HandleHotbarSelection()
    {
        if (!IsHotbarActive)
        {
            EquipHandItem();
            return;
        }
        // scroll down
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedIndex++;
            selectedIndex %= hotbarSlots.Count;
        }
        //scroll up
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex <= 0)
                selectedIndex = hotbarSlots.Count - 1;
            else
                selectedIndex--;
        }
        // 1-X key-binds
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
                selectedIndex = i;
        }

        //item opacity + item in hand
        if(selectedSlot != hotbarSlots[selectedIndex])
        {
            selectedSlot = hotbarSlots[selectedIndex];
            EquipHandItem();
        }
    }

    void EquipHandItem()
    {
        foreach (Transform child in playerHand)
            Destroy(child.gameObject);
        if (selectedSlot.HasItem())
            Instantiate(selectedSlot.GetItem().HandPrefab, playerHand);
    }

    void HandleItemDropping()
    {
        // NOT pressed a Key
        if(!Input.GetKeyDown(dropKey))
            return;

        // Slot has NOT item
        if (!selectedSlot.HasItem())
            return;

        // Slot is busy being dragged
        if (moveItemScr && selectedSlot == moveItemScr.draggedSlot)
            return;

        ItemData selectedItem = selectedSlot.GetItem();

        // Item has NOT worldPrefab (isn't droppable)
        if (!selectedItem.WorldPrefab)
            return;

        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Drop all items
            DropItem(selectedItem, selectedSlot.GetAmount());
            // clear selected slot
            selectedSlot.ClearSlot();
        }
        else
        {
            // drop one item
            DropItem(selectedItem);
            selectedSlot.RemoveAmount(1);
        }
        EquipHandItem();
    }

    public void DropItem(ItemData item, int dropAmount = 1)
    {
        // Create and throw world item prefab
        GameObject droppedItemGO = Instantiate(item.WorldPrefab, playerHand.position, playerHand.rotation);
        droppedItemGO.GetComponent<Rigidbody>().AddForce(playerHand.forward * throwingForce, ForceMode.Impulse);
        // Set dropped amount to match
        droppedItemGO.GetComponent<ItemScript>().amount = dropAmount;
        //trigger event
        OnItemRemoved?.Invoke(item, dropAmount);
    }

    public void ConsumeSelectedItem(int amount = 1)
    {
        if(!selectedSlot.HasItem())
            return;
        selectedSlot.RemoveAmount(amount);
    }
}
