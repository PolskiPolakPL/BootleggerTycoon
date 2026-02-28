using System;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class PickableObject : MonoBehaviour
{
    Interactable interactable;
    public event Action OnItemPickUp;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    void PickUp()
    {
        OnItemPickUp?.Invoke();
    }

    private void OnDestroy()
    {
        Debug.Log($"You've picked up '{gameObject.name}'!");
        interactable.OnInteraction -= PickUp;
    }
}
