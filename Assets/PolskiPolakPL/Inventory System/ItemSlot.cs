using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (HasItem())
        {
            itemRawImage.uvRect = InventorySystem.Instance.GetUVRectFromItemArray(heldItem);
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
