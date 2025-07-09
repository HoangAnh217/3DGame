using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDespawn : DeSpawnByTime
{
    public override void DeSpawnObj()
    {
        base.DeSpawnObj();
        EffectSpawner.Instance.Despawm(transform);
    }
}
