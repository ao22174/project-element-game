using UnityEngine;
using System;
using ElementProject.gameEnums;

public abstract class Reaction
{
    public abstract string ReactionName { get; }
    public abstract void Apply(GameObject target, GameObject? source = null);
}