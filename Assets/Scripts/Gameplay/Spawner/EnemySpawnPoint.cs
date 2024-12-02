using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public WaveDataSO waveDataSO;        // Tham chiếu đến WaveDataSO
    public float waveInterval = 5.0f;   // Thời gian nghỉ giữa các wave

    [SerializeField] private WayPoint _wayPoint;

    private int currentWaveIndex = 0;   // Wave hiện tại

    void Start()
    {   
        currentWaveIndex = 0;
        StartNextWave(); // Bắt đầu wave đầu tiên
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= waveDataSO.waves.Count)
        {
            Debug.Log("All waves completed!");
            return; // Dừng lại nếu đã hết wave
        }

        StartCoroutine(SpawnWave(waveDataSO.waves[currentWaveIndex]));
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        foreach (var enemyData in wave.enemies)
        {
                Debug.Log("asdasd");
            for (int i = 0; i < enemyData.count; i++)
            {
                SpawnEnemy(enemyData.enemyData);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return new WaitForSeconds(waveInterval); // Nghỉ giữa các wave
        currentWaveIndex++;

        StartNextWave(); // Gọi wave tiếp theo
    }

    private void SpawnEnemy(EnemySO enemySO)
    {
        EnemySpawner.Instance.SpawnEnemy(enemySO, transform.position, Quaternion.identity, _wayPoint);
        //enemy.GetComponent<Entity>().Setup(enemySO); // Thiết lập dữ liệu từ SO (nếu cần)
    }
}
