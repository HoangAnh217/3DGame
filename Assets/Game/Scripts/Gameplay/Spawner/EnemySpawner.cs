using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public static EnemySpawner Instance;

    public static string zombile = "Zombile";
    public static string dragon = "Dragon";

    //private List<Transform> enemyWave = new List<Transform>();
    private int numOfEnemy = 0;
    protected override void Awake()
    {
        Instance = this;
    }
    public virtual void SpawnEnemy(EnemySO enemySO, Vector3 spawnPos, Quaternion rotation,WayPoint wayPoint )
    {
        int id = enemySO.id;
        Transform enemySp = GetPrefabByIndex(id);
        enemySp =  GetObjectFromPool(enemySp);
        Entity entity = enemySp.GetComponent<Entity>();
        entity.SetEnemySO(enemySO,wayPoint);
        enemySp.SetParent(holder);
        enemySp.SetPositionAndRotation(spawnPos, rotation);
        enemySp.gameObject.SetActive(true);
    }
    public virtual void SetWave(EnemyWave wave)
    {
        foreach (var enemyData in wave.enemies)
        {
            numOfEnemy += enemyData.count;
        }
    }
    public virtual bool IsClear()
    {
        return numOfEnemy == 0;
    }
    public override void Despawm(Transform obj)
    {   
        base.Despawm(obj);
        DOTween.Kill(obj);
        numOfEnemy--;
        if (IsClear())
        {
            EnemySpawnPoint.Instance.StartWave();
        }
    }
}
