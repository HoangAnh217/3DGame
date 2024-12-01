using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public static EnemySpawner Instance;

    public static string zombile = "Zombile";
    public static string dragon = "Dragon";
    protected override void Awake()
    {
        Instance = this;
    }
    public virtual void SpawnEnemy(EnemySO enemy, Vector3 spawnPos, Quaternion rotation,WayPoint wayPoint )
    {
        int id = enemy.id;
        Transform enemySp = GetPrefabByIndex(id);
        Transform enemySpawn =  GetObjectFromPool(enemySp);
        Entity entity = enemySpawn.GetComponent<Entity>();
        //enemySpawn.GetComponent<Entity>().SetEnemySO(enemy);
        entity.SetEnemySO(enemy);
        entity.wayPoint = wayPoint;
        base.Spawn(enemySpawn, spawnPos, rotation).gameObject.SetActive(true) ; 
    }
}
