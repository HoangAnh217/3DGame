using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour,IDameable
{
    [Header("Component")]
    [SerializeField] private Animator animator;
    private MainTower mainTower;

    [SerializeField] private Slider slider;
    [Header("Propertise")]
    [SerializeField] private float speed=3;
    [SerializeField] private float health;
    [SerializeField] private float dame;
    [Header("Movement")]
    private List<Transform> waypoints; 
    private int currentWaypointIndex = 0;
    private Vector3 dir;
    [SerializeField] protected Transform model;
    public WayPoint wayPoint;
    private void Start()
    {   
        currentWaypointIndex = 0;
        waypoints = new List<Transform>();
        health = 100f;
        mainTower = MainTower.Instance;
        CreatPath();
        RotateModel();
        MoveToNextWaypoint();
    }
    public void ReceiveDamage(float dame)
    {
       // throw new System.NotImplementedException();
       health-=dame;
        slider.value = health;
        if (health<=0)
        {
            // OnDead();
            animator.SetBool("Dead", true);
        }
    }
    public virtual void OnDead()
    {
        EnemySpawner.Instance.Despawm(transform);
    }
    protected virtual void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            transform.DOMove(waypoints[currentWaypointIndex].position, speed)
                .SetSpeedBased(true).SetEase(Ease.Linear) // Ð?t di chuy?n theo t?c ð?
                .OnComplete(() =>
                {
                    currentWaypointIndex++; // Tãng ch? s? ði?m m?c
                    if (currentWaypointIndex >= waypoints.Count)
                    {
                        Attack(); // Chuy?n sang tr?ng thái t?n công
                    } else
                    {
                        RotateModel();
                        MoveToNextWaypoint(); // G?i l?i hàm ð? di chuy?n ð?n ði?m ti?p theo

                    }
                });
        }
    }
    private void CreatPath()
    {   
        waypoints.Add(wayPoint.transform);
        while(true)
        {
            Transform obj = wayPoint.chooseNextPoint();
            if (obj == null)
                return;
            waypoints.Add(obj);
            wayPoint = obj.GetComponent<WayPoint>();
        }
    }
    protected virtual void Attack()
    {
        Debug.Log("attack");
        animator.SetBool("Attack", true);
    }
    public virtual void OnAttack()
    {
        if (mainTower==null)
        {
            return;
        }
        mainTower.ReceiveDamage(dame);
    }
    protected virtual void RotateModel()
    {
        dir = waypoints[currentWaypointIndex].position - transform.position;
        Vector3 convertDir = new Vector3(dir.x, 0, dir.z);
        float angle = Mathf.Atan2(convertDir.z, convertDir.x) * Mathf.Rad2Deg;
        model.localRotation = Quaternion.Euler(0, -angle + 90, 0);
    }
}
