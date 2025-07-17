using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnWave : MonoBehaviour
{
    public static EnemySpawnWave Instance;

    [Header("Wave Data")]
    public List<WaveDataSO> listWaveDataSO;      // Nhiều wave khác nhau
    public float waveInterval = 5.0f;

    [Header("Spawn Points")]
    [SerializeField] private List<WayPoint> wayPoints;         // Đường đi ứng với mỗi portal
    [SerializeField] private List<Transform> portalGates;      // Cổng portal ứng với mỗi spawn point

    private int currentWaveIndex = 0;
    private int maxWave;
    private int amountOfPointSpawn ; 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentWaveIndex = -1;
        amountOfPointSpawn = portalGates.Count;
        // Đặt scale tất cả portal về 0
        foreach (var gate in portalGates)
            gate.localScale = Vector3.zero;

        maxWave = 0;
        foreach ( WaveDataSO waveDataSO in listWaveDataSO)
        {
            foreach (var wave in waveDataSO.waves)
            {
                if (maxWave < wave.waveIndexSpawn)
                {
                    maxWave = wave.waveIndexSpawn + 1;
                }
            }
            
        }
        StartWave();
    }

    public void StartWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= maxWave)
        {
            Debug.Log("You win");
            GameController.Instance.WinGame();
            return;
        }

        StartCoroutine(HandleStartWave());
    }

    private IEnumerator HandleStartWave()
    {
        MainCanvas.Instance.ShowWaveNotification(currentWaveIndex + 1, maxWave, 2f);
        yield return new WaitForSeconds(2f);

        // Scale tất cả portal được dùng
        for (int i = 0; i < amountOfPointSpawn ; i++)
        {

            Debug.Log(listWaveDataSO[i].waves[currentWaveIndex].waveIndexSpawn +"  "+ currentWaveIndex);

            if (listWaveDataSO[i].waves[currentWaveIndex].waveIndexSpawn == currentWaveIndex)
            {
                portalGates[i].DOScale(Vector3.one, 1);
                StartCoroutine(SpawnWave(listWaveDataSO[i].waves[currentWaveIndex],i));
            }
        }
    }

    private IEnumerator SpawnWave(EnemyWave wave,int pointSpawnIndex)
    {
        yield return new WaitForSeconds(2f);

        EnemySpawner.Instance.UpdateAmountOfEnemy(wave);

        foreach (var enemyData in wave.enemies)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                SpawnEnemy(enemyData.enemyData, pointSpawnIndex);

                float rad = Random.Range(0.23f, 0.45f);
                yield return new WaitForSeconds(rad);
            }
        }

        for (int i = 0; i < amountOfPointSpawn; i++)
        {
            portalGates[i].DOScale(Vector3.zero, 0.45f);
        }

    }

    private void SpawnEnemy(EnemySO enemySO, int spawnIndex)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 0.2f;
        Vector3 spawnPos = portalGates[spawnIndex].position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        WayPoint path = wayPoints[spawnIndex];

        EnemySpawner.Instance.SpawnEnemy(enemySO, spawnPos, Quaternion.identity, path);
    }

    #region Tutorial Mode (giữ nguyên)
    public IEnumerator SpawnEnemyTutorial(WaveDataSO wave)
    {
        portalGates[0].DOScale(Vector3.one, 1);
        yield return new WaitForSeconds(1);
        foreach (var enemyData in wave.waves)
        {
            for (int i = 0; i < enemyData.enemies.Count; i++)
            {
                Debug.Log(enemyData.enemies.Count);
                SpawnEnemy(enemyData.enemies[i].enemyData, 0); // dùng portal 0 cho tutorial
                float rad = Random.Range(0.23f, 0.45f);
                yield return new WaitForSeconds(rad);
            }
        }
        portalGates[0].DOScale(Vector3.zero, 0.45f);
    }
    #endregion
}
