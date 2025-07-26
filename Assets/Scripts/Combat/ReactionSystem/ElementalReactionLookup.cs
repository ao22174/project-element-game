using System.Collections.Generic;
using ElementProject.gameEnums;

public static class ElementalReactionLookup
{
    public static readonly Dictionary<(ElementType, ElementType), Reaction> reactions = new()
    {
        {(ElementType.Water, ElementType.Frost), new FreezeReaction(2f)}
        , {(ElementType.Water, ElementType.Fire), new ScaldReaction(50f,5f)}
    };


    public static Reaction? GetReaction(ElementType baseElement, ElementType triggerElement)
    {
        if (reactions.TryGetValue((baseElement, triggerElement), out var reaction)) return reaction;
        if (reactions.TryGetValue((triggerElement, baseElement), out var reverseReaction)) return reverseReaction;
        return null;

    }
}