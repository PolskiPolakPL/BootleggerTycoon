using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] ItemData heldItem;
    [SerializeField] RawImage itemRawImage;

    public ItemData GetItemData()
    {
        return heldItem;
    }

    public void SetItem(ItemData item)
    {
        heldItem = item;
        itemRawImage.uvRect = InventorySystem.Instance.GetUVRectFromItemArray(item);
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (HasItem())
        {
            itemRawImage.enabled = true;
        }
        else
        {
            itemRawImage.enabled = false;
        }
    }

    public void ClearSlot()
    {
        heldItem = null;
        UpdateSlot();
    }

    public bool HasItem()
    {
        return heldItem != null;
    }

}
