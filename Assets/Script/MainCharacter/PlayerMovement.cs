using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down;

    public HealthBar healthBar;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public LayerMask gateLayer;
    public GameObject attackEffectPrefab;
    public float effectDuration = 0.3f;
    public UpgradeManager upgradeManager;


    void Start()
    {
        PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
    }


    void Update()
    {
        // Cek apakah dialog sedang aktif, jika ya maka skip semua input
        // (Ini sebenarnya tidak diperlukan lagi karena Time.timeScale = 0, 
        // tapi tetap bisa digunakan untuk keamanan ekstra)
        if (DialogueManager.IsDialogueActive)
        {
            // Reset movement agar player berhenti
            movement = Vector2.zero;
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Speed", 0);
            return; // Skip semua input lainnya
        }

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
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgradeManager.ToggleUpgradeMenu();
        }

        // Shortcut saat panel aktif
        if (upgradeManager.upgradePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                upgradeManager.UpgradeMaxHP();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                upgradeManager.UpgradeAttack();
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                upgradeManager.HealFull();
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                upgradeManager.UpgradeSpeed();
            }
        }
    }

    void FixedUpdate()
    {
        // Tidak perlu cek DialogueManager.IsDialogueActive lagi karena Time.timeScale = 0
        // akan otomatis menghentikan FixedUpdate
        
        // Gerakkan karakter
        rb.MovePosition(rb.position + movement * PlayerData.Instance.moveSpeed * Time.fixedDeltaTime);
    }
    void Attack()
    {
        Vector2 attackDir = lastMoveDirection.normalized;
        Vector2 attackOrigin = rb.position + attackDir * 0.5f;

        // === Spawn Effect ===
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, attackOrigin, Quaternion.identity);
            float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
            effect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            Destroy(effect, effectDuration);
        }

        // === Serang musuh ===
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(PlayerData.Instance.attackPower);
            }
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(PlayerData.Instance.attackPower);
            }
        }

        // === Serang Gate (GameObject) ===
        Collider2D[] hitGates = Physics2D.OverlapCircleAll(attackOrigin, attackRange, gateLayer);
        foreach (Collider2D gate in hitGates)
        {
            GateHealth gateHealth = gate.GetComponent<GateHealth>();
            if (gateHealth != null)
            {
                gateHealth.TakeDamage(1);
                Debug.Log("Gate attacked!");
            }
            else
            {
                Debug.Log("Gate Not attacked!");
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
