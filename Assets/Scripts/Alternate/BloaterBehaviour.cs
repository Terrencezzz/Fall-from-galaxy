using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BloaterBehaviour : MonoBehaviour
{
    // For navigation and pathfinding 
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 targetVector;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private bool followPlayer = false;

    // For animation
    private Animator animator;
    public float speed;
    private Vector3 lastPosition;
    private float animationSpeed;

    // Enemy statistics 
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        lastPosition = transform.position;
        targetVector = endPoint;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcualte speed for animation purposes
        float actualSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        animationSpeed = actualSpeed / speed;
        animator.SetFloat("Speed", animationSpeed);
        lastPosition = transform.position;

        // FOV code
        InFOV();

        // Navigation code
        if (followPlayer) {
            agent.destination = player.transform.position;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            // Attack player if within 3 units 
            if (distance < 3f) {
                // insert code here to reduce players' health 
                animator.SetBool("Attack", true);
            } else {
                animator.SetBool("Attack", false);
            }
        } else {
            // If player not in range, continue patrol
            agent.destination = targetVector;
            float distance = Vector3.Distance(transform.position, targetVector);
            if (distance < 1f) {
                targetVector = (targetVector == startPoint) ? endPoint : startPoint;
            }
        }
    }

    void InFOV() {
        // Check player is in enemy FOV
        // Vector3 bloaterForward = transform.forward;
        // Vector3 playerForward = player.transform.forward;
        // Vector3 directionBetween = (player.transform.position - transform.position).normalized;
        // float dotProduct1 = Vector3.Dot(bloaterForward, directionBetween);
        // float dotProduct2 = Vector3.Dot(playerForward, -directionBetween);
        // float threshold = 15f; // Adjust for desired accuracy
        // bool arePointingAtEachOther = (dotProduct1 > threshold && dotProduct2 > threshold);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 12f) {
            followPlayer = true;
        } else {
            followPlayer = false;
        }

    }
}
