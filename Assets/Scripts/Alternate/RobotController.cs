using UnityEngine;

public class RobotController : MonoBehaviour
{
    // Movement settings
    public float walkSpeed = 3f;
    public float sprintSpeed = 5f;
    public float jumpHeight = 1.5f;

    // Flashlight reference
    public Light flashlight;

    // Internal variables
    private CharacterController controller;
    private Transform cameraTransform;
    private float gravity = -9.81f;
    private float yVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;

        // Find the flashlight if not assigned
        if (flashlight == null)
            flashlight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        MoveRobot();
        HandleFlashlight();
    }

    void MoveRobot()
    {
        // Same movement logic as the player
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = (cameraTransform.right * moveX + cameraTransform.forward * moveZ).normalized * speed;

        if (controller.isGrounded)
        {
            yVelocity = -0.5f;
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        move.y = yVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
