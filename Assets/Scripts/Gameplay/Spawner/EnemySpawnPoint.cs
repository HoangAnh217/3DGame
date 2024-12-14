using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{   
    public static EnemySpawnPoint Instance;
    public WaveDataSO waveDataSO;        // Tham chiếu đến WaveDataSO
    public float waveInterval = 5.0f;   // Thời gian nghỉ giữa các wave

    [SerializeField] private WayPoint _wayPoint;

    private int currentWaveIndex = 0;   // Wave hiện tại
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentWaveIndex = 0;
        StartWave();
    }
    public void StartWave()
    {
        if (currentWaveIndex >= waveDataSO.waves.Count)
        {
            Debug.Log("All waves completed!");
            return; // Dừng lại nếu đã hết wave
        }
        Debug.Log("Next Wave");
        StartCoroutine(SpawnWave(waveDataSO.waves[currentWaveIndex]));
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {   
        //UI_IngameManager.Instance.
        yield return new WaitForSeconds(3f);
        EnemySpawner.Instance.SetWave(wave);
        foreach (var enemyData in wave.enemies)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                SpawnEnemy(enemyData.enemyData);
                yield return new WaitForSeconds(0.2f);
            }
        }
        currentWaveIndex++;
    }

    private void SpawnEnemy(EnemySO enemySO)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 0.2f;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;

        // Spawn enemy tại vị trí ngẫu nhiên
        EnemySpawner.Instance.SpawnEnemy(enemySO, randomPosition, Quaternion.identity, _wayPoint);
    }
}
