using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Enemy Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float boxrange;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Idle Behaviour")]
    private Animator anim;
    private PlayerHealth playerHealth;
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Patrol Settings")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform enemy;
    [SerializeField] private float patrolSpeed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Chase and Platform Settings")]
    [SerializeField] private float detectionRange;
    [SerializeField] private bool isGroundEnemy;
    [SerializeField] private Transform highPlatformPosition;
    private bool isChasing;

    private Transform player;

    private static List<EnemyHealth> activeEnemies = new List<EnemyHealth>();


    [Header("Chase and Platform Settings")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDelay;
    [SerializeField] private Rigidbody2D rb;
    public UnityEvent OnBEgin, OnDone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initScale = enemy?.localScale ?? Vector3.one;
        if (enemy == null)
        {
            Debug.LogError("Enemy Transform is not assigned in the Inspector!");
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        activeEnemies.Add(this);
    }

    private void Update()
    {

        if (player == null)
        {
            DetectPlayer();
            if (!isChasing) Patrol();
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            //Debug.Log($"Distance to Player: {distanceToPlayer}");

            if (distanceToPlayer <= range && CanAttack())
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
                //Debug.Log("Enemy attacks player.");
                DamagePlayer();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                //Debug.Log("Chasing player.");
                ChasePlayer();
            }
            else
            {
                //Debug.Log("Returning to patrol.");
                ResetToPatrol();
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x,
            new Vector3(boxCollider.bounds.size.x * boxrange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.right, 0, playerLayer);

        if (hit.collider != null)
        {
            //Debug.Log("Player detected in attack range.");
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        }
        else
        {
            //Debug.Log("Player not in attack range.");
        }

        return hit.collider != null;
    }


    private void Patrol()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ChangeDirection();
            }
        }
    }

    private void DetectPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.transform;
                break;
            }
        }
    }

    private void ChasePlayer()
    {
        isChasing = true;
        anim.SetInteger("AnimState", 2); // Walking animation
        if (!isGroundEnemy && transform.position.y > player.position.y)
        {
            MoveTowards(player.position, patrolSpeed);
        }
        else
        {
            MoveTowards(player.position, patrolSpeed);
        }

        ManageMultipleEnemies();
    }

    private void ResetToPatrol()
    {
        isChasing = false;
        player = null;
        anim.SetInteger("AnimState", 0); // Idle animation

        if (!isGroundEnemy && highPlatformPosition != null)
        {
            MoveTowards(highPlatformPosition.position, patrolSpeed);
        }
        else
        {
            Patrol();
        }
    }

    private void ManageMultipleEnemies()
    {
        foreach (EnemyHealth enemy in activeEnemies)
        {
            if (enemy == this) continue;

            float otherDistance = Vector2.Distance(enemy.transform.position, player.position);
            if (otherDistance < detectionRange && !enemy.isChasing)
            {
                isChasing = false;
                break;
            }
        }
    }

    /*private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
    

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
                                             new Vector3(boxCollider.bounds.size.x * boxrange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                                             0, Vector2.right, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        }

        return hit.collider != null;
    }
    */
    private void MoveInDirection(int direction)
    {
        idleTimer = 0;
        anim.SetInteger("AnimState", 2); // Walking animation
        enemy.localScale = new Vector3(initScale.x * direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * patrolSpeed, enemy.position.y, enemy.position.z);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        enemy.position += direction * speed * Time.deltaTime;
        enemy.localScale = new Vector3(direction.x > 0 ? initScale.x : -initScale.x, initScale.y, initScale.z);
    }

    private void ChangeDirection()
    {
        anim.SetInteger("AnimState", 0); // Idle animation
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            Debug.Log("Player is in sight. Attacking.");
            playerHealth.TakeDamage(damage);
        }
        else
        {
            Debug.Log("Player not in sight. Cannot attack.");
        }
    }

    private bool CanAttack()
    {
        //Debug.Log($"Cooldown Timer: {cooldownTimer}, Attack Cooldown: {attackCooldown}");
        return cooldownTimer >= attackCooldown;
    }


    private void OnDestroy()
    {
        activeEnemies.Remove(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
                            new Vector3(boxCollider.bounds.size.x * boxrange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current Health: {currentHealth}");

        // Knockback in horizontal direction only
        Vector2 constrainedHitDirection = new Vector2(hitDirection.x, 0).normalized;
        ApplyKnockback(constrainedHitDirection);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void ApplyKnockback(Vector2 hitDirection)
    {
        if (rb != null)
        {
            // Constrain knockback to the horizontal direction only
            Vector2 knockback = new Vector2(-hitDirection.x, 0).normalized * knockbackForce;
            rb.velocity = knockback; // Apply the knockback force
            Debug.Log($"Knockback applied: {knockback}");
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the enemy!");
        }
    }
    /*
    private IEnumerator KnockbackCoroutine(Vector2 knockback)
    {
        rb.velocity = knockback;
        yield return new WaitForSeconds(knockbackDelay);
        rb.velocity = Vector2.zero; // Reset velocity after delay
    }
    
    private void ApplyKnockback(Vector2 hitDirection)
    {
        if (rb != null)
        {
            Vector2 knockback = new Vector2(hitDirection.x, 0).normalized * knockbackForce;
            StartCoroutine(KnockbackCoroutine(knockback));
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the enemy!");
        }
    }


    
    private IEnumerator KnockbackDelay()
    {
        yield return new WaitForSeconds(knockbackDelay); // Remove this temporarily to test knockback.
        rb.velocity = Vector2.zero; // Ensure this isn't stopping the effect immediately.
    }
    */


    private void Die()
    {
        Debug.Log("Enemy died.");

        // Play death effect if assigned
        /*
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        */

        // Destroy the enemy object
        Destroy(gameObject);
    }

}
