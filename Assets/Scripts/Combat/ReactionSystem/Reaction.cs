using UnityEngine;
using System;
using ElementProject.gameEnums;

public abstract class Reaction
{
    public abstract string ReactionName { get; }
    public abstract float baseDamage { get;  }
    public abstract void Apply(Core core, Core source);
}