using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement settings
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;

    // Flashlight reference
    public Light flashlight;

    // Internal variables
    private CharacterController controller;
    private Transform cameraTransform;
    private float gravity = -19.81f;
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
        MovePlayer();
        HandleFlashlight();
    }

    void MovePlayer()
    {
        // Get input axes
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Determine speed
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        // Calculate movement direction
        Vector3 move = (cameraTransform.right * moveX + cameraTransform.forward * moveZ).normalized * speed;

        // Apply gravity
        if (controller.isGrounded)
        {
            yVelocity = -0.5f; // Small downward force
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

        // Move the character
        controller.Move(move * Time.deltaTime);
    }

    void HandleFlashlight()
    {
        // Toggle flashlight with 'F' key
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
