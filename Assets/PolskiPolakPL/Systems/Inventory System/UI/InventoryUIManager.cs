using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class InventoryUIManager : MonoBehaviour
{
    [field: Header("Selected Slot BG")]
    [Range(0, 1)] public float normalOpacity = .6f;
    [Range(0, 1)] public float selectedOpacity = .8f;

    [Header("Inventory UI Panels")]
    [SerializeField] GameObject playerInventoryPanel;
    [SerializeField] DescriptionPanelScript descrPanelScr;
    [field: SerializeField] public GameObject ChestUIPanel { get; private set; }
    [field: SerializeField] public GameObject CraftingUIPanel { get; private set; }

    public UnityEvent OnShowInventoryPanel;
    public UnityEvent OnHideInventoryPanel;


    InventorySystem inventory;
    bool canMoveItems;


    public static InventoryUIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        inventory = InventorySystem.Instance;
        if (ChestUIPanel)
        {
            ChestUIPanel.SetActive(false);
        }
        canMoveItems = inventory.moveItemScr != null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryPanel(!playerInventoryPanel.activeInHierarchy);
        }

        if (canMoveItems)
        {
            inventory.moveItemScr.HandleItemDrag();
            if (descrPanelScr)
                descrPanelScr.HandleDescriptionPanel(inventory.moveItemScr.GetHoveredSlot());
        }

        UpdateSlotBG(inventory.selectedSlot);
    }

    public void UpdateSlotBG(ItemSlot selectedSlot)
    {
        Image bgImage;
        foreach (ItemSlot slot in inventory.hotbarSlots)
        {
            bgImage = slot.bgImage;
            bgImage.color = (slot == selectedSlot) ? new Color(0, 0, 0, selectedOpacity) : new Color(0, 0, 0, normalOpacity);
        }
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
}
