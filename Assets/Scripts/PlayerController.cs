using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterControllerBase
{
    // Player Stats
    public int health = 200;
    public bool isHand = true;
    private bool stop = false;
    private int maxHealth = 200;

    [Header("UI Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI robotText;
    public TextMeshProUGUI noteText;

    public InteractionController interactionController;

    // Maximum values
    private int maxRobots = 3;
    private int maxNotes = 24;

    [Header("Wayfinder Settings")]
    public GameObject wayfinderPrefab; // Assign in the Inspector

    protected override void Start()
    {
        base.Start(); // Ensure base Start() is called

        // Ensure health starts at maxHealth
        health = maxHealth;
        UpdateHealthUI(); // Update UI at the start
    }

    protected override void Update()
    {
        if (stop)
            return;

        base.Update();

        // Update UI elements
        UpdateHealthUI();
        UpdateAmmoUI();
        UpdateRobotUI();
        UpdateNoteUI();

        // Check for game over condition
        CheckGameOver();

        // Handle Wayfinder activation
        if (Input.GetKeyDown(KeyCode.V))
        {
            ActivateWayfinder();
        }
    }

    protected override void MoveCharacter()
    {
        if (cameraTransform == null || controller == null)
        {
            Debug.LogError("PlayerController: cameraTransform or controller is not assigned.");
            return;
        }

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
        if (healthText != null)
        {
            string heartEmoji = "♥";
            int barLength = 20; // Total number of '|' characters
            int filledLength = Mathf.Clamp(Mathf.CeilToInt((float)health / maxHealth * barLength), 0, barLength);
            string healthBar = new string('|', filledLength).PadRight(barLength, ' ');

            healthText.text = $"Health\n{heartEmoji} [{healthBar}] {health}/{maxHealth}";
        }
        else
        {
            Debug.LogWarning("HealthText UI element is not assigned.");
        }
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null && interactionController != null)
        {
            string ammoEmoji = "";
            ammoText.text = $"Ammo\n{ammoEmoji} x {interactionController.ammoCount}";
        }
        else
        {
            if (ammoText == null)
                Debug.LogWarning("AmmoText UI element is not assigned.");
            if (interactionController == null)
                Debug.LogWarning("InteractionController is not assigned.");
        }
    }

    public void UpdateRobotUI()
    {
        if (robotText != null && interactionController != null)
        {
            string robotEmoji = "";
            int robotCount = interactionController.robotCount;

            // Build the robot slots
            string robotSlots = "<";
            for (int i = 0; i < maxRobots; i++)
            {
                robotSlots += i < robotCount ? "|" : " ";
            }
            robotSlots += ">";

            robotText.text = $"Robots\n{robotEmoji} {robotSlots}";
        }
        else
        {
            if (robotText == null)
                Debug.LogWarning("RobotText UI element is not assigned.");
            if (interactionController == null)
                Debug.LogWarning("InteractionController is not assigned.");
        }
    }

    public void UpdateNoteUI()
    {
        if (noteText != null && interactionController != null)
        {
            string noteEmoji = "";
            int barLength = 20; // Total number of '|' characters
            int filledLength = Mathf.Clamp(Mathf.CeilToInt((float)interactionController.noteCount / maxNotes * barLength), 0, barLength);
            string noteBar = new string('|', filledLength).PadRight(barLength, ' ');

            noteText.text = $"Notes\n{noteEmoji} [{noteBar}] {interactionController.noteCount}/{maxNotes}";
        }
        else
        {
            if (noteText == null)
                Debug.LogWarning("NoteText UI element is not assigned.");
            if (interactionController == null)
                Debug.LogWarning("InteractionController is not assigned.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Next"))
        {
            stop = true;
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes to load.");
            }
        }
    }

    private void CheckGameOver()
    {
        if (health <= 0)
        {
            stop = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Display glitchy "DEAD" text
            if (healthText != null)
            {
                string deadText = "DEAD";
                healthText.text = deadText;
            }

            // Delay before loading game over scene to allow player to see the "DEAD" text
            StartCoroutine(LoadGameOverSceneWithDelay(2f)); // 2-second delay
        }
    }

    private System.Collections.IEnumerator LoadGameOverSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(3); // Ensure scene index 3 is correct
    }

    private void ActivateWayfinder()
    {
        if (wayfinderPrefab != null)
        {
            Instantiate(wayfinderPrefab, transform.position, Quaternion.identity);
            Debug.Log("Wayfinder activated.");
        }
        else
        {
            Debug.LogError("Wayfinder prefab is not assigned in PlayerController.");
        }
    }
}
