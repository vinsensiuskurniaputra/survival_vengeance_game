using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;       // Array dari berbagai prefab musuh
    public Transform[] spawnPoints;         // Titik-titik tempat spawn musuh
    public float spawnInterval = 3f;        // Waktu antar spawn
    public int maxEnemy = 10;
    public int currentEnemy = 0;

    private float timer;

    void Start()
    {
        this.enabled = false; // Dinonaktifkan di awal, aktif via trigger jika perlu
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
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0 || currentEnemy >= maxEnemy)
            return;

        // Pilih spawn point dan prefab secara acak
        int pointIndex = Random.Range(0, spawnPoints.Length);
        int prefabIndex = Random.Range(0, enemyPrefabs.Length);

        Transform spawnPoint = spawnPoints[pointIndex];
        GameObject prefabToSpawn = enemyPrefabs[prefabIndex];

        Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        currentEnemy++;
    }

    public void OnEnemyKilled()
    {
        currentEnemy = Mathf.Max(0, currentEnemy - 1);
    }
}
