using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;          // Prefab musuh yang akan dipanggil
    public Transform[] spawnPoints;         // Titik-titik tempat spawn musuh
    public float spawnInterval = 3f;        // Waktu antar spawn
    public int maxEnemy = 10;
    public int currentEnemy = 0;

    private float timer;
    void Start()
    {
        this.enabled = false; // Disabled by default
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null || currentEnemy >= maxEnemy ) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        currentEnemy += 1;
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
