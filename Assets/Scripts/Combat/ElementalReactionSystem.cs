using System;
using UnityEngine;
using ElementProject.gameEnums;
public class ElementalReactionSystem
{
    public static void TryTriggerReaction(Core coreReactor, Core sourceCore, ElementType baseElement, ElementType triggerElement)
    {
        // Lookup reaction table or logic
        Reaction reaction = ElementalReactionLookup.GetReaction(baseElement, triggerElement);
        reaction?.Apply(coreReactor, sourceCore);
    }

    public static string GetReactionName(ElementType baseElement, ElementType triggerElement)
    {
        Reaction reaction = ElementalReactionLookup.GetReaction(baseElement, triggerElement);
        return reaction?.ReactionName ?? "No Reaction";
    }
}   



