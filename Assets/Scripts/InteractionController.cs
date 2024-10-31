using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class DoorInteraction
{
    public GameObject doorObject;       // The door GameObject in the scene
    public GameObject objectToDelete;   // The associated object to delete (e.g., the door itself)
    public int requiredRobots = 0;      // Robots required to open the door
    public int requiredNotes = 0;       // Notes required to open the door
}

public class InteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public float sphereRadius = 0.5f;
    public LayerMask interactLayerMask;

    [Header("UI Elements")]
    public Canvas mainCanvas;
    public GameObject interactPrompt;               // The GameObject containing the prompt text
    public TextMeshProUGUI interactPromptText;      // The TextMeshPro component displaying the prompt
    public GameObject messagePanel;                 // The panel for displaying messages
    public TextMeshProUGUI messageText;             // The TextMeshPro component displaying messages
    public GameObject notePanel;                    // The panel that displays the note content
    public TextMeshProUGUI noteText;                // The TextMeshPro component displaying the note content

    [Header("Inventory")]
    public int robotCount = 0;
    public int noteCount = 0;
    public int ammoCount = 0;
    public bool hasGun = false;

    [Header("Door Interactions")]
    public List<DoorInteraction> doorInteractions; // List of doors and their requirements

    [Header("Reactor Interaction")]
    public GameObject reactorObject;    // The reactor GameObject in the scene
    public string finishSceneName = "Finish"; // Name of the Finish scene

    // Reference to GunScript
    private GunScript gunScript;

    void Awake()
    {
        // Ensure this object persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initialize Camera
        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("InteractionController: No Camera found in children. Please ensure a Camera component is attached to a child object.");
        }

        // Assign GunScript
        gunScript = GetComponentInChildren<GunScript>();
        if (gunScript == null)
        {
            Debug.LogError("InteractionController: GunScript not found in children.");
        }

        // Ensure all UI elements are assigned
        if (interactPrompt == null || interactPromptText == null)
            Debug.LogError("InteractionController: InteractPrompt or its Text component is not assigned.");
        if (messagePanel == null || messageText == null)
            Debug.LogError("InteractionController: MessagePanel or its Text component is not assigned.");
        if (notePanel == null || noteText == null)
            Debug.LogError("InteractionController: NotePanel or its Text component is not assigned.");

        // Initially disable UI elements
        interactPrompt.SetActive(false);
        messagePanel.SetActive(false);
        notePanel.SetActive(false);
    }

    void Update()
    {
        // Handle key inputs outside of SphereCast interactions
        if (Input.GetKeyDown(KeyCode.V))
        {
            ActivateWayfinder();
        }

        Ray ray = new Ray(transform.position, transform.forward);
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

                case "Gun":
                    ShowInteractPrompt("Press E to pick up gun");
                    if (Input.GetKeyDown(KeyCode.E))
                        CollectGun(hitObject);
                    break;

                case "Door":
                    HandleDoorInteraction(hitObject);
                    break;

                case "Reactor":
                    HandleReactorInteraction(hitObject);
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

    void HandleDoorInteraction(GameObject doorObject)
    {
        DoorInteraction doorInteraction = doorInteractions.Find(d => d.doorObject == doorObject);

        if (doorInteraction != null)
        {
            ShowInteractPrompt("Press E to open door");

            if (Input.GetKeyDown(KeyCode.E))
            {
                bool robotsEnough = robotCount >= doorInteraction.requiredRobots;
                bool notesEnough = noteCount >= doorInteraction.requiredNotes;

                if (robotsEnough && notesEnough)
                {
                    // Requirements met, open the door
                    Destroy(doorInteraction.objectToDelete);
                    DisplayMessage("Door opened", 2f);
                }
                else
                {
                    // Requirements not met, show message
                    int robotsNeeded = doorInteraction.requiredRobots - robotCount;
                    int notesNeeded = doorInteraction.requiredNotes - noteCount;
                    string message = "Collect ";

                    if (robotsNeeded > 0)
                        message += $"{robotsNeeded} more robot(s) ";

                    if (notesNeeded > 0)
                        message += $"{notesNeeded} more log(s) ";

                    message += "to open";

                    DisplayMessage(message, 2f);
                }
            }
        }
        else
        {
            // Door not in the list, treat as a regular door or ignore
            Debug.LogWarning($"InteractionController: Door {doorObject.name} not found in doorInteractions list.");
            ShowInteractPrompt("Press E to open door");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(doorObject);
                DisplayMessage("Door opened", 2f);
            }
        }
    }

    void HandleReactorInteraction(GameObject reactor)
    {
        if (reactor == reactorObject)
        {
            ShowInteractPrompt("Press E to Blow up reactor and destroy ship");

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Perform any required actions before loading the finish scene
                Destroy(reactor);

                // Optionally, play an explosion effect or animation here

                // Load the finish scene
                SceneManager.LoadScene(finishSceneName);
            }
        }
        else
        {
            Debug.LogWarning($"InteractionController: Reactor {reactor.name} does not match the assigned reactorObject.");
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
        DisplayMessage("Robot collected", 2f);
        UpdateInventoryCount("Robot", robotCount);
    }

    void CollectAmmo(GameObject ammoCrate)
    {
        ammoCount += 10; // Assuming each crate gives 10 ammo
        Destroy(ammoCrate);
        Debug.Log("Ammo collected. Total ammo: " + ammoCount);
        DisplayMessage("Ammo collected", 2f);
        UpdateInventoryCount("Ammo", ammoCount);
    }

    void OpenNote(Notes note, GameObject noteObject)
    {
        if (note == null)
        {
            Debug.LogError("InteractionController: Note component missing on the object.");
            return;
        }

        notePanel.SetActive(true);
        noteText.text = note.noteContent;

        // Disable player control
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
            playerController.EnableControl(false);
        else
            Debug.LogError("InteractionController: PlayerController not found.");

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Increment note count and destroy the note object
        noteCount++;
        Destroy(noteObject);
        UpdateInventoryCount("Note", noteCount);
    }

    void CloseNote()
    {
        notePanel.SetActive(false);

        // Enable player control
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
            playerController.EnableControl(true);
        else
            Debug.LogError("InteractionController: PlayerController not found.");

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Display "Note collected" message
        DisplayMessage("Note collected", 2f);
    }

    void CollectGun(GameObject gunObject)
    {
        if (hasGun)
        {
            DisplayMessage("You already have a gun", 2f);
            return;
        }

        hasGun = true;

        if (gunScript != null)
        {
            gunScript.autoGunActive = true;
        }
        else
        {
            Debug.LogError("InteractionController: GunScript is null.");
        }

        Destroy(gunObject);
        Debug.Log("Gun collected.");
        DisplayMessage("Gun collected", 2f);
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

    private void ActivateWayfinder()
    {
        // Functionality to be implemented (handled in PlayerController)
        // Placeholder for future expansions
    }
}
