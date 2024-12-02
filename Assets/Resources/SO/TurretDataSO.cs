using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "ScriptableObjects/TurretData", order = 0)]
public class TurretDataSO : ScriptableObject
{
    public int id;
    public string nameTurret = "Turret";
    public int cost;
    public float damage;
    public float maxShootDistance;
    public int upgradeCost;
    public float fireRate;

    // Tham chiếu đến TurretDataSO của cấp độ tiếp theo (nếu có)
    public TurretDataSO nextLevel;
}
