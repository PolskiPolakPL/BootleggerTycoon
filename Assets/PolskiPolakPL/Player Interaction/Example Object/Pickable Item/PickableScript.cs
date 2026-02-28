using System;
using UnityEngine;

public class PickableScript : MonoBehaviour
{
    Interactable interactable;
    public ItemSO itemSO;
    public event Action<ItemSO> OnItemPickUp;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    void PickUp()
    {
        OnItemPickUp?.Invoke(itemSO);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log($"You've picked up '{gameObject.name}'!");
        interactable.OnInteraction -= PickUp;
    }
}
