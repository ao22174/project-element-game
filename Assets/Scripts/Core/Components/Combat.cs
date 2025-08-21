using System;
using UnityEngine;
public class Combat : CoreComponent, IDamageable
{
    public Faction Faction => core.Faction;
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
            Status?.ApplyElementalBuildup(info.element, info.elementBuildup, info.sourceCore);
        }
        UIManager?.ShowDamageNumber(info.amount, info.element);
        UIManager?.Flash();
        Debug.Log(info.sourceCore.Faction);
        info.sourceCore.GetCoreComponent<Buffs>().OnHitEnemy(info, gameObject);
        if (core.GetCoreComponent<Movement>().canKnockback)
        {
                    core.GetCoreComponent<Movement>()?.Knockback(0.5f, info.direction);
    
        }
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }
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