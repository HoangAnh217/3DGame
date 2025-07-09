using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dragon : Entity
{ 
    [SerializeField] private Material materials;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        materials = model.GetComponentInChildren<Renderer>().material;
    }   
    protected override void Start()
    {
        base.Start();
    }
    public override void OnDead()
    {

        Debug.Log("dead");

        StartCoroutine(EffectDead());
    }
    
    /* public void SetValue(float value)
     {
         for (int i = 0; i < materials.Count; i++)
         {
             materials[i].SetFloat("_Dissolve", value);
         }
     }*/
    public override void OnEnable()
    {
        base.OnEnable();
        materials.SetFloat("_Dissolve", 0);
    }
    private IEnumerator EffectDead()
    {
        float value = 0;
        while (value < 1)
        {
            value += Time.deltaTime;  // Gradually increase the dissolve value
            materials.SetFloat("_Dissolve", value);  // Set the dissolve effect value
            yield return null;  // Wait for the next frame
        }

        // Once the dissolve is complete, call the base method
        base.OnDead();
    }
}
