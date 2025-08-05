using ElementProject.gameEnums;
using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateWeaponDamage(Core attackerCore, Weapon weapon, Core? defenderCore = null, bool isCrit = false)
    {
        float defenseScaling = 1000f;

        Stats attackerStats = attackerCore.GetCoreComponent<Stats>();
        Stats defenderStats = defenderCore?.GetCoreComponent<Stats>();

        float baseAttack = attackerStats.Attack;
        float weaponScaling = weapon.scaling;
        float weaponDamage = weapon.damage;
        float critMultiplier = isCrit ? 1+ attackerStats.CritMultiplier : 1f;
        float elementalBonus = 1+ attackerStats.GetDamageBonus(weapon.elementType);

        float damageBeforeDefense = (baseAttack * weaponScaling + weaponDamage) * critMultiplier * elementalBonus;

        Debug.Log($"[Weapon Damage] BaseAttack: {baseAttack}, WeaponScaling: {weaponScaling}, WeaponDamage: {weaponDamage}");
        Debug.Log($"[Weapon Damage] CritMultiplier: {critMultiplier}, ElementalBonus: {elementalBonus}");
        Debug.Log($"[Weapon Damage] DamageBeforeDefense: {damageBeforeDefense}");

        float defenseReduction = 1f;
        if (defenderStats != null)
        {
            float defense = defenderStats.Defense;
            float resistance = defenderStats.GetResistance(weapon.elementType);
            defenseReduction = (1 - (defense / (defense + defenseScaling))) * (1 - resistance);

            Debug.Log($"[Weapon Damage] Defender Defense: {defense}, Resistance: {resistance}");
            Debug.Log($"[Weapon Damage] DefenseReductionMultiplier: {defenseReduction}");
        }

        float finalDamage = damageBeforeDefense * defenseReduction;

        Debug.Log($"[Weapon Damage] FinalDamage: {finalDamage}");
        return finalDamage;
    }

    public static float CalculateReactionDamage(Core attackerCore, Reaction reaction, Core? defenderCore = null)
    {
        float defenseScaling = 1000f;

        Stats attackerStats = attackerCore.GetCoreComponent<Stats>();
        Stats defenderStats = defenderCore?.GetCoreComponent<Stats>();

        var elements = ElementalReactionLookup.GetElementsFromReaction(reaction);
        if (elements == null)
        {
            Debug.LogWarning("[Reaction Damage] Reaction not found in lookup.");
            return 0f;
        }

        ElementType e1 = elements.Value.Item1;
        ElementType e2 = elements.Value.Item2;

        float baseDamage = reaction.baseDamage;
        float bonus1 = attackerStats.GetDamageBonus(e1);
        float bonus2 = attackerStats.GetDamageBonus(e2);

        float damageBeforeDefense = baseDamage * (1 + bonus1) * (1 + bonus2);

        Debug.Log($"[Reaction Damage] BaseDamage: {baseDamage}, Bonus1({e1}): {bonus1}, Bonus2({e2}): {bonus2}");
        Debug.Log($"[Reaction Damage] DamageBeforeDefense: {damageBeforeDefense}");

        float defenseReduction = 1f;
        if (defenderStats != null)
        {
            float defense = defenderStats.Defense;
            float resist1 = defenderStats.GetResistance(e1);
            float resist2 = defenderStats.GetResistance(e2);
            defenseReduction = (1 - (defense / (defense + defenseScaling))) * (1 - resist1) * (1 - resist2);

            Debug.Log($"[Reaction Damage] Defender Defense: {defense}, Resist1({e1}): {resist1}, Resist2({e2}): {resist2}");
            Debug.Log($"[Reaction Damage] DefenseReductionMultiplier: {defenseReduction}");
        }

        float finalDamage = damageBeforeDefense * defenseReduction;

        Debug.Log($"[Reaction Damage] FinalDamage: {finalDamage}");
        return finalDamage;
    }

    public static float CalulateBuffDamage(Core attackerCore, float baseDamage, ElementType elementType, Core? defenderCore)
    {
         float defenseScaling = 1000f;
        float buffAttackScaling = 0.5f;
        Stats attackerStats = attackerCore.GetCoreComponent<Stats>();
        Stats defenderStats = defenderCore?.GetCoreComponent<Stats>();
        float buffDamage = baseDamage * attackerStats.Attack * buffAttackScaling * (attackerStats.GetDamageBonus(elementType) + 1);
        float defenseReduction = 1f;
        if (defenderStats != null)
        {
            float defense = defenderStats.Defense;
            float resistance = defenderStats.GetResistance(elementType);
            defenseReduction = (1 - (defense / (defense + defenseScaling))) * (1 - resistance);
        }
        float finalDamage = buffDamage * defenseReduction;
        return finalDamage;
    }
}