using UnityEngine;

public class KillHungry : Buff
{
    public override string BuffName => "Kill Hungry";

    private int killCount = 0;
    private float currentAttackIncrease = 0f;

    private float AttackPerKill => data.attackIncreaseOnKill + data.attackIncreaseOnKillPerStack * stackCount;
    private KillHungryData data => (KillHungryData)buffData;

    public override void OnKill(GameObject target, Vector2 position)
    {
        base.OnKill(target, position);
        killCount++;
        UpdateAttackBonus(target);
    }

    public override void OnStackIncrease()
    {
        base.OnStackIncrease();
        UpdateAttackBonus(core.gameObject); // Recalculate with new stack count
    }

    public override void OnRoomEnter(GameObject target)
    {
        base.OnRoomEnter(target);
        ResetKillsAndBonus(); // Reset on stage/room enter
    }

    private void UpdateAttackBonus(GameObject target)
    {
        var stats = core.GetCoreComponent<Stats>();
        
        // Remove old bonus first
        stats.ModifyStat(StatType.Attack, StatOperation.Decrease, currentAttackIncrease);

        // Calculate new total bonus
        currentAttackIncrease = AttackPerKill * killCount;

        // Apply new bonus
        stats.ModifyStat(StatType.Attack, StatOperation.Increase, currentAttackIncrease);
    }

    private void ResetKillsAndBonus()
    {
        var stats = core.GetCoreComponent<Stats>();
        stats.ModifyStat(StatType.Attack, StatOperation.Decrease, currentAttackIncrease);

        killCount = 0;
        currentAttackIncrease = 0f;
    }
}
