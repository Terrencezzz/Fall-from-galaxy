using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    // Movement Settings
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpHeight;

    // Flashlight Reference
    public Light flashlight;

    // Internal Variables
    protected CharacterController controller;
    protected Transform cameraTransform;
    protected float gravity = -19.81f;
    protected float yVelocity = 0f;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;

        if (flashlight == null)
            flashlight = GetComponentInChildren<Light>();
    }

    protected virtual void Update()
    {
        MoveCharacter();
        HandleFlashlight();
    }

    protected abstract void MoveCharacter();

    protected virtual void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
            flashlight.enabled = !flashlight.enabled;
    }

    public virtual void EnableControl(bool enable)
    {
        enabled = enable;
        var animator = GetComponentInChildren<Animator>();
        if (animator != null)
            animator.enabled = enable;

        var cam = GetComponentInChildren<Camera>();
        if (cam != null)
            cam.enabled = enable;

        var audioListener = GetComponentInChildren<AudioListener>();
        if (audioListener != null)
            audioListener.enabled = enable;

        var mouseLook = GetComponentInChildren<MouseLook>();
        if (mouseLook != null)
            mouseLook.enabled = enable;
    }
}
