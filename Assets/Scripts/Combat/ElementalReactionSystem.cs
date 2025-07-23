using System;
using UnityEngine;
using ElementProject.gameEnums;
public class ElementalReactionSystem
{
    public static void TryTriggerReaction(GameObject reactor, GameObject? source, ElementType baseElement, ElementType triggerElement)
    {
        // Lookup reaction table or logic
        Reaction reaction = ElementalReactionLookup.GetReaction(baseElement, triggerElement);
        reaction?.Apply(reactor, source);
    }
}   



