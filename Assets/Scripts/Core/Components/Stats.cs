using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using ElementProject.gameEnums;
public class Stats : CoreComponent
{
    private UIManager? UIManager { get => uiManager ?? core.GetCoreComponent(ref uiManager); }
    private UIManager uiManager;
    public event Action? OnHealthZero;
    
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Attack { get; private set; }
    public float Defense { get; private set; }
    public float CritRate { get; private set; }
    public float CritMultiplier { get; private set; }

    public float attackSpeed{ get; private set; }

    private Dictionary<ElementType, float> resistances;
    private Dictionary<ElementType, float> damageBonuses;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init(CoreData coreData)
    {
        base.Init(coreData);
        SetStats(coreData);
    }

    public void SetStats(CoreData data)
    {
        MaxHealth = data.baseMaxHealth;
        IncreaseHealth(MaxHealth);
        Attack = data.baseAttack;
        Defense = data.baseDefense;
        CritRate = data.baseCritRate;
        CritMultiplier = data.baseCritMultiplier;

        resistances = data.resistances.ToDictionary(r => r.element, r => Mathf.Clamp01(r.res));
        damageBonuses = data.bonuses.ToDictionary(b => b.element, b => b.bonus);
    }

    public void DecreaseHealth(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
            OnHealthZero?.Invoke();
        UIManager?.SetHearts(CurrentHealth,MaxHealth);
    }

    public void IncreaseHealth(float amount)
    {
        CurrentHealth = Mathf.Max(MaxHealth, CurrentHealth += amount);
        UIManager?.SetHearts(CurrentHealth, MaxHealth);

    }

    // Crit Rate Handling
    public void IncreaseCritRate(float amount) => CritRate += amount;
    public void DecreaseCritRate(float amount) => CritRate = Mathf.Max(0f, CritRate - amount);

    // Attack Spped
    public void IncreaseAttackSpeed(float amount) => attackSpeed += amount;
    public void DecreaseAttackSpeed(float amount) => attackSpeed = Mathf.Max(1f, attackSpeed - amount);

    // Crit Multiplier
    public void IncreaseCritMultiplier(float amount) => CritMultiplier += amount;
    public void DecreaseCritMultiplier(float amount) => CritMultiplier = Mathf.Max(1f, CritMultiplier - amount);

    // Elemental Resistance
    public float GetResistance(ElementType element) =>
        resistances.TryGetValue(element, out float res) ? res : 0f;

    public void IncreaseResistance(ElementType element, float amount)
    {
        if (!resistances.ContainsKey(element))
            resistances[element] = 0f;

        resistances[element] = Mathf.Clamp01(resistances[element] + amount);
    }

    public void DecreaseResistance(ElementType element, float amount)
    {
        if (!resistances.ContainsKey(element))
            resistances[element] = 0f;

        resistances[element] = Mathf.Clamp01(resistances[element] - amount);
    }

    // Elemental Bonus
    public float GetDamageBonus(ElementType element) =>
        damageBonuses.TryGetValue(element, out float bonus) ? bonus : 0f;

    public void IncreaseDamageBonus(ElementType element, float amount)
    {
        if (!damageBonuses.ContainsKey(element))
            damageBonuses[element] = 0f;

        damageBonuses[element] += amount;
    }

    public void DecreaseDamageBonus(ElementType element, float amount)
    {
        if (!damageBonuses.ContainsKey(element))
            damageBonuses[element] = 0f;

        damageBonuses[element] -= amount;
    }
}
