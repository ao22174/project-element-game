using UnityEngine;
public enum OwnedBy
{
    Player,
    Enemy,
    All
}
public abstract class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;
    public OwnedBy bulletOwner;

    public virtual void Initialize(Vector2 startPosition, Vector2 direction, float speed, float damage, float lifetime, OwnedBy bulletOwner)
    {
        transform.position = startPosition;
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.bulletOwner = bulletOwner;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Basic movement (optional for base class)
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        CustomUpdate(); // Allow child-specific behavior
    }

    protected virtual void CustomUpdate() { }

    // Optional: Base collision filtering
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected abstract void HandleCollision(Collider2D collision);
}