using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShriekerBehaviour : MonoBehaviour
{
    // For navigation and pathfinding 
    private UnityEngine.AI.NavMeshAgent agent;
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

    // For combat 
    private int health = 15;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        lastPosition = transform.position;
        targetVector = endPoint;
    }

    // Update is called once per frame
    void Update()
    {
        // Death animation 
        if (health <= 0)
        {
            Die();
            animator.SetBool("Dead", true);
        }
        else
        {
            // Calculate speed for animation purposes
            float actualSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
            animationSpeed = actualSpeed / speed;
            animator.SetFloat("Speed", animationSpeed);
            lastPosition = transform.position;

            // FOV code
            InFOV();

            // Navigation code
            if (followPlayer)
            {
                // Follows player (or robot) if within range
                agent.destination = player.transform.position;
                float distance = Vector3.Distance(transform.position, player.transform.position);
                // Attack player if within 3 units 
                if (distance < 3f)
                {
                    // Attack player (if attack animation is not already being played)
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        agent.destination = transform.position; // Stop the NavMeshAgent
                        agent.isStopped = true;
                        animator.SetTrigger("Attack");
                        playerController.TakeDamage(20);
                    }
                }
                else
                {
                    agent.isStopped = false;
                }
            }
            else
            {
                // If player not in range, continue patrol
                agent.destination = targetVector;
                float distance = Vector3.Distance(transform.position, targetVector);
                if (distance < 1f)
                {
                    targetVector = (targetVector == startPoint) ? endPoint : startPoint;
                }
            }
        }
    }

    // Modified from other InFOV - follows the closest robot OR player entity
    void InFOV()
    {
        // Make an array of all objects in radius, find closest. this becomes the target
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 20f)
        {
            followPlayer = true;
        }
        else
        {
            followPlayer = false;
        }
    }

    // Collision detection 
    public void TakeDamage()
    {
        health -= 0;
    }

    // Death logic 
    void Die()
    {
        // Stop navmesh
        agent.isStopped = true;
        speed = 0;
        // Stop physics interactions 
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        // Stop collisi
        GetComponent<Collider>().enabled = false;
    }
}