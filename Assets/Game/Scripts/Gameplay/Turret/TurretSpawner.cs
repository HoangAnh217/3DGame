using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : Spawner
{
    public static TurretSpawner instance;

    protected override void Awake()
    {
        instance = this;
    }
    public override Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        return base.Spawn(prefab, spawnPos, rotation);
    }
}
