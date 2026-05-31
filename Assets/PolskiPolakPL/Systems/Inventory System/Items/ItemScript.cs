using System;
using UnityEngine;

public class ItemScript : MonoBehaviour, IPickable
{
    Interactable interactable;
    public event Action OnPickUp;


    public ItemData itemData;
    [Min(1)] public int amount = 1;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    public void PickUp()
    {
        if (!InventorySystem.Instance.AddItem(itemData, amount))
            return;
        OnPickUp?.Invoke();
        Debug.Log($"You've picked up '{itemData.DisplayName}'!");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        interactable.OnInteraction -= PickUp;
    }

    private void OnCollisionEnter(Collision other)
    {
        // Only (world) Items
        if (!other.collider.TryGetComponent<ItemScript>(out ItemScript otherItemScr))
            return;

        // Only same type of item
        if (otherItemScr.itemData != itemData)
            return;

        // Only one of them executes merge
        if (GetInstanceID() > otherItemScr.GetInstanceID())
            return;

        TryMerge(otherItemScr);
    }

    void TryMerge(ItemScript otherItemScr)
    {
        int mergeAmount = amount + otherItemScr.amount;
        if(mergeAmount <= itemData.StackSize)
        {
            amount = mergeAmount;
            Destroy(otherItemScr.gameObject);
        }
    }
}
