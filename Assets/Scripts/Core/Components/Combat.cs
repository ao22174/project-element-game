using System;
using UnityEngine;
public class Combat : CoreComponent, IDamageable
{
    
    public event Action<DamageInfo>? OnTakeDamage;
    public event Action<DamageInfo>? OnDeath;
    [SerializeField] private GameObject damageNumberPrefab;
    private Stats Stats { get => stats ?? core.GetCoreComponent(ref stats); }
    private Buffs Buffs { get => buffs ?? core.GetCoreComponent(ref buffs); }
    private UIManager? uiManager;
    private UIManager UIManager { get => uiManager ?? core.GetCoreComponent(ref uiManager); }

    private ElementalStatus Status { get => status ?? core.GetCoreComponent(ref status); }
    private ElementalStatus? status;

    private Buffs? buffs;
    private Stats? stats;

    public void TakeDamage(DamageInfo info)
    {

        Stats?.DecreaseHealth(info.amount);
        if (info.elementBuildup > 0)
        {
            Status?.ApplyElementalBuildup(info.element, info.elementBuildup);
        }
        UIManager?.ShowDamageNumber(info.amount.ToString(), info.element);  
        OnTakeDamage?.Invoke(info);
        
        if (stats?.CurrentHealth < 0)
        {
            OnDeath?.Invoke(info);
        }
    }



    public void OnAttack(float damage)
    {
        Buffs?.OnAttack(core.gameObject, damage, Vector2.zero);
    }
}