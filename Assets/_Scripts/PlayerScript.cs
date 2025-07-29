using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] float money = 100;
    [SerializeField] Camera fpsCam;
    [SerializeField] GameObject cameraRigGO;

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

    public void SwitchCameraView(bool fpsActive)
    {

        if (fpsActive)
        {
            cameraRigGO.SetActive(false);
            fpsCam.enabled = true;
        }
        else
        {
            cameraRigGO.SetActive(true);
            fpsCam.enabled = false;
        }
    }
}
