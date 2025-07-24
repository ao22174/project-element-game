using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Scripting;

public class RampingAttackSpeedBuff : Buff
{
    public override string BuffName => "Ramp UP";

    private Stats stats;
    private float currentBoost = 0f;

    public override void OnAttack(GameObject target, float damage, Vector2 direction = default)
    {
        RampUPData data = (RampUPData)buffData;

        Core core = target.GetComponent<Core>();
        if (core == null) return; 

        stats = core.GetCoreComponent<Stats>(ref stats);
        if (stats == null) return; 
        if (currentBoost >= data.maxAttackSpeedBeforeReset)
        {
            stats.DecreaseAttackSpeed(currentBoost);
            currentBoost = 0f;
        }
        else
        {
            currentBoost += 0.1f;
            stats.IncreaseAttackSpeed(0.1f);
        }
    }
}