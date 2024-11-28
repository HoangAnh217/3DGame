using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private int wave = 1;                     // Wave hiện tại
    private int numOfEneSpawn =5;               // Số lượng quái trong wave
    public GameObject enemyPrefab;           // Prefab của quái
    public Transform spawnPoint;             // Điểm spawn quái
    public Transform enemyParent;            // Cha quản lý quái trong Hierarchy
    public float spawnInterval = 1.0f;       // Khoảng thời gian giữa mỗi lần spawn
    public float waveInterval = 5.0f;        // Thời gian nghỉ giữa các wave

    private bool isSpawning = false;         // Trạng thái spawn
    [SerializeField] private WayPoint _wayPoint;

    void Start()
    {
        StartNextWave(); // Bắt đầu wave đầu tiên
    }

    public void EnemySpawn()
    {
        if (!isSpawning)
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        Debug.Log($"Wave {wave} started!");
        numOfEneSpawn = 7; // Số lượng quái trong mỗi wave tăng dần
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        for (int i = 0; i < numOfEneSpawn; i++)
        {
            SpawnEnemy(); // Gọi hàm spawn từng quái
            yield return new WaitForSeconds(spawnInterval); // Chờ giữa mỗi lần spawn
        }

        isSpawning = false;
        wave++; // Chuyển sang wave tiếp theo

        Debug.Log($"Wave {wave - 1} completed. Next wave in {waveInterval} seconds.");
        yield return new WaitForSeconds(waveInterval); // Nghỉ giữa các wave

        EnemySpawn(); // Gọi wave tiếp theo
    }

    private void SpawnEnemy()
    {
        // Tạo quái tại điểm spawn
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, enemyParent);
        enemy.gameObject.SetActive(true);
        enemy.GetComponent<Entity>().wayPoint = _wayPoint;
        Debug.Log($"Enemy spawned: {enemy.name}");
    }
}
