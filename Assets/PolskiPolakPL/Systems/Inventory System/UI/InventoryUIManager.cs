using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InventoryUIManager : MonoBehaviour
{
    [field: SerializeField] public MoveItemScript moveItemScr { get; private set; }

    [Header("Inventory UI Panels")]
    [SerializeField] GameObject playerInventoryPanel;
    [SerializeField] DescriptionPanelScript descrPanelScr;
    [field: SerializeField] public GameObject ChestUIPanel { get; private set; }
    [field: SerializeField] public GameObject CraftingUIPanel { get; private set; }

    public UnityEvent OnShowInventoryPanel;
    public UnityEvent OnHideInventoryPanel;

    InventorySystem inventory;


    public static InventoryUIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        OnShowInventoryPanel.AddListener(clearSlotsBgColors);
    }

    private void Start()
    {
        inventory = InventorySystem.Instance;
        if (ChestUIPanel)
        {
            ChestUIPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && playerInventoryPanel)
        {
            ToggleInventoryPanel(!playerInventoryPanel.activeInHierarchy);
        }

        if (moveItemScr)
            moveItemScr.HandleItemDrag();

        if (descrPanelScr)
            descrPanelScr.HandleDescriptionPanel(GetHoveredSlot());
    }

    public void ToggleInventoryPanel(bool toggle)
    {
        // Handle Inventory Panel
        playerInventoryPanel.SetActive(toggle);
        inventory.IsHotbarActive = !toggle;

        // Handle Cursor
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;

        if (toggle)
        {
            OnShowInventoryPanel?.Invoke();
            return;
        }
        OnHideInventoryPanel?.Invoke();
    }

    void clearSlotsBgColors()
    {
        foreach(ItemSlot slot in inventory.playerInventorySlots)
        {
            slot.bgImage.color = slot.bgColor;
        }
    }

    public ItemSlot GetHoveredSlot()
    {
        List<ItemSlot> allSlots = new List<ItemSlot>();

        allSlots.AddRange(inventory.playerInventorySlots);
        if(ItemStorageScript.ChestUISlots != null)
            allSlots.AddRange(ItemStorageScript.ChestUISlots);


        foreach (ItemSlot slot in allSlots)
        {
            if (slot.hovering)
                return slot;
        }

        return null;
    }

    private void OnDestroy()
    {
        OnShowInventoryPanel.RemoveAllListeners();
        OnHideInventoryPanel.RemoveAllListeners();
    }
}
