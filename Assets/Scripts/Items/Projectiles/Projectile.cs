using System.Threading;
using ElementProject.gameEnums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
public enum OwnedBy
{
    Player,
    Enemy,
    All
}

public struct BulletInfo
{
    public Core core;
    public Vector2 startPosition;
    public Vector2 direction;
    public float speed;
    public float damage;
    public float lifetime;
    public int elementBuildup;
    public ElementType element;
    public Faction faction;

    public BulletInfo(
        Core core,
        Vector2 startPosition,
        Vector2 direction,
        float speed,
        float damage,
        float lifetime,
        int elementBuildup,
        ElementType element,
        Faction faction)
    {
        this.core = core;
        this.startPosition = startPosition;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.elementBuildup = elementBuildup;
        this.element = element;
        this.faction = faction;
    }
}
public abstract class Projectile : MonoBehaviour
{
    protected Core core;
    protected Vector2 direction;
    protected float speed;
    protected float damage;
    protected int elementBuildup;
    protected float lifetime;
    protected ElementType element;
    protected Faction faction;

    public virtual void Initialize(BulletInfo info)
    {
        transform.position = info.startPosition;
        direction = info.direction.normalized;
        speed = info.speed;
        damage = info.damage;
        lifetime = info.lifetime;
        element = info.element;
        elementBuildup = info.elementBuildup;
        faction = info.faction;
        core = info.core;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        CustomUpdate();
    }

    protected virtual void CustomUpdate() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected abstract void HandleCollision(Collider2D collision);
}