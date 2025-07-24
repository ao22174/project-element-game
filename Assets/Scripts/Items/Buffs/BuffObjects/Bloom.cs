using UnityEngine;
using System;
using ElementProject.gameEnums;
public class Bloom : MonoBehaviour
{

    private float damage;
    [SerializeField]private float damageRadius;

    private bool armed = false;
    private float detonationTime;
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
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius);

    foreach (var hit in hits)
    {
        Entity enemy = hit.GetComponentInParent<Entity>();
        if (enemy != null)
        {
                hit.GetComponent<IDamageable>().TakeDamage(new DamageInfo(core, Vector2.zero, ElementType.Nature, false, core.Faction, damage));
        }
    }
    GameObject explosion = GameObject.Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
    Destroy(gameObject);
    }

}