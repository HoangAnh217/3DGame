using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class TurretRocket : TurretController
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem effectPrefabs;
    [SerializeField] private Transform firePoint;
    protected override void Shoot()
    {   
        bulletPrefab.transform.position = firePoint.position;
        bulletPrefab.SetActive(true);
        bulletPrefab.transform.DOMove(enemyTarget.position, 0.1f).OnComplete(() =>
        {
            base.Shoot();
            bulletPrefab.gameObject.SetActive(false);
            effectPrefabs.gameObject.SetActive(true);
            effectPrefabs.transform.position = enemyTarget.position+Vector3.up*0.4f;
            effectPrefabs.Play();
            DealExplosionDamage(effectPrefabs.transform.position);
            // lay enemy xung quanh vi tri effect de gay dame aoe
            //effectPrefabs.gameObject.SetActive(false);
        });

    }
    private void DealExplosionDamage(Vector3 explosionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, 0.5f, LayerMask.GetMask("Enemy"));

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDameable damageable))
            {
                // Gây sát thương cho đối tượng
                damageable.ReceiveDamage(turretDataSO.damage/4);
            }
        }
    }
    /*[SerializeField] private float timer = 1f;
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
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, turretDataSO.maxShootDistance, LayerMask.GetMask("Enemy"));
        foreach (var enemies in enemiesInRange)
        {
            Entity health = enemies.gameObject.GetComponent<Entity>();
            if (health == null)
            {
                Debug.LogWarning("We hit something that doesn't have health.........");
            }
            else
            {
                health.ReceiveDamage(turretDataSO.damage);
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
    }*/
}
