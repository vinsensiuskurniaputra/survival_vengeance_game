using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private Animator animator;
    public BossHealthBarUI healthBar;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        if (healthBar != null)
            healthBar.SetHealth((float)currentHealth / maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Optional: trigger hit animation here
        animator.SetTrigger("Hit");

        if (healthBar != null)
            healthBar.SetHealth((float)currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);

        // Disable enemy movement & collision
        GetComponent<BossMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        if (healthBar != null)
        {
            healthBar.IsDead = true; // ini penting
                                     // healthBar.gameObject.SetActive(false); // tidak perlu lagi
        }

        // Optionally: Destroy after delay
        //Destroy(gameObject, 5f);
    }
}
