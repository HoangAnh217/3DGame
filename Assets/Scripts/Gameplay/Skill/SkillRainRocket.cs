using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class SKillRainRocket : CastSkill
{
    [SerializeField] private VisualEffect prefabsRainMeteorite;
    protected override void Start()
    {   
        base.Start();
        prefabsRainMeteorite.gameObject.SetActive(false);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (TryGetHitPosition(eventData, out RaycastHit hit))
        {
            StartCoroutine(DameEffect(hit.point+Vector3.up*0.3f,2f));
            base.OnEndDrag(eventData);
        }
    }
    private IEnumerator DameEffect(Vector3 pos,float time)
    {
        // Activate the meteorite prefab
        prefabsRainMeteorite.gameObject.SetActive(true);
        prefabsRainMeteorite.Play();
        int count = Mathf.FloorToInt(time/0.2f);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < count; i++)
        {
            // Apply damage to enemies within the radius
            yield return DealDamage(pos, 0.8f, 1); // Radius = 0.8f, Damage = 10
        }
        prefabsRainMeteorite.Stop();
        yield return new WaitForSeconds(1.7f);
        prefabsRainMeteorite.gameObject.SetActive(false );
        skill.SetActive(false);
    }

    private IEnumerator DealDamage(Vector3 position, float radius, int damage)
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Enemy"));
        foreach (var enemy in enemiesInRange)
        {
            Entity health = enemy.gameObject.GetComponent<Entity>();
            if (health == null)
            {
                Debug.LogWarning("We hit something that doesn't have health...");
            }
            else
            {
                health.ReceiveDamage(damage);
            }
        }
        yield return new WaitForSeconds(0.2f);  // Wait before the next damage
    }
}
