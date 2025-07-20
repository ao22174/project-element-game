using UnityEngine;
using UnityEngine.Scripting;

public class RampingAttackSpeedBuff : Buff
{
    public override string BuffName => "Ramp UP";


    public override void OnAttack(GameObject target, float damage, Vector2 direction = default)
    {
        RampUPData data = (RampUPData)buffData;
        target.GetComponent<Player>().stats.attackSpeedBonus +=data.increments;
        if (target.GetComponent<Player>().stats.attackSpeedBonus >= data.maxAttackSpeedBeforeReset) target.GetComponent<Player>().stats.attackSpeedBonus = 0f;
        
    }
}