using UnityEngine;
using ElementProject.gameEnums;
using UnityEditor.Rendering;
public class Melt : Reaction
{
    public override string ReactionName => "Melt";
    public override float baseScaling => 0.2f;
    public Melt()
    {

    }
    public override void Apply(Core core, Core source = null)
    {
        CombatStats stats = source.GetCoreComponent<Stats>().GetCombatStats(ElementType.Fire);
        core.GetCoreComponent<Combat>()?.TakeDamage(new DamageInfo
        {
            amount =DamageCalculator.CalculateReactionDamage(stats, baseScaling, ElementType.Fire, ElementType.Frost, core),
            element = ElementType.Fire,
            sourceCore = source
        });
        if (core.GetCoreComponent<Status>().IsFrozen)
        {
            core.GetCoreComponent<Combat>()?.TakeDamage(new DamageInfo
            {
            amount =DamageCalculator.CalculateReactionDamage(stats, baseScaling, ElementType.Fire, ElementType.Frost, core),
                element = ElementType.Fire,
                sourceCore = source
            });
            core.GetCoreComponent<Status>().RemoveFreeze();
        }
    }
    }
