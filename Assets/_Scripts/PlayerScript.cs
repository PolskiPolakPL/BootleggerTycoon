using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] float money = 100;

    public event Action OnPlayerGainMoney;
    public event Action OnPlayerLooseMoney;

    public float GetMoney()
    {
        return money;
    }

    public void GainMoney(float amount)
    {
        money += amount;
        OnPlayerGainMoney?.Invoke();
    }

    public void LooseMoney(float amount)
    {
        money -= amount;
        OnPlayerLooseMoney?.Invoke();
    }
}
