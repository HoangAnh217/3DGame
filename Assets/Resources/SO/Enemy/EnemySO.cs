using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemySO : ScriptableObject
{
    public int id;
    public string enemyName;
    public int health;
    public float speed;
    public int dameAttack;
    public int moneyForDead;
    /*public Sprite enemySprite;
    public GameObject enemyPrefab;*/
}
