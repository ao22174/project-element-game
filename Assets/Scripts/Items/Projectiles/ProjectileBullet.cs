using Unity.VisualScripting;
using UnityEngine;


public class ProjectileBullet : Projectile
{

    protected override void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            Destroy(gameObject);
            return;
        }
        if (collision.GetComponentInParent<IWeaponUser>() is Player && bulletOwner == OwnedBy.Player)
        {
            return;
        }
        if (collision.GetComponentInParent<IWeaponUser>() is Entity && bulletOwner == OwnedBy.Enemy)
        {
            return;
        }

        IDamageable damageable = collision.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(new DamageInfo(gameObject, direction, element, false, bulletOwner, damage, elementBuildup));
            Destroy(gameObject);
        }


    }
}