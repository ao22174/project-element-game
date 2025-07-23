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
        var target = collision.GetComponentInParent<IWeaponUser>();
        if ((target is Player && bulletOwner == OwnedBy.Player) || (target is Entity && bulletOwner == OwnedBy.Enemy)) return;

        

        IDamageable damageable = collision.GetComponentInParent<IDamageable>();
        IFreezable freezable = collision.GetComponentInParent<IFreezable>();
        if (damageable != null)
        {
            damageable.TakeDamage(new DamageInfo(gameObject, direction, element, false, bulletOwner, damage));
            if (freezable != null) freezable.ApplyFreeze(duration);
            Destroy(gameObject);
        }
    }
}