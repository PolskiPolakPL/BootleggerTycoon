using PolskiPolakPL.Utils;
using System;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class ItemScript : MonoBehaviour, IPickable, ISellable
{
    public ItemData itemData;
    [SerializeField] float lifeSpan = 60.0f;
    [SerializeField] float sellValue;

    Timer timer;
    Interactable interactable;

    public event Action OnPickUp;
    private void Awake()
    {
        timer = new Timer(lifeSpan, false);
        timer.OnTimerElapsed += DespawnObject;
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    void Update()
    {
        timer.Tick(Time.deltaTime);
    }

    public void DespawnObject()
    {
        Destroy(gameObject);
    }


    public void PickUp()
    {
        if (!InventorySystem.Instance.AddItem(itemData))
            return;
        OnPickUp?.Invoke();
        DespawnObject();
    }

    private void OnDestroy()
    {
        Debug.Log($"You've picked up '{gameObject.name}'!");
        interactable.OnInteraction -= PickUp;
        timer.OnTimerElapsed -= DespawnObject;
    }

    public void Sell()
    {
        if (GameManager.Instance)
            GameManager.Instance.Player.GainMoney(sellValue);
        Debug.Log($"Sold {gameObject.name} for {sellValue}$ !");
        DespawnObject();
    }
}
