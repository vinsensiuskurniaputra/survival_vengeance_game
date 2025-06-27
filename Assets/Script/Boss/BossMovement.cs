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

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileRange = 5f;

    public int shotsPerCycle = 3;              // jumlah tembakan per siklus
    public float timeBetweenShots = 0.5f;      // delay antar proyektil
    public float delayAfterCycle = 2f;         // jeda setelah siklus selesai

    private bool isShooting = false;
    private int shotsLeft;
    private float shotTimer;


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

        if (!isShooting && distance <= projectileRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartRangedAttack();
        }

        if (isShooting)
        {
            shotTimer += Time.deltaTime;
            if (shotsLeft > 0 && shotTimer >= timeBetweenShots)
            {
                shotTimer = 0f;
                ShootProjectile();
                shotsLeft--;

                if (shotsLeft == 0)
                {
                    isShooting = false;
                    lastAttackTime = Time.time; // reset delay setelah selesai 1 batch
                }
            }
        }

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown && !isShooting)
        {
            MeleeAttack();
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

    void MeleeAttack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        lastAttackTime = Time.time;

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerData.Instance.TakeDamage(damage);
        }
    }

    void StartRangedAttack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        isShooting = true;
        shotsLeft = shotsPerCycle;
        shotTimer = timeBetweenShots; // langsung tembak pertama kali
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null && player != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 shootDir = (player.position - firePoint.position).normalized;
            proj.GetComponent<Projectile>().SetDirection(shootDir);
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, projectileRange);
    }
}
