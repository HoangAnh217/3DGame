using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class TurretRocket : TurretController
{
    [SerializeField] private float timer = 1f;
    [SerializeField] private Transform turretGun;
    [SerializeField] private ParticleSystem particle;
    protected override void Shoot()
    {
        StartCoroutine(DameEffect());
    }
    private IEnumerator DameEffect()
    {
        Transform obj = EffectSpawner.Instance.Spawn("RocketRainEffect", enemyTarget.transform.position, Quaternion.identity);
        obj.gameObject.SetActive(true);
        particle.Play();
        yield return new WaitForSeconds(0.5f);
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _maxShootDistance, LayerMask.GetMask("Enemy"));
        foreach (var enemies in enemiesInRange)
        {
            Entity health = enemies.gameObject.GetComponent<Entity>();
            if (health == null)
            {
                Debug.LogWarning("We hit something that doesn't have health.........");
            }
            else
            {
                health.ReceiveDamage(_damageAmount);
            }
        }
        yield return new WaitForSeconds(1.3f);
        obj.GetComponentInChildren<VisualEffect>().Stop();
        yield return new WaitForSeconds(1.3f) ;
        EffectSpawner.Instance.Despawm(obj);
    }
    protected override void LookAtTarget()
    {
        base.LookAtTarget();
        float dis = Vector3.Distance(ConvertPos3(enemyTarget), ConvertPos3(transform));
        float angleZ = Mathf.Atan2(10f, dis / 2) * Mathf.Rad2Deg;
        turretGun.localRotation = Quaternion.Euler(0, 0,- angleZ);
    }
}
