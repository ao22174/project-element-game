using UnityEngine;


public class ProjectileFreezer : Projectile
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;
    public OwnedBy bulletOwner;

    public void Initialize(Vector2 startPosition, Vector2 direction, float speed, float damage, float lifetime, OwnedBy bulletOwner)
    {
        transform.position = startPosition;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.bulletOwner = bulletOwner;
        Destroy(gameObject, lifetime);
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

            Entity entity = collision.gameObject.GetComponentInParent<Entity>();
            entity.HitBullet(damage, direction);
            Destroy(gameObject);
        }
    }
}