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
        canMoveItems = moveItemScr != null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && playerInventoryPanel)
        {
            ToggleInventoryPanel(!playerInventoryPanel.activeInHierarchy);
        }

        if (canMoveItems)
        {
            moveItemScr.HandleItemDrag();
            if (descrPanelScr)
                descrPanelScr.HandleDescriptionPanel(moveItemScr.GetHoveredSlot());
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
