using UnityEngine;
using System.Collections.Generic;   
using ElementProject.gameEnums;

public class Explosion : MonoBehaviour
{
    private float explosionRadius = 5f;
    private float explosionDamage = 50f;
    [SerializeField] private ElementType elementType = ElementType.Fire;
    [SerializeField] private LayerMask damageLayer;
    private Core sourceCore;
    private CombatStats combatStats;
    
    public void Initialize(Vector2 position, float explosionRadius, float explosionDamage, ElementType elementType, LayerMask damageLayer, CombatStats combatStats, Core source = null)
    {
        sourceCore = source;
        this.explosionRadius = explosionRadius;
        this.explosionDamage = explosionDamage;
        this.elementType = elementType;
        this.damageLayer = damageLayer;
        this.combatStats = combatStats;
        Detonate(position);
    }
    void Detonate(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, explosionRadius, damageLayer);

        foreach (var hit in hits)
        {
            Core hitCore = hit.GetComponentInParent<Core>();
            if (hitCore == null) continue;
            
                Debug.Log($"Hit: {hit.name} with core: {hitCore?.name}");
                if (hitCore.Faction == sourceCore.Faction && hitCore.Faction != Faction.Neutral) //blow up parameters
                    continue;
            

            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(new DamageInfo(sourceCore, Vector2.zero, elementType, false, sourceCore.Faction, DamageCalculator.CalculateGenericDamage(combatStats, explosionDamage, ElementType.Fire, hitCore)));
                
        }
        Destroy(gameObject, 0.5f);
    }
}
