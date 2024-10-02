using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public float speed = 0.5f;
    public float mouseSensitivity = 300.0f;
    public float jumpForce = 500.0f;
    public float gravityModifyer = 2;
    public float xRotate = 0;
    public bool isGrounded = true;
    public bool isGameOver = false;
    public Rigidbody playerRb;
    public Rigidbody robotRb;
    public bool playerControl;
    public bool robotControl;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        robotRb = GameObject.Find("Robot").GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifyer;
        Cursor.lockState = CursorLockMode.Locked;
        playerControl = true;
        robotControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerControl = !playerControl; 
                robotControl = !robotControl;
            }

            if (playerControl)
            {
                ControlPlayer();
            }
            else if (robotControl)
            {
                ControlRobot();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void ControlPlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        playerRb.transform.Rotate(Vector3.up * mouseX * 2);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.W)) { playerRb.transform.Translate(Vector3.forward * Time.deltaTime * speed); }

        if (Input.GetKey(KeyCode.S)) { playerRb.transform.Translate(Vector3.back * Time.deltaTime * speed); }

        if (Input.GetKey(KeyCode.A)) { playerRb.transform.Translate(Vector3.left * Time.deltaTime * speed); }

        if (Input.GetKey(KeyCode.D)) { playerRb.transform.Translate(Vector3.right * Time.deltaTime * speed); }
    }

    private void ControlRobot()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        robotRb.transform.Rotate(Vector3.up * mouseX * 2);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            robotRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.W)) { robotRb.transform.Translate(Vector3.forward * Time.deltaTime * speed); }

        if (Input.GetKey(KeyCode.S)) { robotRb.transform.Translate(Vector3.back * Time.deltaTime * speed); }

        if (Input.GetKey(KeyCode.A)) { robotRb.transform.Translate(Vector3.left * Time.deltaTime * speed); }

        if (Input.GetKey(KeyCode.D)) { robotRb.transform.Translate(Vector3.right * Time.deltaTime * speed); }
    }
}
