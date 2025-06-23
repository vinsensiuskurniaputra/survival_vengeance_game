using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 1f;
    public int damage = 1;
    public float damageCooldown = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 originalScale;
    private float lastDamageTime;

    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            movement = direction;
            SetAttacking(false); // not attacking when chasing

            // Flip sprite
            if (direction.x != 0)
            {
                Vector3 newScale = originalScale;
                newScale.x = Mathf.Abs(originalScale.x) * (direction.x > 0 ? 1 : -1);
                transform.localScale = newScale;
            }
        }
        else
        {
            movement = Vector2.zero;
            SetAttacking(true); // start attack animation
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastDamageTime >= damageCooldown)
        {
            if (PlayerData.Instance != null)
            {
                PlayerData.Instance.TakeDamage(damage);
                lastDamageTime = Time.time;
                SetAttacking(true); // ensure attack is triggered
            }
        }
    }

    private void SetAttacking(bool attacking)
    {
        if (isAttacking != attacking)
        {
            isAttacking = attacking;
            animator.SetBool("IsAttacking", attacking);
        }
    }
}
