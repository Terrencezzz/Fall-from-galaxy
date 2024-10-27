using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public Transform Camera;
    public Camera playerCamera;
    public Camera robotCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera.enabled = true;
        robotCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerCamera.enabled = !playerCamera.enabled;
            robotCamera.enabled = !robotCamera.enabled;
        }

        if (playerCamera)
        {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 2;
            mouseY = Mathf.Clamp(mouseY, -80f, 80f);
            Camera.Rotate(Vector3.left * mouseY);
        }
    }
}
