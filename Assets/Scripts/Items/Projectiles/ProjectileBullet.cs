using Unity.VisualScripting;
using UnityEngine;


public class ProjectileBullet : Projectile
{

    protected override void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player") && bulletOwner == OwnedBy.Enemy)
        {

            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (player.isInvincible) return;
            player.HealthModify(-damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy") && bulletOwner == OwnedBy.Player)
        {

            Entity entity = collision.gameObject.GetComponentInParent<Entity>();
            entity.Hit(damage, gameObject, bulletOwner);
            Destroy(gameObject);
        }
    }
}