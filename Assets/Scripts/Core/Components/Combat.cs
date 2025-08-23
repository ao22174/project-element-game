using System;
using UnityEngine;

public class Combat : CoreComponent, IDamageable
{
    public Faction Faction => core?.Faction ?? Faction.Neutral;

    public event Action<DamageInfo>? OnTakeDamage;
    public event Action<DamageInfo>? OnDeath;

    [SerializeField] private GameObject damageNumberPrefab;

    private Stats Stats => stats ?? core?.GetCoreComponent(ref stats);
    private Buffs Buffs => buffs ?? core?.GetCoreComponent(ref buffs);
    private ElementalStatus Status => status ?? core?.GetCoreComponent(ref status);
    private UIManager UIManager => uiManager ?? core?.GetCoreComponent(ref uiManager);

    private Stats? stats;
    private Buffs? buffs;
    private ElementalStatus? status;
    private UIManager? uiManager;

    public void TakeDamage(DamageInfo info)
    {
        // 1. Apply health reduction
        Stats?.DecreaseHealth(info.amount);

        // 2. Apply elemental buildup if present
        if (info.elementBuildup > 0)
        {
            Status?.ApplyElementalBuildup(info.element, info.elementBuildup, info.sourceCore);
        }

        // 3. Show damage UI
        UIManager?.ShowDamageNumber(info.amount, info.element);
        UIManager?.Flash();

        // 4. Trigger on-hit buffs on the source, if it exists
        if (info.sourceCore != null)
        {
            Buffs sourceBuffs = info.sourceCore.GetCoreComponent<Buffs>();
            sourceBuffs?.OnHitEnemy(info, gameObject);
        }

        // 5. Apply knockback if this entity can be knocked back
        if (core != null)
        {
            Movement movement = core.GetCoreComponent<Movement>();
            if (movement?.canKnockback == true)
            {
                movement.Knockback(0.5f, info.direction);
            }
        }

        // 6. Play hit animation
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        // 7. Check for death
        if (Stats?.CurrentHealth <= 0)
        {
            OnDeath?.Invoke(info);
        }

        // 8. Trigger general OnTakeDamage event
        OnTakeDamage?.Invoke(info);
    }

    public void OnAttack(float damage)
    {
        Buffs?.OnAttack(core?.gameObject, damage, Vector2.zero);
    }
}
