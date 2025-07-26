using UnityEngine;
using System;
using ElementProject.gameEnums;
public class Bloom : MonoBehaviour
{

    private float damage;
    [SerializeField]private float damageRadius;

    private bool armed = false;
    private float detonationTime;
    [SerializeField] private LayerMask damageLayer;


    private Core core;
    [SerializeField] private GameObject explosionPrefab;

    private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, damageRadius);
}
    public void Initialize(float damage, float damageRadius, float detonation, Core core)
    {
        this.damage = damage;
        this.core = core;
        this.damageRadius = damageRadius;
        detonationTime = Time.time + detonation;
        armed = true;
    }

    private void Update()
    {
        if (Time.time >= detonationTime && armed) Detonate();
    }
    private void Detonate()
    {
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius, damageLayer);

    foreach (var hit in hits)
    {
        Core hitCore = hit.GetComponentInParent<Core>();
        Debug.Log($"Hit: {hit.name} with core: {hitCore?.name}");
        if (hitCore != null && hitCore.Faction == core.Faction)
                continue; 

        IDamageable damageable = hit.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(new DamageInfo(core, Vector2.zero, ElementType.Nature, false, core.Faction, damage));
    }

    GameObject.Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
    Destroy(gameObject);
    }

}