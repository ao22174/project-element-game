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
    
    public float MovementSpeed { get; private set; }

    public float attackSpeed { get; private set; }

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
        MovementSpeed = data.movementSpeed;
        attackSpeed = data.baseAttackSpeed; // will be calculated linearly, 2.0 attack speed means double bullets comparetilvely to 1.0 attacks peed

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

    public void SetHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(amount, 0, MaxHealth);
        UIManager?.SetHearts(CurrentHealth, MaxHealth);
    }


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

    private void IncreaseDamageBonus(ElementType element, float amount)
    {
        if (!damageBonuses.ContainsKey(element))
            damageBonuses[element] = 0f;

        damageBonuses[element] += amount;
    }

    private void DecreaseDamageBonus(ElementType element, float amount)
    {
        if (!damageBonuses.ContainsKey(element))
            damageBonuses[element] = 0f;

        damageBonuses[element] -= amount;
    }

    public void ModifyStat(StatType statType, StatOperation operation, float amount, ElementType element = ElementType.None)
    {
        switch (statType)
        {
            case StatType.Health:
                if (operation == StatOperation.Increase) IncreaseHealth(amount);
                else if (operation == StatOperation.Decrease) DecreaseHealth(amount);
                else if (operation == StatOperation.Set) CurrentHealth = amount;
                break;
            case StatType.Attack:
                if (operation == StatOperation.Increase) Attack += amount;
                else if (operation == StatOperation.Decrease) Attack -= amount;
                else if (operation == StatOperation.Set) Attack = amount;
                break;
            case StatType.Defense:
                if (operation == StatOperation.Increase) Defense += amount;
                else if (operation == StatOperation.Decrease) Defense -= amount;
                else if (operation == StatOperation.Set) Defense = amount;
                break;
            case StatType.CritRate:
                if (operation == StatOperation.Increase) CritRate += amount;
                else if (operation == StatOperation.Decrease) CritRate = Mathf.Max(0f, CritRate - amount);
                else if (operation == StatOperation.Set) CritRate = amount;
                break;
            case StatType.CritMultiplier:
                if (operation == StatOperation.Increase) CritMultiplier += amount;
                else if (operation == StatOperation.Decrease) CritMultiplier = Mathf.Max(1f, CritMultiplier - amount);
                else if (operation == StatOperation.Set) CritMultiplier = amount;
                break;
            case StatType.AttackSpeed:
                if (operation == StatOperation.Increase) attackSpeed += amount;
                else if (operation == StatOperation.Decrease) attackSpeed = Mathf.Max(1f, attackSpeed - amount);
                else if (operation == StatOperation.Set) attackSpeed = amount;
                break;
            case StatType.Resistance:
                if (element == ElementType.None) break;
                if (operation == StatOperation.Increase) IncreaseResistance(element, amount);
                else if (operation == StatOperation.Decrease) DecreaseResistance(element, amount);
                else if (operation == StatOperation.Set) resistances[element] = Mathf.Clamp01(amount);
                break;
            case StatType.DamageBonus:
                if (element == ElementType.None) break;
                if (operation == StatOperation.Increase) IncreaseDamageBonus(element, amount);
                else if (operation == StatOperation.Decrease) DecreaseDamageBonus(element, amount);
                else if (operation == StatOperation.Set) damageBonuses[element] = amount;
                break;
            default:
                Debug.LogWarning("Unknown stat type: " + statType);
                break;
        }
    }
}

public enum StatType
{
    Health,
    Attack,
    Defense,
    CritRate,
    CritMultiplier,
    AttackSpeed,
    Resistance,
    DamageBonus
}

public enum StatOperation
{
    Increase,
    Decrease,
    Set
}
