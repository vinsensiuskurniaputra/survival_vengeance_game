using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float stoppingDistance = 1.5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    public int damage = 1;
    public LayerMask playerLayer;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 originalScale;
    private float lastAttackTime;

    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && distance > stoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            movement = Vector2.zero;
            if (animator != null)
                animator.SetBool("IsMoving", false);
        }

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;

        // Flip boss sprite
        if (direction.x != 0)
        {
            Vector3 newScale = originalScale;
            newScale.x = Mathf.Abs(originalScale.x) * (direction.x > 0 ? 1 : -1);
            transform.localScale = newScale;
        }

        if (animator != null)
            animator.SetBool("IsMoving", true);
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        lastAttackTime = Time.time;

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerData playerData = hitPlayer.GetComponent<PlayerData>();
            if (playerData != null)
            {
                playerData.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
