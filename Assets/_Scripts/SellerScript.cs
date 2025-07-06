using System;
using UnityEngine;

public class SellerScript : MonoBehaviour
{
    void SellItem(Item item)
    {
        if(GameManager.Instance)
            GameManager.Instance.Player.GainMoney(item.GetSellValue());
        Debug.Log($"Sold {item.gameObject.name} for {item.GetSellValue()}$ !");
        item.DespawnObject();
    }
    private void OnCollisionEnter(Collision other)
    {
        Item item;
        if(other.gameObject.TryGetComponent<Item>(out item))
        SellItem(item);
    }
}
