using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input axes
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isJumping = Input.GetButtonDown("Jump");

        // Calculate movement speed
        Vector3 move = new Vector3(moveX, 0, moveZ);
        float speed = move.magnitude;

        // Determine if running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Set animator parameters
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsJumping", !characterController.isGrounded);

        // Movement
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = transform.forward * speed * currentSpeed * Time.deltaTime;
        characterController.Move(moveDirection);
    }
}
