using UnityEngine;


public class ProjectileFreezer : Projectile
{
    public float duration;
    public void Initialize(Vector2 startPosition, Vector2 direction, float speed, float damage, float lifetime, float duration, IWeaponUser user)
    {

        base.Initialize(startPosition, direction, speed, damage, lifetime, user);
        this.duration = duration;
    }
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
            Debug.Log("should go here");
            Entity entity = collision.gameObject.GetComponentInParent<Entity>();
            entity.EnterFreezeState(duration);
            entity.Hit(damage, gameObject, bulletOwner);
            Destroy(gameObject);
        }
    }
}