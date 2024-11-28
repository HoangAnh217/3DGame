using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimtorController : MonoBehaviour
{
    private Entity entity;
    private void Start()
    {
        entity = GetComponentInParent<Entity>();
    }
    public void OnDead()
    {
        entity.OnDead();
    }
    public void OnAttack()
    {
        entity.OnAttack();
    }
}
