using Unity.VisualScripting;
using UnityEngine;


public class ProjectileBullet : Projectile
{

    protected override void HandleCollision(Collider2D collision)
    {
            Debug.Log("Hit: " + collision.name);
        if (collision.CompareTag("Structure"))
        {
            Destroy(gameObject);
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (collision.GetComponentInParent<Core>().Faction == Faction.Player && faction == Faction.Player) return;
            if (collision.GetComponentInParent<Core>().Faction == Faction.Enemy && faction == Faction.Enemy) return;
            Debug.Log(collision.GetComponentInParent<Core>().Faction);
            damageable.TakeDamage(new DamageInfo(core, direction, element, false,faction, damage, elementBuildup));
            Destroy(gameObject);
        }


    }
}