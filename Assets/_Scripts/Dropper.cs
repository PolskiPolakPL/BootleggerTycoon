using PolskiPolakPL.Utils;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] GameObject itemGO;
    [SerializeField] Transform spawnerT;
    [SerializeField] float dropTime = 1f;

    Timer timer;

    private void Awake()
    {
        timer = new Timer(dropTime);
        timer.OnTimerElapsed += DropItem;
    }

    void DropItem()
    {
        Instantiate(itemGO,spawnerT.position, Quaternion.identity);
    }

    private void Update()
    {
        timer.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        timer.OnTimerElapsed -= DropItem;
    }
}
