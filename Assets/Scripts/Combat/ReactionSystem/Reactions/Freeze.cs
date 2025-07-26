using UnityEngine;
using System;

public class FreezeReaction : Reaction
{
    public override string ReactionName => "Freeze";

    private float duration;

    public FreezeReaction(float duration)
    {
        this.duration = duration;
    }

    public override void Apply(Core core, GameObject? source = null)
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