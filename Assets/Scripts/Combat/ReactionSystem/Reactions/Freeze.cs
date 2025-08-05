using UnityEngine;
using System;

//WATER + ICE
public class FreezeReaction : Reaction
{
    public override string ReactionName => "Freeze";
    public override float baseDamage => 0f;

    private float duration;

    public FreezeReaction(float duration)
    {
        this.duration = duration;
    }

    public override void Apply(Core core, Core sourceCore)
    {
        Debug.Log($"Applying {ReactionName} reaction to {core.name} for {duration} seconds.");
        IFreezable freezable = core.gameObject.GetComponentInChildren<IFreezable>();
        if (freezable != null)
        {
            Debug.Log("attempting to freeze");
            freezable.ApplyFreeze(duration);
        }
        else
        {
            Debug.LogWarning($"{core.name} does not implement IFreezable interface.");
        }
    }
}