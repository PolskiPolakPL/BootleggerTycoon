using UnityEngine;

public class RotateObjects : MonoBehaviour
{
    [SerializeField] Vector3 rotateDirection = Vector3.up;
    

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (Transform child in transform)
        {
            child.Rotate(rotateDirection* Time.deltaTime);
        }
    }
}
