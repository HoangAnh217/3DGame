using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnWave : MonoBehaviour
{   
    public static EnemySpawnWave Instance;
    public WaveDataSO waveDataSO;        // Tham chiếu đến WaveDataSO
    public float waveInterval = 5.0f;   // Thời gian nghỉ giữa các wave

    [SerializeField] private WayPoint _wayPoint;
    [SerializeField] Transform portalGate;

    private int currentWaveIndex = 0;   // Wave hiện tại
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentWaveIndex = 0;
        portalGate.localScale = Vector3.zero;
        StartWave();
    }
    public void StartWave()
    {
        if (currentWaveIndex >= waveDataSO.waves.Count)
        {
            Debug.Log("You win");
            GameController.Instance.WinGame();
            return;
        }
        Debug.Log("asdasd");

        StartCoroutine(HandleStartWave());
    }
    private IEnumerator HandleStartWave()
    {
        MainCanvas.Instance.ShowWaveNotification(currentWaveIndex + 1, waveDataSO.waves.Count, 2f);

        Debug.Log("asdasd");

        yield return new WaitForSeconds(2f); // Đợi hiển thị xong wave
        Debug.Log("asdasd");
        portalGate.DOScale(Vector3.one, 1); // Có thể chạy cùng lúc với hiệu ứng wave
        StartCoroutine(SpawnWave(waveDataSO.waves[currentWaveIndex]));
    }


    private IEnumerator SpawnWave(EnemyWave wave)
    {   
        //UI_IngameManager.Instance.
        yield return new WaitForSeconds(2f);
        EnemySpawner.Instance.SetWave(wave);
        foreach (var enemyData in wave.enemies)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                SpawnEnemy(enemyData.enemyData);
                float rad = Random.Range(0.23f, 0.45f);

                yield return new WaitForSeconds(rad);
            }
        }
        portalGate.DOScale(Vector3.zero, 0.45f);
        currentWaveIndex++;
    }

    private void SpawnEnemy(EnemySO enemySO)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 0.2f;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;

        // Spawn enemy tại vị trí ngẫu nhiên
        EnemySpawner.Instance.SpawnEnemy(enemySO, randomPosition, Quaternion.identity, _wayPoint);
    }

    #region for tutorial system
    public IEnumerator SpawnEnemyTutorial(WaveDataSO wave)
    {
        portalGate.DOScale(Vector3.one, 1);
        yield return new WaitForSeconds(1);
        foreach (var enemyData in wave.waves)
        {


            for (int i = 0; i < enemyData.enemies.Count; i++)
            {

                Debug.Log(enemyData.enemies.Count);

                SpawnEnemy(enemyData.enemies[i].enemyData);
                float rad = Random.Range(0.23f, 0.45f);

                yield return new WaitForSeconds(rad);
            }
        }
        portalGate.DOScale(Vector3.zero, 0.45f);
    }
    #endregion
}
