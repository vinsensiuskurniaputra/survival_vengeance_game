using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public EnemySpawner spawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.enabled = true;   // Start the spawner
            Destroy(gameObject);      // Optional: remove trigger after activation
        }
    }
}
