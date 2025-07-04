using UnityEngine;

public class SellerScript : MonoBehaviour
{

    void SellItem(Item item)
    {
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
