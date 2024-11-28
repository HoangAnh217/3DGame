using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private WayPoint[] nextPoints;
    
    public Transform chooseNextPoint()
    {
        if (nextPoints.Length==0)
        {
            return null;
        }
        if (nextPoints.Length > 1)
        {
            return nextPoints[Random.Range(0, nextPoints.Length)].transform;
        }
        else return nextPoints[0].transform;
    }
}
