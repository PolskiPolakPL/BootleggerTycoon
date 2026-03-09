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

    Rect GetUVRectFromItemArray(int itemArraySize)
    {
        float stepSize = 1f / itemArraySize;
        int rowIndex = heldItem.ID / itemArraySize;
        int columnIndex = heldItem.ID % itemArraySize;
        float x = stepSize * columnIndex;
        float y = stepSize * rowIndex;
        Debug.Log($"X: {x} \t Y: {y} \t stepSize: {stepSize}");
        return new Rect(x, y, stepSize, stepSize);
    }

}
