using PolskiPolakPL.Utils;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class ItemScript : MonoBehaviour, IPickable
{
    [SerializeField] float lifeSpan = 60.0f;
    [SerializeField] float sellValue;

    Timer timer;
    Interactable interactable;

    private void Awake()
    {
        timer = new Timer(lifeSpan, false);
        timer.OnTimerElapsed += DespawnObject;
        interactable = GetComponent<Interactable>();
        interactable.OnInteraction += PickUp;
    }

    // Update is called once per frame
    void Update()
    {
        timer.Tick(Time.deltaTime);
    }

    public float GetSellValue()
    {
        return sellValue;
    }

    public void ChangeSellValue(float newSellValue)
    {
        if (newSellValue < 0)
            return;
        sellValue = newSellValue;
    }

    public void DespawnObject()
    {
        Destroy(gameObject);
    }

    public void PickUp()
    {
        Debug.Log($"You've picked up '{gameObject.name}'!");
        DespawnObject();
    }

    private void OnDestroy()
    {
        interactable.OnInteraction -= PickUp;
        timer.OnTimerElapsed -= DespawnObject;
    }
}
