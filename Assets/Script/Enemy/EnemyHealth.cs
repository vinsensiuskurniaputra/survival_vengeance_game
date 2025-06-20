using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private Animator animator;
    public GameObject expPrefab;
    public Transform dropPoint;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Optional: trigger hit animation here
        animator.SetTrigger("Hit");

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
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        StartCoroutine(DropExpWithDelay(1f));

        // Optionally: Destroy after delay
        Destroy(gameObject, 5f);
    }

    IEnumerator DropExpWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (expPrefab != null)
        {
            Instantiate(expPrefab, dropPoint != null ? dropPoint.position : transform.position, Quaternion.identity);
        }
    }
}
