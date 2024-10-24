using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public float interactDistance = 3f;
    public float sphereRadius = 0.5f;
    public LayerMask interactLayerMask;

    // UI Elements
    public GameObject interactPrompt;
    public GameObject noteCanvas;
    public TextMeshProUGUI noteText;

    // Internal variables
    private bool isReadingNote = false;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        interactPrompt.SetActive(false);
        noteCanvas.SetActive(false);
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
            Notes note = hit.collider.GetComponent<Notes>();
            if (note != null)
            {
                interactPrompt.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                    OpenNote(note);
            }
            else
            {
                interactPrompt.SetActive(false);
            }
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    void OpenNote(Notes note)
    {
        isReadingNote = true;
        noteCanvas.SetActive(true);
        noteText.text = note.noteContent;
        interactPrompt.SetActive(false);

        // Disable player control
        var playerController = transform.parent.GetComponent<CharacterControllerBase>();
        if (playerController != null)
            playerController.EnableControl(false);

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseNote()
    {
        isReadingNote = false;
        noteCanvas.SetActive(false);

        // Enable player control
        var playerController = transform.parent.GetComponent<CharacterControllerBase>();
        if (playerController != null)
            playerController.EnableControl(true);

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
