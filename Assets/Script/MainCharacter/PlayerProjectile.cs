using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 1;
    public float lifetime = 5f;
    public LayerMask enemyLayer;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotasi sprite sesuai arah
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime); // Arah "kanan" relatif ke rotasi saat spawn
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Cek apakah ini musuh dan berikan damage
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            BossHealth bossHealth = other.GetComponent<BossHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            else if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
