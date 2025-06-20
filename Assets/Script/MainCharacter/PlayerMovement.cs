using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down;

    public HealthBar healthBar;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public GameObject attackEffectPrefab; // <- Tambahkan ini
    public float effectDuration = 0.3f;   // <- Durasi efek animasi sebelum dihancurkan


    void Start()
    {
        PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
    }


    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;
        }

        movement.Normalize();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Simpan arah terakhir
        animator.SetFloat("LastHorizontal", lastMoveDirection.x);
        animator.SetFloat("LastVertical", lastMoveDirection.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Gerakkan karakter
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    void Attack()
    {
        // Find enemies in front of player
        Vector2 attackDir = lastMoveDirection.normalized;
        Vector2 attackOrigin = rb.position + attackDir * 0.5f;

        // === Spawn Effect ===
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, attackOrigin, Quaternion.identity);
            
            // Optional: rotate based on direction
            float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
            effect.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Destroy(effect, effectDuration); // Auto-destroy
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            int damage = PlayerData.Instance.attackPower;
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (rb == null) return;

        Vector2 attackDir = lastMoveDirection.normalized;
        Vector2 attackOrigin = (Vector2)transform.position + attackDir * 0.5f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRange);
    }
}
