using UnityEngine;

public class Wayfinder : MonoBehaviour
{
    public float speed = 10f;
    public AudioClip beepSound;
    private Transform targetNote;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (beepSound != null)
        {
            audioSource.clip = beepSound;
            audioSource.Play();
        }

        FindNearestNote();
    }

    void Update()
    {
        if (targetNote == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target note
        transform.position = Vector3.MoveTowards(transform.position, targetNote.position, speed * Time.deltaTime);

        // Optional: Rotate towards the target
        Vector3 direction = (targetNote.position - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        // Destroy when reached the target
        if (Vector3.Distance(transform.position, targetNote.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void FindNearestNote()
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        float shortestDistance = Mathf.Infinity;
        Transform nearestNote = null;

        foreach (GameObject note in notes)
        {
            float distance = Vector3.Distance(transform.position, note.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestNote = note.transform;
            }
        }

        if (nearestNote != null)
        {
            targetNote = nearestNote;
        }
        else
        {
            Debug.Log("No notes found in the scene.");
            Destroy(gameObject);
        }
    }
}
