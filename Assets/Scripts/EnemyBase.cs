using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour, EnemyDamage
{
    // Navigation and Pathfinding
    protected NavMeshAgent agent;
    protected GameObject player;
    protected Vector3 targetVector;
    public Vector3 startPoint;
    public Vector3 endPoint;
    protected bool followPlayer = false;

    // Animation
    protected Animator animator;
    public float speed;
    protected Vector3 lastPosition;
    protected float animationSpeed;

    // Combat
    protected int health;
    protected int maxHealth;
    protected int damageToPlayer;
    public PlayerController playerController;

    // Enemy Settings
    public float detectionRange = 12f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        lastPosition = transform.position;
        targetVector = endPoint;
        health = maxHealth;
        agent.speed = speed;
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            Die();
            animator.SetBool("Dead", true);
        }
        else
        {
            UpdateMovementAnimation();
            InFOV();

            if (followPlayer)
                FollowPlayer();
            else
                Patrol();
        }
    }

    protected void UpdateMovementAnimation()
    {
        float actualSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        animationSpeed = actualSpeed / speed;
        animator.SetFloat("Speed", animationSpeed);
        lastPosition = transform.position;
    }

    protected virtual void InFOV()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        followPlayer = distance < detectionRange;
    }

    protected virtual void FollowPlayer()
    {
        agent.destination = player.transform.position;
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < attackRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            agent.isStopped = true;
            animator.SetTrigger("Attack");
            StartCoroutine(AttackPlayer());
        }
    }

    protected virtual void Patrol()
    {
        agent.destination = targetVector;
        float distance = Vector3.Distance(transform.position, targetVector);

        if (distance < 1f)
            targetVector = (targetVector == startPoint) ? endPoint : startPoint;
    }

    public virtual void TakeDamage()
    {
        health -= 1;
    }

    protected virtual void Die()
    {
        agent.isStopped = true;
        speed = 0;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    protected virtual IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(attackCooldown);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < attackRange)
            playerController.TakeDamage(damageToPlayer);
        agent.isStopped = false;
    }
}
