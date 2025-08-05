using UnityEngine;
using ElementProject.gameEnums;
using UnityEditor.Rendering;
public class Melt : Reaction
{
    public override string ReactionName => "Melt";
    public override float baseDamage => 20f;
    public Melt()
    {

    }
    public override void Apply(Core core, Core source = null)
    {
        core.GetCoreComponent<Combat>()?.TakeDamage(new DamageInfo
        {
            amount =DamageCalculator.CalculateReactionDamage(source, this, core),
            element = ElementType.Fire,
            sourceCore = source
        });
        if (core.GetCoreComponent<Status>().IsFrozen)
        {
            core.GetCoreComponent<Combat>()?.TakeDamage(new DamageInfo
            {
                amount =DamageCalculator.CalculateReactionDamage(source, this, core),
                element = ElementType.Fire,
                sourceCore = source
            });
            core.GetCoreComponent<Status>().RemoveFreeze();
        }
    }
    }
