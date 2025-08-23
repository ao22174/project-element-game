using ElementProject.gameEnums;
using UnityEngine;
public struct CombatStats
{
    public float Attack;
    public float CritMultiplier;
    public float ElementBonus;

    public CombatStats(float attack, float critMultiplier, float elementBonus)
    {
        Attack = attack;
        CritMultiplier = critMultiplier;
        ElementBonus = elementBonus;
    }
}
public static class DamageCalculator
{
    public static float CalculateGenericDamage(CombatStats attackerStats, float damageScaling, ElementType element, Core? defenderCore = null, bool isCrit = false)
    {
        float defenseScaling = 1000f;
        Stats defenderStats = defenderCore?.GetCoreComponent<Stats>();

        float baseAttack = attackerStats.Attack;
        float critMultiplier = isCrit ? 1 + attackerStats.CritMultiplier : 1f;
        float elementalBonus = 1 + attackerStats.ElementBonus;

        float damageBeforeDefense = baseAttack * damageScaling * critMultiplier * elementalBonus;


        float defenseReduction = 1f;
        if (defenderStats != null)
        {
            float defense = defenderStats.Defense;
            float resistance = defenderStats.GetResistance(element);
            defenseReduction = (1 - (defense / (defense + defenseScaling))) * (1 - resistance);

        }

        float finalDamage = damageBeforeDefense * defenseReduction;

        Debug.Log($"[Weapon Damage] FinalDamage: {finalDamage}");
        return finalDamage;
    }

    public static float CalculateReactionDamage(CombatStats attackerStats, float ReactionScaling, ElementType e1, ElementType e2, Core? defenderCore = null)
    {
        float defenseScaling = 1000f;
        Stats defenderStats = defenderCore?.GetCoreComponent<Stats>();


        float bonus1 = 0f;
        float bonus2 = 0f;

        float damageBeforeDefense = attackerStats.Attack * ReactionScaling * (1 + bonus1) * (1 + bonus2);


        float defenseReduction = 1f;
        if (defenderStats != null)
        {
            float defense = defenderStats.Defense;
            float resist1 = defenderStats.GetResistance(e1);
            float resist2 = defenderStats.GetResistance(e2);
            defenseReduction = (1 - (defense / (defense + defenseScaling))) * (1 - resist1) * (1 - resist2);

        }

        float finalDamage = damageBeforeDefense * defenseReduction;

        Debug.Log($"[Reaction Damage] FinalDamage: {finalDamage}");
        return finalDamage;
    }

}