using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public static EnemySpawner Instance;

    public static string zombile = "Zombile";
    protected override void Awake()
    {
        Instance = this;
    }
}
