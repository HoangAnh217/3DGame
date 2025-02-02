using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class SKillRainRocket : CastSkill
{
    [SerializeField] private ParticleSystem prefabsRainMeteorite;
    private SpriteRenderer spriteRenderer;
    protected override void Start()
    {   
        base.Start();
        // prefabsRainMeteorite.gameObject.SetActive(false);
        //prefabsRainMeteorite.shape.radius = radiusDame;
        spriteRenderer = prefabsRainMeteorite.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);
        prefabsRainMeteorite.Stop();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (timer >= 0)
        {
            Debug.Log("cant do ");
            return;
        }
        isCasting = true;
        spriteRenderer.gameObject.SetActive(true);

        Debug.Log("asdasd");


    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!isCasting)
            return;
        if (TryGetHitPosition(eventData, out RaycastHit hit))
        {   
            spriteRenderer.gameObject.SetActive(false);
            StartCoroutine(DameEffect(hit.point+Vector3.up*0.3f,4f - 0.35f-0.5f));
            base.OnEndDrag(eventData);
        }
    }
    private IEnumerator DameEffect(Vector3 pos, float time)
    {
        // Activate the meteorite prefab
        prefabsRainMeteorite.Play();
        float timerDame = 0.3f;
        int count = Mathf.FloorToInt(time / timerDame);
        yield return new WaitForSeconds(0.35f);
        for (int i = 0; i < count; i++)
        {
            // Apply damage to enemies within the radius
            yield return DealDamage(pos, 1.3f, 2, timerDame); // Radius = 0.8f, Damage = 10
        }
        yield return new WaitForSeconds(0.5f);
        prefabsRainMeteorite.Stop();
    }
    private IEnumerator DealDamage(Vector3 position, float radius, int damage,float time)
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
        yield return new WaitForSeconds(time);  // Wait before the next damage
    }
    private void OnDrawGizmosSelected()
    {
        // Màu sắc của Gizmo
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Màu đỏ, bán trong suốt

        // Vẽ Sphere để hiển thị phạm vi gây damage
        Gizmos.DrawWireSphere(skill.transform.position, 1.3f); // Thay 0.8f bằng radius nếu là biến
    }
}
