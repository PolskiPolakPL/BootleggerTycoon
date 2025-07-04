using UnityEngine;

public class FPSLook : MonoBehaviour
{

    [SerializeField] Camera playerCamera;
    [SerializeField] float mouseSensitivity = 1f;
    public bool invertYAxis = false;
    public bool invertXAxis = false;
    private float xRotation = 0f;

    void Awake()
    {
        if(!playerCamera)
            playerCamera = Camera.main;
    }

    void Update()
    {
        Look();
    }

    void Look()
    {
        // Read mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        if(invertXAxis)
            mouseX = -mouseX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (invertYAxis)
            mouseY = -mouseY;

        // Rotate the player's body left and right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera up and down (clamping to 90 degrees)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
