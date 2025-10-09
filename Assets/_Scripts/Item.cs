using PolskiPolakPL.Utils;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float lifeSpan = 60.0f;
    [SerializeField] float sellValue;

    Timer timer;

    private void Awake()
    {
        timer = new Timer(lifeSpan, false);
        timer.OnTimerElapsed += DespawnObject;
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

    private void OnDestroy()
    {
        timer.OnTimerElapsed -= DespawnObject;
    }
}
