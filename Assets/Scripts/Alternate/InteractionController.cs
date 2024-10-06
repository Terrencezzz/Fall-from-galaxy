using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactDistance = 3f;
    public float sphereRadius = 0.5f; // Adjust this radius as needed
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
            {
                CloseNote();
            }
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
                {
                    OpenNote(note);
                }
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

    void OpenNote(Notes note)  // Changed to Notes
    {
        isReadingNote = true;
        noteCanvas.SetActive(true);
        noteText.text = note.noteContent;  // Use noteContent from Notes class
        interactPrompt.SetActive(false);

        // Disable player movement and mouse look
        transform.parent.GetComponent<PlayerController>().enabled = false;
        GetComponent<MouseLook>().enabled = false;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void CloseNote()
    {
        isReadingNote = false;
        noteCanvas.SetActive(false);

        // Enable player movement and mouse look
        transform.parent.GetComponent<PlayerController>().enabled = true;
        GetComponent<MouseLook>().enabled = true;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
