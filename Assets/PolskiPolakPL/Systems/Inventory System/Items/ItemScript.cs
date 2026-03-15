using System;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class ItemScript : MonoBehaviour, IPickable
{
    public ItemData itemData;
    Interactable interactable;

    public event Action OnPickUp;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    public void PickUp()
    {
        if (!InventorySystem.Instance.AddItem(itemData))
            return;
        OnPickUp?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        interactable.OnInteraction -= PickUp;
    }
}
