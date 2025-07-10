using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dragon : Entity
{ 
    protected override void LoadComponent()
    {
        base.LoadComponent();
    }   
    protected override void Start()
    {
        base.Start();
    }
    public override void OnDead()
    {
        Debug.Log("dead");
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }
}
