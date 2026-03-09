using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance {  get; private set; }
    [Header("Player Hand")]
    [SerializeField] Transform playerHand;
    [SerializeField] float throwingForce = 5;
    [Header("UI")]
    [SerializeField] Transform slotsParent;
    [SerializeField][Range(0,1)] float normalOpacity = .6f;
    [SerializeField][Range(0, 1)] float selectedOpacity = .8f;
    [SerializeField] Rect baseUIRect;

    int selectedIndex = 0;
    List<ItemSlot> itemSlots = new List<ItemSlot>();

    private void Awake()
    {
        if(Instance && Instance!=this)
            Destroy(gameObject);
        else
            Instance = this;

        itemSlots.AddRange(slotsParent.GetComponentsInChildren<ItemSlot>());
    }

    private void Update()
    {
        HandleHotbarSelection();
        HandleItemDropping();
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

        //item opacity + item in hand
        UpdateSelectedSlot();
        SetItemInHand();
    }

    void UpdateSelectedSlot()
    {
        Image bgImage;
        for (int i = 0; i < itemSlots.Count; i++)
        {
            bgImage = itemSlots[i].GetComponent<Image>();
            if (i == selectedIndex)
                bgImage.color = new Color(0, 0, 0, selectedOpacity);
            else
                bgImage.color = new Color(0,0,0,normalOpacity);
        }
    }

    void SetItemInHand()
    {
        ItemSlot selectedSlot = itemSlots[selectedIndex];
        foreach (Transform child in playerHand)
            Destroy(child.gameObject);
        if (selectedSlot.HasItem())
            Instantiate(selectedSlot.GetItemData().HandPrefab, playerHand);
    }

    void HandleItemDropping()
    {
        if(!Input.GetKeyDown(KeyCode.G))
            return;
        ItemSlot selectedSlot = itemSlots[selectedIndex];
        if (!selectedSlot.HasItem())
            return;

        ItemData itemData = selectedSlot.GetItemData();
        // Remove hand item prefab
        GameObject handPrefab = playerHand.GetChild(0).gameObject;
        Destroy(handPrefab);

        // Create and throw world item prefab
        GameObject droppedItemGo = Instantiate(itemData.WorldPrefab, playerHand.position, playerHand.rotation);
        droppedItemGo.GetComponent<Rigidbody>().AddForce(playerHand.forward * throwingForce, ForceMode.Impulse);

        selectedSlot.ClearSlot();
    }

    public Rect GetUVRectFromItemArray(ItemData item)
    {
        Vector2 arrayPosition = IdToArrayPosition(item);
        float x = baseUIRect.x * arrayPosition.x;
        float y = baseUIRect.y * arrayPosition.y;
        Debug.Log($"X: {x} \t Y: {y}");
        return new Rect(x, y, baseUIRect.width, baseUIRect.height);
    }

    Vector2 IdToArrayPosition(ItemData item)
    {
        Vector2 arrayPosition;
        int rowSize = Mathf.RoundToInt(1 / baseUIRect.width);
        arrayPosition.x = (float)item.ID%rowSize;
        arrayPosition.y = Mathf.Floor((float)item.ID/rowSize);
        return arrayPosition;
    }

}
