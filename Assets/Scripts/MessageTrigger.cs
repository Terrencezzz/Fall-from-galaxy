// MessageTrigger.cs

using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [Header("Message Settings")]
    [TextArea]
    public string messageText;
    public Color fontColor = Color.white;
    public int fontSize = 24;
    public Vector2 position = new Vector2(0.5f, 0.5f); // Normalized position (0 to 1)
    public float displayDuration = 5f; // Time to display the message

    private bool messageDisplayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (messageDisplayed)
            return;

        if (other.CompareTag("Player"))
        {
            messageDisplayed = true;
            // GameManager.Instance.DisplayMessage(this);
        }
    }
}
