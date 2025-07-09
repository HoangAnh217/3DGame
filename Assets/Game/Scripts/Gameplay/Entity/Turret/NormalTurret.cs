using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTurret : TurretController
{
    [SerializeField] private ParticleSystem par;   
    protected override void Shoot()
    {
        // wait time 1s, 
        par.Play();
        base.Shoot();

    }
}

