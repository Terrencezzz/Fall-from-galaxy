using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public Transform Camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 2;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);
        Camera.Rotate(Vector3.left * mouseY);
    }
}
