using UnityEngine;

public class RampingAttackSpeedBuff : Buff
{
    public override string BuffName => "Ramp UP";

    private float currentBoost = 0f;

    public override void OnAttack(GameObject target, float damage, Vector2 direction = default)
    {
        RampUPData data = (RampUPData)buffData;

        Core core = target.GetComponent<Core>();
        if (core == null) return;

        var stats = core.GetCoreComponent<Stats>();
        if (stats == null) return;

        if (currentBoost >= data.maxAttackSpeedBeforeReset)
        {
            stats.ModifyStat(StatType.AttackSpeed, StatOperation.Decrease, currentBoost);
            currentBoost = 0f;
        }
        else
        {
            currentBoost += 0.1f;
            stats.ModifyStat(StatType.AttackSpeed, StatOperation.Increase, 0.1f);
        }
    }
}