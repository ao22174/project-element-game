using UnityEngine;
using System;
using ElementProject.gameEnums;
using System.Collections.Generic;


public struct ElementDamageBonuses
{
    public ElementType element;
    public float damage;

    public ElementDamageBonuses(ElementType element, float damage)
    {
        this.damage = damage;
        this.element = element;
    }
}
public class PlayerStatModifiers
{
    public int flatAttackBonus;
    public float percentAttackBonus;
    public float movementSpeedBonus;

    public float flatHPBonus;
    public float percentHPBonus;

    public int flatDefenseBonus;
    public float percentDefenseBonus;

    public float criticalChanceBonus;      // 0.15 = 15%
    public float criticalDamageBonus;      // e.g. 1.5 = 150% damage

    public float attackSpeedBonus;         // Multiplier

    public float cooldownReduction;        // 0.2 = 20% reduction
    public float healthRegenBonus;

    public float stunResistance;           // 0 to 1
    public float damageReduction;          // 0 to 1

    public float elementalDamageBonus;

    public float lifeStealPercent;

    public float knockbackResistance;

    public float experienceBonus;
    public float goldBonus;

    public List<ElementDamageBonuses> damageBonuses = new List<ElementDamageBonuses>
    {
        new ElementDamageBonuses(ElementType.Fire, 0f),
        new ElementDamageBonuses(ElementType.Water, 0f),
        new ElementDamageBonuses(ElementType.Frost, 0f),
        new ElementDamageBonuses(ElementType.Earth, 0f),
        new ElementDamageBonuses(ElementType.Lightning, 0f),
        new ElementDamageBonuses(ElementType.None, 0f),
        new ElementDamageBonuses(ElementType.Air, 0f)

    };
    private Dictionary<ElementType, float> damageBonusDict = new Dictionary<ElementType, float>();
    public PlayerStatModifiers()
    {
        RebuildDamageBonusDictionary();
    }
    public float GetDamageBonus(ElementType element)
    {
        if (damageBonusDict.TryGetValue(element, out float bonus))
            return bonus;
        return 0f;
    }

   
    public void RebuildDamageBonusDictionary()
    {
        damageBonusDict.Clear();
        foreach (var bonus in damageBonuses)
        {
            damageBonusDict[bonus.element] = bonus.damage;
        }
    }
    public void UpdateElementDamageBonus(ElementType element, float damage)
{
    // Update or add in the List
    bool updated = false;
    for (int i = 0; i < damageBonuses.Count; i++)
    {
        if (damageBonuses[i].element == element)
        {
            damageBonuses[i] = new ElementDamageBonuses(element, damage);
            updated = true;
            break;
        }
    }

    if (!updated)
        damageBonuses.Add(new ElementDamageBonuses(element, damage));

    // Sync with dictionary
    damageBonusDict[element] = damage;
}

}