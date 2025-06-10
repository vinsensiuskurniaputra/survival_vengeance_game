using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down;

    public HealthBar healthBar;


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
            PlayerData.Instance.TakeDamage(1);
        }
    }

    void FixedUpdate()
    {
        // Gerakkan karakter
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
