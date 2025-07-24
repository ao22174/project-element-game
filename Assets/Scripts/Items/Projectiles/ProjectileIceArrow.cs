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
        IFreezable freezable = collision.GetComponentInParent<IFreezable>();
        if (damageable != null)
        {
            if (collision.GetComponentInParent<Core>().Faction == Faction.Player && faction == Faction.Player) return;
            if (collision.GetComponentInParent<Core>().Faction == Faction.Enemy && faction == Faction.Enemy) return;

            damageable.TakeDamage(new DamageInfo(core, direction, element, false,faction, damage));
            if (freezable != null) freezable.ApplyFreeze(duration);
            Destroy(gameObject);
        }
    }
}