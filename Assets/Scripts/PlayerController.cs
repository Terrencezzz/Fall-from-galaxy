using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterControllerBase
{
    // Player Stats
    public int health = 100;
    public bool isHand = true;
    private bool stop = false;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI LogText;
    public TextMeshProUGUI AmmoText;

    public InteractionController interactionController;

    protected override void Update()
    {
        if (stop)
            return;

        base.Update();
        UpdateHealthUI();
        UpdateLogText();
        UpdateAmmoText();
    }

    protected override void MoveCharacter()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isHand;
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = (cameraTransform.right * moveX + cameraTransform.forward * moveZ).normalized * speed;

        if (controller.isGrounded)
        {
            yVelocity = -0.5f;
            if (Input.GetButtonDown("Jump"))
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        move.y = yVelocity;
        controller.Move(move * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Next"))
        {
            stop = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player health: " + health);
    }

    public void UpdateHealthUI()
    {
        healthText.text = "Health: " + (100 + health).ToString();
    }

    public void UpdateLogText()
    {
        LogText.text = "Note collected: " + interactionController.noteCount.ToString();
    }

    public void UpdateAmmoText()
    {
        AmmoText.text = "Ammo: " + interactionController.ammoCount.ToString();
    }
}
