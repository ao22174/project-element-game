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

    public override void Apply(GameObject target, GameObject? source = null)
    {
        target.GetComponent<IFreezable>().ApplyFreeze(2f);
    }
}