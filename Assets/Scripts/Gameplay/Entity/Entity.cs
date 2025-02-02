using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Entity : TriBehaviour, IDameable
{
    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] protected Transform model;


    private EnemySO enemySO;
    private float health;
    private int level;

    [Header("Movement")]
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;

    private WayPoint wayPoint ;

    [Header("effect game juice")]
    public Canvas canvas;
    public GameObject coinPrefab; // Prefab đồng xu
    public GameObject parentCoin;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider damageEffectSlider;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private TextMeshPro textLevel;
    protected override void Start()
    {   
        
        InitializeEntity();
        healthSlider.gameObject.SetActive(false);
        damageTextPrefab.SetActive(false);
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
        //RotateModel();
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
    }
    // Set enemy data
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
        //textLevel = transform.Find("Canvas").Find("Hp").Find("Image").GetComponentInChildren<TextMeshPro>();
        LoadDataGameJuice();
    }
    protected virtual void LoadDataGameJuice()
    {
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        damageEffectSlider = transform.Find("Canvas").Find("Hp").Find("SliderDameReiceive").GetComponent<Slider>();
        damageTextPrefab = transform.Find("Canvas").Find("DamageText").gameObject;
       /* parentCoin = GameObject.Find("Canvas").transform.Find("Money").gameObject;
        coinPrefab = parentCoin.transform.Find("Image").gameObject;*/
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
            animator.SetBool("Dead", true);
        }
    }
    private void DisplayDamageText(int damage)
    {
        // Tạo bản sao của TextMeshPro từ prefab
        GameObject damageTextInstance = Instantiate(damageTextPrefab.gameObject, damageTextPrefab.transform.position, damageTextPrefab.transform.rotation);
        RectTransform rectTransform = damageTextPrefab.GetComponent<RectTransform>();
        // Đảm bảo bản sao được thêm vào Canvas (hoặc nơi bạn muốn)
        damageTextInstance.transform.SetParent(transform.Find("Canvas"), false); // False giữ nguyên vị trí và tỷ lệ
        damageTextInstance.GetComponent<RectTransform>().localPosition = rectTransform.localPosition;
        // Hiển thị sát thương
        damageTextInstance.SetActive(true);
        TextMeshPro damageText = damageTextInstance.GetComponent<TextMeshPro>();
        damageText.text =  damage.ToString(); // Hiển thị số lượng sát thương
        // Di chuyển Text lên và làm mờ nó
        damageTextInstance.transform.DOMoveY(damageTextInstance.transform.position.y + 0.5f, 0.8f) // Di chuyển lên trên
            .SetEase(Ease.OutQuad); // Hiệu ứng di chuyển mượt mà

        // Làm mờ Text sau khi di chuyển xong
        damageText.DOFade(0, 0.5f).OnKill(() => Destroy(damageTextInstance)); // Làm mờ text dần dần và hủy khi hoàn thành
    }

    public virtual void OnDead()
    {
        EnemySpawner.Instance.Despawm(transform);
        //GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        SpawnCoin();
       
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
                        Attack();
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

        // Tính toán hướng từ vị trí hiện tại đến waypoint
        Vector3 dir = waypoints[currentWaypointIndex].position - transform.position;
        Vector3 convertDir = new Vector3(dir.x, 0, dir.z);

        // Chuyển đổi hướng thành góc quay mục tiêu
        float angle = Mathf.Atan2(convertDir.z, convertDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, -angle + 90, 0);

        // Bắt đầu Coroutine để xoay từ từ
        StartCoroutine(RotateOverTime(targetRotation, 0.2f)); // 0.5f là thời gian để xoay hoàn tất
    }

    // Coroutine để xoay từ từ
    private IEnumerator RotateOverTime(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = model.localRotation; // Lưu góc quay ban đầu
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Nội suy góc quay theo thời gian
            model.localRotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Đợi đến frame tiếp theo
        }

        // Đảm bảo rotation cuối cùng đúng chính xác góc mục tiêu
        model.localRotation = targetRotation;
    }
    #endregion

    #region Attack

    protected virtual void Attack()
    {
        animator.SetBool("Attack", true);
    }

    public virtual void OnAttack()
    {
        Debug.Log("Take Damage");
        PlayerData.instance.ReceiveDamage(enemySO.dameAttack);
    }

    #endregion
    #region effect
    private void SpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab, parentCoin.transform);
        RectTransform coinRect = coin.GetComponent<RectTransform>();
        coinRect.anchoredPosition = ConvertWorldToCanvasPosition(transform.position);


        // Bay từ vị trí bắt đầu đến vị trí UI mục tiêu
        coinRect.DOAnchorPos(coinPrefab.GetComponent<RectTransform>().anchoredPosition, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            Destroy(coin); // Xóa coin sau khi bay tới UI
            PlayerData.instance.ReceiveMoney(enemySO.moneyForDead);
        });
    }
    public Vector2 ConvertWorldToCanvasPosition(Vector3 worldPosition)
    {
        // 1. Chuyển từ World Space sang Screen Space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // 2. Chuyển từ Screen Space sang Canvas Space
        RectTransform canvasRect = parentCoin.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );

        return canvasPosition;
    }
    #endregion
}
