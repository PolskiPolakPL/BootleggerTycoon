using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    [SerializeField] float speed = 30;
    private void OnCollisionStay(Collision other)
    {
        Rigidbody rigidbody;
        if(other.gameObject.TryGetComponent<Rigidbody>(out rigidbody))
        {
            rigidbody.velocity = speed * Time.deltaTime * transform.forward;
        }
    }
}
