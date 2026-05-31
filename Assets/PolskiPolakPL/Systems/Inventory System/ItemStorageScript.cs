using UnityEngine;

[System.Serializable]
public record StoredItem
{
    public ItemData Item;
    public int amount;
}

public class ItemStorageScript : MonoBehaviour
{
    Interactable interactable;

    [SerializeField] StoredItem[] storedItems;

    GameObject ChestUIPanel;
    public static ItemSlot[] ChestUISlots { get; private set; }
    int chestSize;

    bool isOpen;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += OpenCloseChest;
    }

    private void Start()
    {
        if (!ChestUIPanel)
        {
            ChestUIPanel = InventoryUIManager.Instance.ChestUIPanel;
            ChestUISlots = ChestUIPanel.GetComponentsInChildren<ItemSlot>(true);
            foreach (ItemSlot slot in ChestUISlots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        chestSize = Mathf.Min(storedItems.Length, ChestUISlots.Length);
        ChestUIPanel.SetActive(false);
        InventoryUIManager.Instance.OnHideInventoryPanel.AddListener(Close);
    }

    void OpenCloseChest()
    {
        if (!isOpen)
            Open();
        else
            Close();
        InventoryUIManager.Instance.ToggleInventoryPanel(isOpen);
    }

    void Open()
    {
        isOpen = true;
        ReadChest();
        ChestUIPanel.SetActive(true);
        interactable.message = "Close chest";
    }

    public void Close()
    {
        if (!isOpen) return;

        WriteChest();
        ChestUIPanel.SetActive(false);
        interactable.message = "Open chest";
        isOpen = false;
    }

    void WriteChest()
    {
        for (int i = 0; i < chestSize; i++)
        {
            if (ChestUISlots[i].HasItem())
            {
                storedItems[i].Item = ChestUISlots[i].GetItem();
                storedItems[i].amount = ChestUISlots[i].GetAmount();
            }
            else
            {
                storedItems[i].Item = null;
                storedItems[i].amount = 0;
            }
            ChestUISlots[i].gameObject.SetActive(false);
        }
    }

    void ReadChest()
    {
        StoredItem storedItem;
        for (int i = 0; i < chestSize; i++)
        {
            storedItem = storedItems[i];
            if (storedItem.Item)
                ChestUISlots[i].SetItem(storedItem.Item, storedItem.amount);
            else
                ChestUISlots[i].ClearSlot();
            ChestUISlots[i].gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        interactable.OnInteraction -= OpenCloseChest;
        InventoryUIManager.Instance.OnHideInventoryPanel.RemoveListener(Close);
    }
}
