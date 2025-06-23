using UnityEngine;

public class SpawnerBossTrigger : MonoBehaviour
{
    public EnemySpawnerBoss spawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.ActivateSpawner();
            Destroy(gameObject);      // Optional: remove trigger after activation
        }
    }
}

