using System;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class RedCubeScript : MonoBehaviour, IPickable
{
    Interactable interactable;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    public void PickUp()
    {
        Debug.Log($"Picked up {this.name}!");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log($"You've picked up '{gameObject.name}'!");
        interactable.OnInteraction -= PickUp;
    }
}
