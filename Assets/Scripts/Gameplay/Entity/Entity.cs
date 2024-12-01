using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Entity : TriBehaviour,IDameable
{
    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private Slider slider;
    private MainTower mainTower;

    [Header("Propertise")]
    /* [SerializeField] private float speed=3;
     [SerializeField] private float health;
     [SerializeField] private float dame;
     [SerializeField] private int moneyForDead = 75;*/
    [SerializeField] private EnemySO enemySO;
    private float health;
    [Header("Movement")]
    private List<Transform> waypoints; 
    private int currentWaypointIndex = 0;
    private Vector3 dir;
    [SerializeField] protected Transform model;
    [HideInInspector] public WayPoint wayPoint;
    protected override void Start()
    {   
        currentWaypointIndex = 0;
        waypoints = new List<Transform>();
        health = enemySO.health;
        mainTower = MainTower.Instance;
        CreatPath();
        RotateModel();
        MoveToNextWaypoint();
    }
    public void SetEnemySO(EnemySO _enemySO)
    {
        enemySO = _enemySO;
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        animator = GetComponentInChildren<Animator>();
        slider = transform.Find("Canvas").GetComponentInChildren<Slider>();
        model = transform.Find("Model");
    }
    public void ReceiveDamage(float dame)
    {
       // throw new System.NotImplementedException();
       health-=dame;
        slider.value = health;
        if (health<=0)
        {
            OnDead();
            animator.SetBool("Dead", true);
        }
    }
    public virtual void OnDead()
    {
        EnemySpawner.Instance.Despawm(transform);
        GameController.instance.ReceiveMoney(enemySO.moneyForDead);
    }
    protected virtual void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            transform.DOMove(waypoints[currentWaypointIndex].position, enemySO.speed)
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
        if (wayPoint==null)
        {
            return;
        }
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
        mainTower.ReceiveDamage(enemySO.dameAttack);
    }
    protected virtual void RotateModel()
    {
        if (waypoints.Count==0)
        {
            return;
        }
        dir = waypoints[currentWaypointIndex].position - transform.position;
        Vector3 convertDir = new Vector3(dir.x, 0, dir.z);
        float angle = Mathf.Atan2(convertDir.z, convertDir.x) * Mathf.Rad2Deg;
        model.localRotation = Quaternion.Euler(0, -angle + 90, 0);
    }
}
