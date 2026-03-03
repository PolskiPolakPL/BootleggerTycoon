using UnityEngine;

public class SellerScript : MonoBehaviour
{
    void SellItem(ItemScript item)
    {
        if(GameManager.Instance)
            GameManager.Instance.Player.GainMoney(item.GetSellValue());
        Debug.Log($"Sold {item.gameObject.name} for {item.GetSellValue()}$ !");
        item.DespawnObject();
    }
    private void OnCollisionEnter(Collision other)
    {
        ItemScript item;
        if(other.gameObject.TryGetComponent<ItemScript>(out item))
        SellItem(item);
    }
}
