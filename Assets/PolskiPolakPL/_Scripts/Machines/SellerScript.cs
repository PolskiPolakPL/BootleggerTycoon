using UnityEngine;

public class SellerScript : MonoBehaviour
{
    ISellable sellable;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<ISellable>(out sellable))
        sellable.Sell();
    }
}
