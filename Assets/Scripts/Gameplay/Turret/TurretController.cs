using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class TurretController : Turret, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image rangeAttack;
    // turret data test 
    /* public int cost = 20;
     public int towerLevel = 1;
     public int upgradeCost = 50;
     public float plusDame = 50;*/
    private int level = 1;
    [Header("Upgrade")]
    [SerializeField] private List<MeshRenderer> rendererOld;
    [SerializeField] private List<MeshRenderer> rendererUpgrade;
    protected override void Start()
    {
        base.Start();
        rangeAttack.color = Color.green;
        rangeAttack.gameObject.SetActive(false);
        //upgradeUI.SetActive(false);
    }
    protected override void Update()
    {
       base.Update();
    }
    private void OnValidate()
    {
        rangeAttack.transform.localScale = Vector3.one*(turretDataSO.maxShootDistance/10);
    }
    public void UpgradeTurret()
    {
        /*towerLevel++;*/
        if (level == 2)
        {
            Debug.Log("MaxLevel");
            return;
        }
        // turretDataSO.damage += plusDame;
        level++;
        turretDataSO = turretDataSO.nextLevel;
        rangeAttack.transform.localScale = Vector3.one * (turretDataSO.maxShootDistance / 10);
        PlayerData.instance.ReceiveMoney(-turretDataSO.cost);
        for (int i = 0; i < rendererOld.Count; i++)
        {
            rendererOld[i].materials = rendererUpgrade[i].materials;
        }
    }
    protected override bool CheckTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, turretDataSO.maxShootDistance, LayerMask.GetMask("Enemy"));
        if (enemiesInRange.Length > 0)
        {
            enemyTarget = GetNearestEnemy(enemiesInRange);
            rangeAttack.color = Color.red;
            return true;
        }
        else
        {   
            enemyTarget=null;
            rangeAttack.color = Color.green;
            return false;
        }
    }
    protected override Transform GetNearestEnemy(Collider[] enemies)
    {
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }
        return nearestEnemy;
    }
    protected override void LookAtTarget()
    {
        Vector3 direction = enemyTarget.position - turretPivot.position;
        Vector3 convertDir = new Vector3(direction.x,0,direction.z);
        
        float angle = Mathf.Atan2(convertDir.z,convertDir.x)*Mathf.Rad2Deg;

        turretPivot.localRotation = Quaternion.Euler(0,-angle+90,0);
    }

    protected override void Shoot()
    {   
        Entity health = enemyTarget.gameObject.GetComponent<Entity>();
        if (health == null)
        {
            Debug.LogWarning("We hit something that doesn't have health.........");
        }
        else
        {
            health.ReceiveDamage(turretDataSO.damage);
        }
    }
    protected virtual Vector3 ConvertPos3(Transform pos)
    {
        return new Vector3(pos.position.x, 0.4f,pos.position.z);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Đặt màu cho Gizmos
        // Vẽ hình tròn bằng cách nối các điểm lại với nhau
        Vector3 previousPoint = transform.position + new Vector3(turretDataSO.maxShootDistance, 0, 0);
        for (int i = 1; i <= 100; i++)
        {
            float angle = i * Mathf.PI * 2 / 100; // Góc hiện tại
            Vector3 newPoint = ConvertPos3(transform) + new Vector3(Mathf.Cos(angle) * turretDataSO.maxShootDistance, 0, Mathf.Sin(angle) * turretDataSO.maxShootDistance);
            Gizmos.DrawLine(previousPoint, newPoint); // Vẽ đường thẳng giữa các điểm
            previousPoint = newPoint;
        }
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        rangeAttack.gameObject.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        rangeAttack.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Ray ray = new Ray(transform.position + Vector3.up*3, Vector3.down); // Lấy tia Ray từ vị trí chuột
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity,LayerMask.GetMask(LayerMask.LayerToName(10))))
        {
            GameObject obj = hit.collider.gameObject;
            //Vector2Int a= grid.GetMouseToArr(hit.point);
            UI_IngameManager.Instance.ShowUpgradePanel(GetComponent<TurretController>(), obj,transform.position);
        } // Kiểm tra tia Ray có trúng đối tượng nào không

        Debug.Log(hit.collider);
    }
     
}

