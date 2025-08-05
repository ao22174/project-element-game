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
        if (collision.CompareTag("Melee"))
        {
            Destroy(gameObject);
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Core hitCore = collision.GetComponentInParent<Core>();
            if (hitCore != null)
            {
                Debug.Log("hitting player");
                if (hitCore.Faction == Faction.Player && faction == Faction.Player) return;
                if (hitCore.Faction == Faction.Enemy && faction == Faction.Enemy) return;
                Debug.Log(collision.GetComponentInParent<Core>().Faction);
                damageable.TakeDamage(new DamageInfo(ownerCore, direction, element, false, faction, DamageCalculator.CalculateWeaponDamage(ownerCore, weapon, hitCore), elementBuildup));

            }
                        Destroy(gameObject);

        }


    }
}