using System.Collections.Generic;
using UnityEngine;

public class CraftingTableScript : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    CraftingSystem craftingSys;
    [field: SerializeField] public List<CraftingRecipe> craftingRecipes { get; private set;} = new List<CraftingRecipe>();
    static GameObject craftingPanel;
    bool isOpen = false;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += OpenCloseCrafting;
    }

    private void Start()
    {
        if(!craftingSys)
            craftingSys = CraftingSystem.Instance;

        if(!craftingPanel)
            craftingPanel = InventoryUIManager.Instance.CraftingUIPanel;
        InventoryUIManager.Instance.OnHideInventoryPanel.AddListener(Close);
    }

    void OpenCloseCrafting()
    {
        if (isOpen)
            Close();
        else
            Open();
        InventoryUIManager.Instance.ToggleInventoryPanel(isOpen);
    }

    void Close()
    {
        if (!isOpen)
            return;
        isOpen = false;
        interactable.message = "Open crafting";
        craftingPanel.SetActive(false);
    }

    void Open()
    {
        isOpen = true;
        interactable.message = "Close crafting";
        craftingSys.RepopulateCraftingContainer(craftingRecipes);
        craftingPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        interactable.OnInteraction -= OpenCloseCrafting;
        InventoryUIManager.Instance.OnHideInventoryPanel.RemoveListener(Close);
    }
}
