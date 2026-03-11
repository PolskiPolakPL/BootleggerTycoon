using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance {  get; private set; }

    //PLAYER HAND
    [SerializeField] Transform playerHand;
    [SerializeField] float throwingForce = 5;
    
    // HOTBAR
    [SerializeField] Transform playerHotbar;
    [SerializeField][Range(0,1)] float normalSlotOpacity = .6f;
    [SerializeField][Range(0, 1)] float selectedSlotOpacity = .8f;
    [SerializeField][Min(1)][Tooltip("How many items are in a single row of 'GameItems.png' file (or Render Texture).")] int itemsPNGArraySize = 10;

    private int previousIndex = 0;
    private int selectedIndex = 0;
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    private void Awake()
    {
        if(Instance && Instance!=this)
            Destroy(gameObject);
        else
            Instance = this;

        itemSlots.AddRange(playerHotbar.GetComponentsInChildren<ItemSlot>());
        foreach(ItemSlot slot in itemSlots)
        {
            slot.UpdateSlot();
        }
        UpdateSelectedSlot();
        UpdatePlayerHand();
        previousIndex = selectedIndex;
    }

    private void Update()
    {
        HandleHotbarSelection();
        if(Input.GetKeyDown(KeyCode.G))
            TryDropItem(itemSlots[selectedIndex]);
    }

    public bool AddItem(ItemData item)
    {
        ItemSlot selectedSlot = itemSlots[selectedIndex];
        //Try putting item in selected slot
        if (!selectedSlot.HasItem())
        {
            selectedSlot.SetItem(item);
            return true;
        }
        //Try putting item in any slot
        int i = 0;
        foreach (ItemSlot slot in itemSlots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return true;
            }
            i++;
        }
        Debug.Log("Inventory full!");
        return false;
    }

    void HandleHotbarSelection()
    {
        // scroll down
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedIndex++;
            selectedIndex %= itemSlots.Count;
        }
        //scroll up
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex <= 0)
                selectedIndex = itemSlots.Count - 1;
            else
                selectedIndex--;
        }
        // 1-X key-binds
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
                selectedIndex = i;
        }

        if(selectedIndex==previousIndex)
            return;
        // Update UI Slot & Player Hand
        UpdateSelectedSlot();
        UpdatePlayerHand();
        previousIndex = selectedIndex;
    }

    void UpdateSelectedSlot()
    {
        Image bgImage;
        for (int i = 0; i < itemSlots.Count; i++)
        {
            bgImage = itemSlots[i].GetComponent<Image>();
            if (i == selectedIndex)
                bgImage.color = new Color(0, 0, 0, selectedSlotOpacity);
            else
                bgImage.color = new Color(0,0,0,normalSlotOpacity);
        }
    }

    void UpdatePlayerHand()
    {
        ItemSlot selectedSlot = itemSlots[selectedIndex];
        foreach (Transform child in playerHand)
            Destroy(child.gameObject);
        if (selectedSlot.HasItem())
            Instantiate(selectedSlot.GetItemData().HandPrefab, playerHand);
    }

    bool TryDropItem(ItemSlot selectedSlot)
    {
        if (!selectedSlot.HasItem())
            return false;

        ItemData itemData = selectedSlot.GetItemData();
        if(!itemData.isThrowable)
            return false;

        DropItem(itemData);
        selectedSlot.ClearSlot();
        return true;
    }

    void DropItem(ItemData itemData)
    {
        // Remove hand item prefab
        GameObject handPrefab = playerHand.GetChild(0).gameObject;
        Destroy(handPrefab);
        // Create and throw world item prefab
        GameObject droppedItemGo = Instantiate(itemData.WorldPrefab, playerHand.position, playerHand.rotation);
        droppedItemGo.GetComponent<Rigidbody>().AddForce(playerHand.forward * throwingForce, ForceMode.Impulse);
    }

    public Rect GetUVRectFromItemArray(ItemData item)
    {
        Vector2 arrayPosition = IdToArrayPosition(item);
        float step = 1/(float)itemsPNGArraySize;
        float x = step * arrayPosition.x;
        float y = step * arrayPosition.y;
        Debug.Log($"X: {x} \t Y: {y}");
        return new Rect(x, y, step, step);
    }

    Vector2 IdToArrayPosition(ItemData item)
    {
        Vector2 arrayPosition;
        arrayPosition.x = (float)item.ID%itemsPNGArraySize;
        arrayPosition.y = Mathf.Floor((float)item.ID/ itemsPNGArraySize);
        return arrayPosition;
    }

}
