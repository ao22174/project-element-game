using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossHurtbox : MonoBehaviour
{
    private Boss boss;

    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Vector2 direction = collision.transform.position - boss.transform.position;
            Core hitCore = collision.GetComponentInParent<Core>();
            if (hitCore != null)
            {
                if (hitCore.Faction == boss.core.Faction) return;
            }

            CombatStats combatStats = boss.core.GetCoreComponent<Stats>().GetCombatStats(ElementProject.gameEnums.ElementType.None);
            damageable.TakeDamage(new DamageInfo(boss.core,
                direction,
                boss.bossData.elementType,
                false,
                Faction.Enemy, DamageCalculator.CalculateGenericDamage(combatStats,
                 boss.bossData.jumpDamageScaling, boss.bossData.elementType,
                 collision.GetComponentInParent<Core>()),
                 boss.bossData.jumpBuildup));
        }
    }
}