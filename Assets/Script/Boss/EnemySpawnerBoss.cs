using UnityEngine;

public class EnemySpawnerBoss : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // ← Array untuk lebih dari 1 prefab
    public float spawnInterval = 5f;

    private float timer;
    public bool isActive = false;

    void Update()
    {
        if (!isActive || enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Pilih prefab secara acak
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject chosenPrefab = enemyPrefabs[index];

        Instantiate(chosenPrefab, transform.position, Quaternion.identity);
    }

    public void DeactivateSpawner()
    {
        isActive = false;
    }

    public void ActivateSpawner()
    {
        isActive = true;
    }
}
