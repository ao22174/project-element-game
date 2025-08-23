using UnityEngine;

public class Missile : HomingProjectile
{
    [SerializeField]private GameObject explosionPrefab;
    LayerMask mask => LayerMask.GetMask("Damageable");

    public void Initialize(BulletInfo info, Transform? target)
    {
        base.Initialize(info, target);
    }
    protected override void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            Explode(transform.position);
            return;
        }
        Core? hitCore = collision.GetComponentInParent<Core>();
        if (hitCore == null || hitCore.Faction == faction)
            return;

        Explode(transform.position);
    }

    private void Explode(Vector2 position)
    {
        Explosion explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>();
        explosion.Initialize(position, 1f, damageScaling, element, mask,combatStats, ownerCore);
        
        Destroy(gameObject);
    }
}
