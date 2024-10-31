using UnityEngine;
using UnityEngine.AI;

public class Wayfinder : MonoBehaviour
{
    public float speed = 10f;
    public AudioClip beepSound;
    private Transform targetNote;
    private AudioSource audioSource;
    private NavMeshAgent navAgent;

    void Start()
    {
        // Initialize AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // Make it 3D sound
        audioSource.playOnAwake = false;

        if (beepSound != null)
        {
            audioSource.clip = beepSound;
            audioSource.Play();
        }

        // Initialize NavMeshAgent
        navAgent = gameObject.AddComponent<NavMeshAgent>();
        navAgent.speed = speed;
        navAgent.acceleration = 999f; // Instant acceleration
        navAgent.angularSpeed = 999f; // Instant turning
        navAgent.radius = 0.1f; // Small radius
        navAgent.height = 0.1f; // Small height
        navAgent.stoppingDistance = 0.1f;
        navAgent.autoBraking = true;

        // Disable NavMeshAgent control over rotation
        navAgent.updateRotation = false;

        FindNearestNote();
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
            navAgent.SetDestination(targetNote.position);
        }
        else
        {
            Debug.Log("No notes found in the scene.");
            Destroy(gameObject); // Destroy the Wayfinder if no notes are found
        }
    }

    void Update()
    {
        if (targetNote == null)
        {
            Destroy(gameObject);
            return;
        }

        // Check if the Wayfinder has reached the target
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            // Optionally, you can add a delay or an effect here before destroying
            Destroy(gameObject);
        }

        // Rotate the Wayfinder to face the movement direction
        if (navAgent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(navAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
