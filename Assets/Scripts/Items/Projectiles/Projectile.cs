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
    public CombatStats combatStats;
    public Core ownerCore;
    public Vector2 startPosition;
    public Vector2 direction;
    public float speed;
    public float damageScaling;
    public float lifetime;
    public int elementBuildup;
    public ElementType element;
    public Faction faction;

    public BulletInfo(
        Core ownerCore,
        CombatStats combatStats,
        Vector2 startPosition,
        Vector2 direction,
        float speed,
        float damageScaling,
        float lifetime,
        int elementBuildup,
        ElementType element,
        Faction faction)
    {
        this.ownerCore = ownerCore;
        this.combatStats = combatStats;
        this.startPosition = startPosition;
        this.direction = direction;
        this.speed = speed;
        this.damageScaling = damageScaling;
        this.lifetime = lifetime;
        this.elementBuildup = elementBuildup;
        this.element = element;
        this.faction = faction;
    }
}
public abstract class Projectile : MonoBehaviour
{
    protected Vector2 direction;
    protected float speed;
    protected CombatStats combatStats;
    protected float damageScaling;
    protected int elementBuildup;
    protected float lifetime;
    protected ElementType element;
    protected Faction faction;
    protected Core ownerCore;

    public virtual void Initialize(BulletInfo info)
    {
        ownerCore = info.ownerCore;
        combatStats = info.combatStats;
        transform.position = info.startPosition;
        direction = info.direction.normalized;
        speed = info.speed;
        damageScaling = info.damageScaling;
        lifetime = info.lifetime;
        element = info.element;
        elementBuildup = info.elementBuildup;
        faction = info.faction;

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
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