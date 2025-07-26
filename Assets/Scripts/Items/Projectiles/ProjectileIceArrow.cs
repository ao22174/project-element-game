using UnityEngine;


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
            core = collision.GetComponentInParent<Core>();
            if (core == null) return;
            if (core.Faction == Faction.Player && faction == Faction.Player) return;
            if (core.Faction == Faction.Enemy && faction == Faction.Enemy) return;
            damageable.TakeDamage(new DamageInfo(core, direction, element, false, faction, damage));

            Destroy(gameObject);
            var freezeable = core.GetCoreComponent<Status>();// NEEDS TO BE CHANGED TO IFREEZABLE LATER ON
            if (freezeable != null)
            {
            freezeable.ApplyFreeze(duration);
            }
        }
        
    }
   
}