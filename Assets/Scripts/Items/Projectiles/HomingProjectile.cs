
using ElementProject.gameEnums;
using UnityEngine;

public abstract class HomingProjectile : MonoBehaviour
{
    protected Vector2 direction;
    protected float speed;
    protected float damage;
    protected int elementBuildup;
    protected float lifetime;
    protected ElementType element;
    protected Faction faction;
    protected Weapon? weapon;
    protected Core ownerCore;
    protected Transform? target;
    [SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private float targetSearchRadius = 10f;

    public virtual void Initialize(BulletInfo info, Transform? target)
    {
        weapon = info.weapon;
        ownerCore = info.ownerCore;
        transform.position = info.startPosition;
        direction = info.direction.normalized;
        speed = info.speed;
        damage = info.damage;
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


    protected virtual void CustomUpdate()
    {
        if (target == null)
        {
            AcquireTarget();
        }

        if (target != null)
        {
            Vector2 directionToTarget = ((Vector2)target.position - (Vector2)transform.position).normalized;
            float angle = Vector3.Cross(-transform.right, directionToTarget).z;
            float angularVelocity = -angle * rotateSpeed;

            transform.Rotate(Vector3.forward, angularVelocity * Time.deltaTime);
            direction = ((Vector2)transform.right).normalized;
        }
    }
    protected virtual void AcquireTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetSearchRadius);
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (var hit in hits)
        {
            Core core = hit.GetComponentInParent<Core>();
            if (core !=null)
            {
                if (core.Faction != faction) // Don't target same faction
                {
                    float distance = Vector2.Distance(transform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = hit.transform;
                    }
                }
            }
        }

        target = closestTarget;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected abstract void HandleCollision(Collider2D collision);
}