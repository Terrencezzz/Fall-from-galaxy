using UnityEngine;

public class Note : MonoBehaviour
{
    [Tooltip("Path to the text file in Assets/Notes/")]
    public string noteFileName;

    private string noteContent;

    void Start()
    {
        LoadNoteContent();
    }

    void LoadNoteContent()
    {
        // Load the text file from Resources folder
        TextAsset textAsset = Resources.Load<TextAsset>("Notes/" + noteFileName);
        if (textAsset != null)
        {
            noteContent = textAsset.text;
        }
        else
        {
            Debug.LogError("Note text file not found at path: Resources/Notes/" + noteFileName);
        }
    }

    public string GetNoteContent()
    {
        return noteContent;
    }
}
