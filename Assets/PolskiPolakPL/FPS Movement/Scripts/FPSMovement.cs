using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    private CharacterController controller;


    [Header("Movement")]
    [SerializeField] float baseSpeed = 2.5f;


    [Header("Sprint")]
    public bool CanRun = true;
    [SerializeField] KeyCode runningKey = KeyCode.LeftShift;
    [SerializeField] float runningSpeed = 5;


    [Header("Jump")]
    public bool CanJump = true;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float jumpHeight = 1.0f;

    private Vector3 playerVelocity;
    private float gravityValue = Physics.gravity.y;
    private bool groundedPlayer;
    private bool canControl = true;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0f;
        }

        if (canControl)
        {
            PlayerMove();

            if (Input.GetKey(jumpKey))
                PlayerJump();
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void EnableMovement(bool enabled)
    {
        if (enabled)
            canControl = true;
        else
            canControl = false;
    }

    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float speed = GetMovementSpeed();

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(speed * Time.deltaTime * move);
    }

    void PlayerJump()
    {
        if (!groundedPlayer || !CanJump)
            return;
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    float GetMovementSpeed()
    {
        if (Input.GetKey(runningKey) && CanRun)
            return runningSpeed;
        else
            return baseSpeed;
    }
}
