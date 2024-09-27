using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 300.0f;
    public float jumpForce = 500.0f;
    public float gravityModifyer = 2;
    public float xRotate = 0;
    public bool isGrounded = true;
    public bool isGameOver = false;
    public Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifyer;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            playerRb.transform.Rotate(Vector3.up * mouseX * 2);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            if (Input.GetKey(KeyCode.W)) { transform.Translate(Vector3.forward * Time.deltaTime * speed); }

            if (Input.GetKey(KeyCode.S)) { transform.Translate(Vector3.back * Time.deltaTime * speed); }

            if (Input.GetKey(KeyCode.A)) { transform.Translate(Vector3.left * Time.deltaTime * speed); }

            if (Input.GetKey(KeyCode.D)) { transform.Translate(Vector3.right * Time.deltaTime * speed); }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
