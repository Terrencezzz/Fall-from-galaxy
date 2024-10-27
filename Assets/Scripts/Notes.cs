using UnityEngine;
using System.IO;

public class Notes : MonoBehaviour
{
    [Tooltip("Enter the name of the text file (including extension) located in Assets/Notes/")]
    public string textFileName;

    [TextArea]
    public string noteContent;

    void OnValidate()
    {
        LoadNoteContent();
    }

    void LoadNoteContent()
    {
        if (!string.IsNullOrEmpty(textFileName))
        {
            string path = Application.dataPath + "/Notes/" + textFileName;
            if (File.Exists(path))
            {
                noteContent = File.ReadAllText(path);
            }
            else
            {
                noteContent = "File not found at path: " + path;
            }
        }
    }
}
