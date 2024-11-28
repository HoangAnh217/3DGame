using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints; // Mảng các điểm mốc
    public float speed = 3f; // Tốc độ di chuyển
    private int currentWaypointIndex = 0; // Chỉ số của điểm mốc hiện tại
    private Vector3 dir;
    [SerializeField] protected Transform model;
    void Start()
    {   
        dir = waypoints[0].position- transform.position;
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            // Sử dụng DOTween để di chuyển Enemy tới điểm mốc hiện tại
            transform.DOMove(waypoints[currentWaypointIndex].position, speed)
                .SetSpeedBased(true) // Đặt di chuyển theo tốc độ
                .OnComplete(() =>
                {
                    currentWaypointIndex++; // Tăng chỉ số điểm mốc
                    MoveToNextWaypoint(); // Gọi lại hàm để di chuyển đến điểm tiếp theo
                });
        }
    }
    
}
