using UnityEngine;
using System.Collections;
using ElementProject.gameEnums;

//WATER + ICE
public class ScaldReaction : Reaction
{
    public override string ReactionName => "Scald";
    private float totalDamage;
    private float duration;
    private int ticks = 5; // Number of ticks

    public ScaldReaction(float totalDamage, float duration)
    {
        this.totalDamage = totalDamage;
        this.duration = duration;
    }

    public override void Apply(Core core, GameObject? source = null)
    {
        core.StartCoroutine(ApplyScaldDoT(core, totalDamage, duration, ticks));
    }

    private IEnumerator ApplyScaldDoT(Core core, float totalDamage, float duration, int ticks)
    {
        float tickInterval = duration / ticks;
        float damagePerTick = totalDamage / ticks;
        for (int i = 0; i < ticks; i++)
        {
            var combat = core.GetCoreComponent<Combat>();
            if (combat != null)
            {
               combat.TakeDamage(new DamageInfo
               {
                   amount = damagePerTick,
                   element = ElementType.Fire,
                   core = core
                });
            }
            yield return new WaitForSeconds(tickInterval);
        }
    }
}