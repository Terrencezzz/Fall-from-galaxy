using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public float sphereRadius = 0.5f;
    public LayerMask interactLayerMask;

    // UI Elements
    [Header("UI Elements")]
    public Canvas mainCanvas;
    public GameObject interactPrompt;       // The GameObject containing the prompt text
    public TextMeshProUGUI interactPromptText;  // The TextMeshPro component displaying the prompt
    public GameObject notePanel;            // The panel that displays the note content
    public TextMeshProUGUI noteText;        // The TextMeshPro component displaying the note content
    public GameObject messagePanel;         // The panel for displaying messages
    public TextMeshProUGUI messageText;     // The TextMeshPro component displaying messages

    // Internal variables
    private bool isReadingNote = false;
    private Camera cam;

    // Inventory counts
    private int robotCount = 0;
    private int noteCount = 0;
    private int ammoCount = 0;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.Log("InteractionController: Camera not found in children.");
        }

        // Ensure all UI elements are assigned
        if (mainCanvas == null)
            Debug.Log("InteractionController: MainCanvas is not assigned.");
        if (interactPrompt == null || interactPromptText == null)
            Debug.Log("InteractionController: InteractPrompt or its Text component is not assigned.");
        if (notePanel == null || noteText == null)
            Debug.Log("InteractionController: NotePanel or its Text component is not assigned.");
        if (messagePanel == null || messageText == null)
            Debug.Log("InteractionController: MessagePanel or its Text component is not assigned.");

        // Initially disable UI elements
        interactPrompt.SetActive(false);
        notePanel.SetActive(false);
        messagePanel.SetActive(false);
    }

    void Update()
    {
        if (isReadingNote)
        {
            if (Input.GetKeyDown(KeyCode.E))
                CloseNote();
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRadius, out hit, interactDistance, interactLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            string tag = hitObject.tag;

            switch (tag)
            {
                case "Robot":
                    ShowInteractPrompt("Press E to collect robot");
                    if (Input.GetKeyDown(KeyCode.E))
                        CollectRobot(hitObject);
                    break;

                case "Note":
                    ShowInteractPrompt("Press E to read note");
                    if (Input.GetKeyDown(KeyCode.E))
                        OpenNote(hitObject.GetComponent<Notes>(), hitObject);
                    break;

                case "AmmoCrate":
                    ShowInteractPrompt("Press E to collect ammo");
                    if (Input.GetKeyDown(KeyCode.E))
                        CollectAmmo(hitObject);
                    break;

                case "Reactor":
                    ShowInteractPrompt("Press E to interact");
                    if (Input.GetKeyDown(KeyCode.E))
                        InteractWithReactor();
                    break;

                default:
                    HideInteractPrompt();
                    break;
            }
        }
        else
        {
            HideInteractPrompt();
        }
    }

    void ShowInteractPrompt(string message)
    {
        if (interactPrompt != null && interactPromptText != null)
        {
            interactPrompt.SetActive(true);
            interactPromptText.text = message;
        }
    }

    void HideInteractPrompt()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void CollectRobot(GameObject robot)
    {
        robotCount++;
        Destroy(robot);
        Debug.Log("Robot collected. Total robots: " + robotCount);
        DisplayMessage("Robot collected");
        UpdateInventoryCount("Robot", robotCount);
    }

    void CollectAmmo(GameObject ammoCrate)
    {
        ammoCount += 10; // Assuming each crate gives 10 ammo
        Destroy(ammoCrate);
        Debug.Log("Ammo collected. Total ammo: " + ammoCount);
        DisplayMessage("Ammo collected");
        UpdateInventoryCount("Ammo", ammoCount);
    }

    void OpenNote(Notes note, GameObject noteObject)
    {
        if (note == null)
        {
            Debug.Log("InteractionController: Note component missing on the object.");
            return;
        }

        isReadingNote = true;

        if (notePanel != null && noteText != null)
        {
            notePanel.SetActive(true);
            noteText.text = note.noteContent;
        }

        HideInteractPrompt();

        // Disable player control
        var playerController = GetComponentInParent<CharacterControllerBase>();
        if (playerController != null)
            playerController.EnableControl(false);

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Destroy the note object (collecting it)
        noteCount++;
        Destroy(noteObject);
        UpdateInventoryCount("Note", noteCount);
    }

    void CloseNote()
    {
        isReadingNote = false;

        if (notePanel != null)
            notePanel.SetActive(false);

        // Enable player control
        var playerController = GetComponentInParent<CharacterControllerBase>();
        if (playerController != null)
            playerController.EnableControl(true);

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Display "Note collected" message
        DisplayMessage("Note collected", 2f);
    }

    void InteractWithReactor()
    {
        Debug.Log("Interacting with reactor...");
        DisplayMessage("Interacting with reactor...");
        // Load the next scene or perform other actions
        SceneManager.LoadScene("ReactorScene"); // Replace with your scene name
    }

    void DisplayMessage(string message, float duration = 2f)
    {
        if (messagePanel != null && messageText != null)
        {
            messagePanel.SetActive(true);
            messageText.text = message;
            CancelInvoke("HideMessage");
            Invoke("HideMessage", duration);
        }
    }

    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void UpdateInventoryCount(string itemType, int count)
    {
        // Update your inventory UI or data here
        Debug.Log($"{itemType} count updated: {count}");
    }
}
