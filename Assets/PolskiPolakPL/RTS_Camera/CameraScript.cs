using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float polarAngle = 45f;
    [SerializeField] float radialDistance = 20f;

    [Header("Camera Movement")]
    [SerializeField] float movementSpeed = 0.1f;
    [SerializeField] float movementMultiplayer = 1;
    [SerializeField] float rotationSpeed = 2;
    [SerializeField] float movementTime = 5;
    [SerializeField] bool allowRotation = true;
    [SerializeField] bool allowZoom = true;
    [SerializeField] bool freezeOnRotation = false;
    [Header("Camera Zoom")]
    [SerializeField] float minZoomDistance = 3;
    [SerializeField] float maxZoomDistance = 20;
    [SerializeField] float zoomAmount = 5;
    [Header("Camera Border")]
    [SerializeField] bool enableBorderMovement = false;
    [SerializeField] int left = 1;
    [SerializeField] int right = 1;
    [SerializeField] int top = 1;
    [SerializeField] int bottom = 1;

    Vector3 newPosition;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        startPosition = GameManager.Instance.Player.gameObject.transform.position;
        if(cam==null)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = getNewCameraPosition();
        cam.transform.LookAt(transform.position);

        // moving camera
        if (!(Input.GetMouseButton(2) && freezeOnRotation)) // check if not rotating camera with MMB (optional)
        {
            MoveCamera();
        }

        // rotating camera
        if(allowRotation)
            RotateCamera();

        // changing distance (zoom)
        if (Input.mouseScrollDelta.y != 0 && allowZoom)
        {
            radialDistance = Mathf.Clamp(radialDistance - Input.mouseScrollDelta.y * zoomAmount, minZoomDistance, maxZoomDistance);
        }
    }

    private Vector3 getNewCameraPosition()
    {
        float h = radialDistance * Mathf.Sin(polarAngle * Mathf.Deg2Rad);
        float x = Mathf.Sqrt(Mathf.Pow(radialDistance, 2) - Mathf.Pow(h, 2));

        return transform.position - transform.forward * x + Vector3.up * h;
    }

    private void MoveCamera()
    {
        Vector3 mousePosition = Input.mousePosition;
        float multiplayedSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            multiplayedSpeed = movementSpeed * movementMultiplayer;
        }
        else
        {
            multiplayedSpeed = movementSpeed;
        }
        multiplayedSpeed *= Time.deltaTime;

        if ((mousePosition.y >= Screen.height - top && enableBorderMovement) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // up edge detected
        {
            newPosition += (transform.forward * multiplayedSpeed);
        }

        if ((mousePosition.y <= bottom && enableBorderMovement) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) // down edge detected
        {
            newPosition += (transform.forward * -multiplayedSpeed);
        }

        if ((mousePosition.x <= left && enableBorderMovement) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // left edge detected
        {
            newPosition += (transform.right * -multiplayedSpeed);
        }

        if ((mousePosition.x >= Screen.width - right && enableBorderMovement) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // right edge detected
        {
            newPosition += (transform.right * multiplayedSpeed);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, movementTime);
    }

    void RotateCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButton(2)) // MMB
        {
            // horizontally
            if (Input.GetAxis("Mouse X") != 0)
            {
                transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed, 0));
            }

            // vertically
            if (Input.GetAxis("Mouse Y") != 0)
            {
                polarAngle = Mathf.Clamp(polarAngle + Input.GetAxis("Mouse Y") * rotationSpeed, 0, 89.9f);
            }
        }

        if (Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
