using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : TriBehaviour
{
    protected Transform enemyTarget;
    [Header("propertise")]
    /*[SerializeField] protected float _fireRate = 0.1f;
    [SerializeField] protected float _damageAmount = 20f;
    [SerializeField] protected float _maxShootDistance = 10f;*/
    public TurretDataSO turretDataSO;
    [Header("i4 Turret")]
    [SerializeField] protected Transform turretPivot;

    // 
    private float timeTillNextShot;
    #region get set 
   /* public float GetMaxShootDistance()
    {
        return _maxShootDistance;
    }*/
    #endregion
    protected override void Start()
    {

    }
    protected virtual void Update()
    {
        timeTillNextShot -= Time.deltaTime;
        if (CheckTarget())
        {
            LookAtTarget();
            if (timeTillNextShot<0)
            {
                Shoot();
                timeTillNextShot = turretDataSO.fireRate;
            }
        } 
    }
    protected abstract bool CheckTarget();
    protected abstract Transform GetNearestEnemy(Collider[] enemies);
    protected abstract void LookAtTarget();
    protected abstract void Shoot();
}
