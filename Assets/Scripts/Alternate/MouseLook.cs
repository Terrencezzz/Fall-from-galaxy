using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        playerBody = transform.parent;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
    }

    void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player body for horizontal movement
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate camera for vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
