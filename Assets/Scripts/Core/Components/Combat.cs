using System;
using UnityEngine;
public class Combat : CoreComponent, IDamageable
{

    public event Action<DamageInfo>? OnTakeDamage;
    public event Action<DamageInfo>? OnDeath;
    private Stats Stats { get => stats ?? core.GetCoreComponent(ref stats); }
    private Buffs Buffs { get => buffs ?? core.GetCoreComponent(ref buffs); }

    private Buffs buffs;
    private Stats stats;

    public void TakeDamage(DamageInfo info)
    {
        Stats?.DecreaseHealth(info.amount);
        if (stats.CurrentHealth < 0)
        {
            OnDeath?.Invoke(info);
        }
    }
}