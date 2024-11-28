using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretLaze : TurretController
{

    [Header("Laze properties")]
    [SerializeField] private ParticleSystem remnants;
    [SerializeField] LayerMask layerMask;
    [SerializeField] private float length = 30;
    [SerializeField] private LineRenderer laze;
    [SerializeField] private Transform firePoint;
    private Vector3 endPoint;
    protected override void Start()
    {
        base.Start();
        laze.gameObject.SetActive(false);
        laze.useWorldSpace = false;
    }
    protected override void Update()
    {
        base.Update();
        if (enemyTarget != null)
        {
            laze.gameObject.SetActive(true);
            LazeEffect();
        }
        else
        {
            laze.gameObject.SetActive(false);
        }
    }
    protected override void Shoot()
    {
        base.Shoot();
    }
    protected override void LookAtTarget()
    {
        base.LookAtTarget();
    }
    private void UpdateLineRenderer(Vector3 startPoint,Vector3 endPoint)
    {
        //Vector3 startPointLocal = transform.InverseTransformPoint(startPoint);
        Vector3 endPointLocal = laze.transform.InverseTransformPoint(endPoint);
        laze.SetPosition(0, Vector3.zero);
        laze.SetPosition(1, endPoint);
    }
    private void LazeEffect()
    {
        RaycastHit hit;
        Debug.DrawRay(firePoint.position, firePoint.forward * length, Color.red);
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, length))
        {
            endPoint = hit.point;
            remnants.transform.position = endPoint;
            UpdateLineRenderer(firePoint.position, remnants.transform.localPosition);
            //lineRenderer.SetPosition(1, new Vector3(0,0distance) );
        }
    }
}
