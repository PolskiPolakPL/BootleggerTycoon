using PolskiPolakPL.Utils;
using UnityEngine;

public class ProductScript : MonoBehaviour, ISellable
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
    public void Sell()
    {
        if (GameManager.Instance)
            GameManager.Instance.Player.GainMoney(sellValue);
        Debug.Log($"Sold {gameObject.name} for {sellValue}$ !");
        DespawnObject();
    }

    public void DespawnObject()
    {
        Destroy(gameObject);
    }
}
