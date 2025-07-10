using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Entity : TriBehaviour, IDameable
{   
    [SerializeField] private Animator animator;
    [SerializeField] protected Transform model;
    [Header("component scripts")]
    private EffectSpawner effectSpawner;

    //
    private EnemySO enemySO;
    private float health;
    private int level;

    [Header("Movement")]
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    private WayPoint wayPoint ;

    [Header("effect game juice")]
    public Canvas canvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider damageEffectSlider;
    [SerializeField] private TextMeshPro textLevel;
    protected override void Start()
    {   
        InitializeEntity();
        healthSlider.gameObject.SetActive(false);
    }
    public override void OnEnable()
    {
        base.OnEnable();
        level =enemySO.level;
        textLevel.text = level.ToString();
        healthSlider.gameObject.SetActive(false);
        currentWaypointIndex = 0;
        waypoints.Clear();
        CreatePath();
        if (waypoints.Count == 0) return;

        Vector3 dir = waypoints[currentWaypointIndex].position - transform.position;
        Vector3 convertDir = new Vector3(dir.x, 0, dir.z);
        float angle = Mathf.Atan2(convertDir.z, convertDir.x) * Mathf.Rad2Deg;
        model.localRotation = Quaternion.Euler(0, -angle + 90, 0);
        MoveToNextWaypoint();
    }
    protected virtual void InitializeEntity()
    {
        healthSlider.maxValue = enemySO.health;
        damageEffectSlider.maxValue = enemySO.health;

        // component scripts
        effectSpawner = EffectSpawner.Instance;
    }
    public void SetEnemySO(EnemySO newEnemySO,WayPoint _wayPoint)
    {

        enemySO = newEnemySO;
        health = enemySO.health;
        healthSlider.maxValue = health;
        healthSlider.value = health;
        wayPoint = _wayPoint;
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        animator = GetComponentInChildren<Animator>();
        healthSlider = transform.Find("Canvas").GetComponentInChildren<Slider>();
        model = transform.Find("Model");
        LoadDataGameJuice();
    }
    protected virtual void LoadDataGameJuice()
    {
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        damageEffectSlider = transform.Find("Canvas").Find("Hp").Find("SliderDameReiceive").GetComponent<Slider>();
       
    }
    #region Damage Handling

    public virtual void ReceiveDamage(int damage)
    {
        DisplayDamageText(damage);
        healthSlider.gameObject.SetActive(true);
        damageEffectSlider.value = health;
        health -= damage;
        healthSlider.value = health;
        damageEffectSlider.DOValue(health-1, 0.5f).SetEase(Ease.Linear); 
        if (health <= 0)
        {
            OnDead();
           // animator.SetBool("Dead", true);
        }
    }
    private void DisplayDamageText(int damage)
    {
        Transform obj =  EffectSpawner.Instance.Spawn(EffectSpawner.TextFloat, transform.position, Quaternion.identity);
        obj.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public virtual void OnDead()
    {
        effectSpawner.SpawnCoin(enemySO.moneyForDead,transform.position);
        EnemySpawner.Instance.Despawm(transform);
       
    }

    #endregion

    #region Movement

   

    protected virtual void MoveToNextWaypoint()
    {   
        if (currentWaypointIndex < waypoints.Count)
        {
            transform.DOMove(waypoints[currentWaypointIndex].position + new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f)), enemySO.speed)
                .SetSpeedBased(true).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    currentWaypointIndex++;
                    if (currentWaypointIndex >= waypoints.Count)
                    {   
                        PlayerData.instance.ReceiveDamage(10);
                        EnemySpawner.Instance.Despawm(transform);
                    }
                    else
                    {
                        RotateModel();
                        MoveToNextWaypoint();
                    }
                });
        }
    }

    private void CreatePath()
    {
        if (wayPoint == null) return;

        waypoints.Add(wayPoint.transform);

        while (true)
        {
            Transform nextPoint = wayPoint.chooseNextPoint();
            if (nextPoint == null) return;
            waypoints.Add(nextPoint);
            wayPoint = nextPoint.GetComponent<WayPoint>();
        }
    }

    protected virtual void RotateModel()
    {
        if (waypoints.Count == 0) return;

        Vector3 dir = waypoints[currentWaypointIndex].position - transform.position;
        Vector3 convertDir = new Vector3(dir.x, 0, dir.z);

        float angle = Mathf.Atan2(convertDir.z, convertDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, -angle + 90, 0);

        StartCoroutine(RotateOverTime(targetRotation, 0.2f)); // 0.5f là thời gian để xoay hoàn tất
    }

    private IEnumerator RotateOverTime(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = model.localRotation; // Lưu góc quay ban đầu
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            model.localRotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Đợi đến frame tiếp theo
        }

        model.localRotation = targetRotation;
    }
    #endregion

    #region Attack

    protected virtual void Attack()
    {
       // animator.SetBool("Attack", true);
    }

    public virtual void OnAttack()
    {
        Debug.Log("Take Damage");
        PlayerData.instance.ReceiveDamage(enemySO.dameAttack);
    }

    #endregion
   
}
