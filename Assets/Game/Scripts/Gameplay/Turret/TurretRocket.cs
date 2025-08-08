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
            audioManager.PlaySFX("Shoot");
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
   
}
