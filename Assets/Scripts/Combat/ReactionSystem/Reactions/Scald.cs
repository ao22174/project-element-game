using UnityEngine;
using System.Collections;
using ElementProject.gameEnums;

//Water + FIRE
public class ScaldReaction : Reaction
{
    public override string ReactionName => "Scald";
    public override float baseScaling => totalScaling;
    float totalScaling;
    private float duration;
    private int ticks = 5; // Number of ticks

    public ScaldReaction(float totalScaling, float duration)
    {
        this.totalScaling = totalScaling;
        this.duration = duration;
    }

    public override void Apply(Core core, Core source)
    {
        core.StartCoroutine(ApplyScaldDoT(core, duration, ticks, source));
    }

    private IEnumerator ApplyScaldDoT(Core core, float duration, int ticks, Core source)
    {
        CombatStats stats = source.GetCoreComponent<Stats>().GetCombatStats(ElementType.Fire);

        float tickInterval = duration / ticks;
        float damagePerTick = DamageCalculator.CalculateReactionDamage(stats, totalScaling/ticks, ElementType.Water, ElementType.Fire, core) / ticks;
        for (int i = 0; i < ticks; i++)
        {
            var combat = core.GetCoreComponent<Combat>();
            if (combat != null)
            {
               combat.TakeDamage(new DamageInfo
               {
                   amount = damagePerTick,
                   element = ElementType.Fire,
                   sourceCore = source
                });
            }
            yield return new WaitForSeconds(tickInterval);
        }
    }
}