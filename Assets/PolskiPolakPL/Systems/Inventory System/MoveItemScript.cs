using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveItemScript : MonoBehaviour
{
    [SerializeField] RawImage dragIcon;
    public ItemSlot draggedSlot {  get; private set; }

    InventorySystem inventorySys;

    private void Start()
    {
        inventorySys = InventorySystem.Instance;
        InventoryUIManager.Instance.OnHideInventoryPanel.AddListener(Abort);
    }

    public void HandleItemDrag()
    {
        //StartDrag
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartDrag();
        }
        //UpdateDragPosition
        if (IsDragging())
            UpdateDragPosition();
        //EndDrag
        if (Input.GetKeyUp(KeyCode.Mouse0) && IsDragging())
        {
            EndDrag();
        }
    }

    void StartDrag()
    {
        ItemSlot hoveredSlot = GetHoveredSlot();

        if (!hoveredSlot || !hoveredSlot.HasItem())
            return;

        draggedSlot = hoveredSlot;

        //Show drag item
        dragIcon.texture = draggedSlot.GetItem().ImageTexture;
        dragIcon.uvRect = draggedSlot.GetItem().UVRect;
        dragIcon.color = new Color(1, 1, 1, 0.5f);
        dragIcon.enabled = true;
    }

    void UpdateDragPosition()
    {
        dragIcon.transform.position = Input.mousePosition;
    }

    void EndDrag()
    {
        ItemSlot hovered = GetHoveredSlot();
        HandleDrop(draggedSlot, hovered);
        dragIcon.enabled = false;
        draggedSlot = null;
    }

    public void Abort()
    {
        dragIcon.enabled = false;
        draggedSlot = null;
    }

    bool IsDragging()
    {
        return draggedSlot;
    }

    public ItemSlot GetHoveredSlot()
    {
        List<ItemSlot> allSlots = new List<ItemSlot>();

        allSlots.AddRange(inventorySys.playerInventorySlots);
        allSlots.AddRange(ItemStorageScript.ChestUISlots);


        foreach (ItemSlot slot in allSlots)
        {
            if (slot.hovering)
                return slot;
        }

        return null;
    }

    void HandleDrop(ItemSlot originSlot, ItemSlot targetSlot)
    {
        if (!targetSlot)
        {
            inventorySys.DropItem(originSlot.GetItem(), originSlot.GetAmount());
            originSlot.ClearSlot();
            return;
        }

        if (targetSlot == originSlot)
            return;

        // Stack Items
        if (TryMergeItems(originSlot, targetSlot))
            return;

        //Swap Items
        if (TrySwapItems(originSlot, targetSlot))
            return;

        // Move Item
        targetSlot.SetItem(originSlot.GetItem(), originSlot.GetAmount());
        originSlot.ClearSlot();
    }

    bool TryMergeItems(ItemSlot originSlot, ItemSlot targetSlot)
    {
        if (!targetSlot.HasItem() || targetSlot.GetItem() != originSlot.GetItem())
            return false;
        int max = targetSlot.GetItem().StackSize;
        int space = max - targetSlot.GetAmount();

        if (space <= 0)
            return false;

        int move = Mathf.Min(space, originSlot.GetAmount());
        targetSlot.SetItem(targetSlot.GetItem(), targetSlot.GetAmount() + move);
        originSlot.SetItem(originSlot.GetItem(), originSlot.GetAmount() - move);

        if (originSlot.GetAmount() <= 0)
            originSlot.ClearSlot();
        return true;
    }

    bool TrySwapItems(ItemSlot originSlot, ItemSlot targetSlot)
    {
        if (targetSlot.HasItem())
        {
            ItemData tempItem = targetSlot.GetItem();
            int tempAmount = targetSlot.GetAmount();

            targetSlot.SetItem(originSlot.GetItem(), originSlot.GetAmount());
            originSlot.SetItem(tempItem, tempAmount);
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        InventoryUIManager.Instance.OnHideInventoryPanel.RemoveListener(Abort);
    }
}
