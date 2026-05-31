using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool hovering {  get; private set; }

    // Item Data
    [SerializeField] ItemData heldItem;
    int itemAmount;

    // UI Elements
    [field: Header("UI Elements")]
    [field: SerializeField] public Image bgImage {  get; private set; }
    [SerializeField] RawImage itemRawImage;
    [SerializeField] TMP_Text amountTextField;

    #region Debugging

    Color bgColor;
    [SerializeField] Color hoveringColor;
    private void Awake()
    {
        bgColor = bgImage.color;
    }

    #endregion

    public ItemData GetItem()
    {
        return heldItem;
    }

    public int GetAmount()
    {
        return itemAmount;
    }

    public void SetItem(ItemData item, int amount = 1)
    {
        heldItem = item;
        itemAmount = amount;
        itemRawImage.texture = item.ImageTexture;
        itemRawImage.uvRect = item.UVRect;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (HasItem())
        {
            itemRawImage.enabled = true;
            if (itemAmount > 1)
                amountTextField.text = itemAmount.ToString();
            else
                amountTextField.text = "";
        }
        else
        {
            itemRawImage.enabled = false;
            amountTextField.text = "";
        }
    }

    public int AddAmount(int amount)
    {
        itemAmount += amount;
        UpdateSlot();
        return itemAmount;
    }

    public int RemoveAmount(int amount)
    {
        itemAmount -= amount;
        if(itemAmount <= 0)
            ClearSlot();
        else
            UpdateSlot();

        return itemAmount;
    }

    public void ClearSlot()
    {
        heldItem = null;
        itemAmount = 0;
        UpdateSlot();
    }

    public bool HasItem()
    {
        return heldItem != null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        bgImage.color = hoveringColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        bgImage.color = bgColor;
    }

    private void OnDisable()
    {
        hovering = false;
        bgImage.color = bgColor;
    }
}
