using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using ElementProject.gameEnums;

#pragma warning disable CS8618

public class Stats : CoreComponent
{
    private UIManager? UIManager => uiManager ?? core.GetCoreComponent(ref uiManager);
    private UIManager uiManager;

    public event Action? OnHealthZero;

    [Header("Health")]
    [SerializeField] public float MaxHealth;
    [SerializeField] public float CurrentHealth;

    [Header("Attack")]
    [SerializeField] private float baseAttack;
    private float flatAttackBonus;
    private float percentAttackBonus;
    public float Attack => (baseAttack + flatAttackBonus) * (1 + percentAttackBonus);

    [Header("Defense")]
    [SerializeField] private float baseDefense;
    private float flatDefenseBonus;
    private float percentDefenseBonus;
    public float Defense => (baseDefense + flatDefenseBonus) * (1 + percentDefenseBonus);

    [Header("Crit Rate")]
    [SerializeField] private float baseCritRate;
    private float flatCritRateBonus;
    private float percentCritRateBonus;
    public float CritRate => (baseCritRate + flatCritRateBonus) * (1 + percentCritRateBonus);

    [Header("Crit Multiplier")]
    [SerializeField] private float baseCritMultiplier;
    private float flatCritMultiplierBonus;
    private float percentCritMultiplierBonus;
    public float CritMultiplier => (baseCritMultiplier + flatCritMultiplierBonus) * (1 + percentCritMultiplierBonus);

    [Header("Attack Speed")]
    [SerializeField] private float baseAttackSpeed;
    private float flatAttackSpeedBonus;
    private float percentAttackSpeedBonus;
    public float AttackSpeed => (baseAttackSpeed + flatAttackSpeedBonus) * (1 + percentAttackSpeedBonus);

    [Header("Movement Speed")]
    [SerializeField] private float baseMovementSpeed;
    private float flatMovementSpeedBonus;
    private float percentMovementSpeedBonus;
    public float MovementSpeed => (baseMovementSpeed + flatMovementSpeedBonus) * (1 + percentMovementSpeedBonus);

    private Dictionary<ElementType, float> resistances = new();
    private Dictionary<ElementType, float> damageBonuses = new();


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
        SetHealth(MaxHealth);

        baseAttack = data.baseAttack;
        baseDefense = data.baseDefense;
        baseCritRate = data.baseCritRate;
        baseCritMultiplier = data.baseCritMultiplier;
        baseAttackSpeed = data.baseAttackSpeed;
        baseMovementSpeed = data.movementSpeed;

        resistances = data.resistances.ToDictionary(r => r.element, r => Mathf.Clamp01(r.res));
        damageBonuses = data.bonuses.ToDictionary(b => b.element, b => b.bonus);
    }

    public CombatStats GetCombatStats(ElementType element)
    {
        return new CombatStats(Attack, CritMultiplier, GetDamageBonus(element));
    }

    public void DecreaseHealth(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
            OnHealthZero?.Invoke();
        UIManager?.SetHearts(CurrentHealth, MaxHealth);
    }

    public void IncreaseHealth(float amount)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        UIManager?.SetHearts(CurrentHealth, MaxHealth);
    }

    public void SetHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(amount, 0, MaxHealth);
        UIManager?.SetHearts(CurrentHealth, MaxHealth);
    }

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

    private void ApplyStatWithBonuses(
        ref float baseValue,
        ref float flatBonus,
        ref float percentBonus,
        StatOperation op,
        float amount,
        bool isPercent)
    {
        switch (op)
        {
            case StatOperation.Increase:
                if (isPercent) percentBonus += amount;
                else flatBonus += amount;
                break;
            case StatOperation.Decrease:
                if (isPercent) percentBonus -= amount;
                else flatBonus -= amount;
                break;
            case StatOperation.Set:
                baseValue = amount;
                break;
        }
    }

    public void ModifyStat(StatType statType, StatOperation operation, float amount, ElementType element = ElementType.None, bool isPercent = false)
    {
        switch (statType)
        {
            case StatType.Health:
                if (operation == StatOperation.Increase) IncreaseHealth(amount);
                else if (operation == StatOperation.Decrease) DecreaseHealth(amount);
                else if (operation == StatOperation.Set) SetHealth(amount);
                break;

            case StatType.Attack:
                ApplyStatWithBonuses(ref baseAttack, ref flatAttackBonus, ref percentAttackBonus, operation, amount, isPercent);
                break;

            case StatType.Defense:
                ApplyStatWithBonuses(ref baseDefense, ref flatDefenseBonus, ref percentDefenseBonus, operation, amount, isPercent);
                break;

            case StatType.CritRate:
                ApplyStatWithBonuses(ref baseCritRate, ref flatCritRateBonus, ref percentCritRateBonus, operation, amount, isPercent);
                break;

            case StatType.CritMultiplier:
                ApplyStatWithBonuses(ref baseCritMultiplier, ref flatCritMultiplierBonus, ref percentCritMultiplierBonus, operation, amount, isPercent);
                break;

            case StatType.AttackSpeed:
                ApplyStatWithBonuses(ref baseAttackSpeed, ref flatAttackSpeedBonus, ref percentAttackSpeedBonus, operation, amount, isPercent);
                break;

            case StatType.MovementSpeed:
                ApplyStatWithBonuses(ref baseMovementSpeed, ref flatMovementSpeedBonus, ref percentMovementSpeedBonus, operation, amount, isPercent);
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
    MovementSpeed,
    Resistance,
    DamageBonus
}

public enum StatOperation
{
    Increase,
    Decrease,
    Set
}