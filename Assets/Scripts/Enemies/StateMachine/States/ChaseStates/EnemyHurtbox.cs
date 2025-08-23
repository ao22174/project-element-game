using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    private ChargerEntity charger;

    private void Awake()
    {
        charger = GetComponentInParent<ChargerEntity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Vector2 direction = collision.transform.position - charger.transform.position;
            Core hitCore = collision.GetComponentInParent<Core>();
            if (hitCore != null)
            {
                if (hitCore.Faction == charger.core.Faction) return;
            }

            CombatStats combatStats = charger.core.GetCoreComponent<Stats>().GetCombatStats(ElementProject.gameEnums.ElementType.None);
            damageable.TakeDamage(new DamageInfo(charger.core,
                direction,
                charger.chargerData.elementType,
                false,
                Faction.Enemy, DamageCalculator.CalculateGenericDamage(combatStats,
                 charger.chargerData.chargeDamageScaling, charger.chargerData.elementType,
                 collision.GetComponentInParent<Core>()),
                 charger.chargerData.chargeBuildup));
        }
    }
}