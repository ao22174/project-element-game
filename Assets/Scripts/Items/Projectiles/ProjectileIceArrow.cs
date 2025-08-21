using UnityEngine;
using UnityEngine.Rendering;


public class ProjectileIceArrow : Projectile
{
    public float duration;
    public void Initialize(BulletInfo info, float duration)
    {

        base.Initialize(info);
        this.duration = duration;
    }
    protected override void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            Destroy(gameObject);
            return;
        }
        IDamageable damageable = collision.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            Core hitCore = collision.GetComponentInParent<Core>();
            if (hitCore != null)
            {
                if (hitCore.Faction == Faction.Player && faction == Faction.Player) return;
                if (hitCore.Faction == Faction.Enemy && faction == Faction.Enemy) return;
                Debug.Log(collision.GetComponentInParent<Core>().Faction);
                var freezeable = hitCore.GetCoreComponent<Status>();// NEEDS TO BE CHANGED TO IFREEZABLE LATER ON
            if (freezeable != null)
            {
                freezeable.ApplyFreeze(duration);
            }
            }
            damageable.TakeDamage(new DamageInfo(ownerCore, direction, element, false, faction,
                 DamageCalculator.CalulateBuffDamage(ownerCore, damage, ElementProject.gameEnums.ElementType.Frost, hitCore), elementBuildup));
            Destroy(gameObject);
            

        }
        
        
    }
   
}