using UnityEngine;
using System.Collections;
using ElementProject.gameEnums;

//Water + FIRE
public class ScaldReaction : Reaction
{
    public override string ReactionName => "Scald";
    public override float baseDamage => totalDamage;
    float totalDamage;
    private float duration;
    private int ticks = 5; // Number of ticks

    public ScaldReaction(float totalDamage, float duration)
    {
        this.totalDamage = totalDamage;
        this.duration = duration;
    }

    public override void Apply(Core core, Core source)
    {
        core.StartCoroutine(ApplyScaldDoT(core, duration, ticks, source));
    }

    private IEnumerator ApplyScaldDoT(Core core, float duration, int ticks, Core source)
    {
        float tickInterval = duration / ticks;
        float damagePerTick = DamageCalculator.CalculateReactionDamage(source, this, core) / ticks;
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