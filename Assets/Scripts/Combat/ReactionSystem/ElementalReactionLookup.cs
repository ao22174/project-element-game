    using System.Collections.Generic;
    using ElementProject.gameEnums;
using UnityEngine;

public static class ElementalReactionLookup
{
    private static GameObject explosionPrefab;
    private static LayerMask damageLayer;

    public static readonly Dictionary<(ElementType, ElementType), Reaction> reactions = new();
    public static readonly Dictionary<Reaction, (ElementType, ElementType)> reverseLookup = new();

    public static void Initialize(GameObject prefab, LayerMask layer)
    {
        explosionPrefab = prefab;
        damageLayer = layer;

        reactions.Clear();
        reverseLookup.Clear();

        AddReaction(ElementType.Water, ElementType.Frost, new FreezeReaction(2f));
        AddReaction(ElementType.Water, ElementType.Fire, new ScaldReaction(50f, 5f));
        AddReaction(ElementType.Fire, ElementType.Frost, new Melt());
        AddReaction(ElementType.Lightning, ElementType.Fire, new Overload(damageLayer, explosionPrefab));

        Debug.Log($"Elemental reactions initialized with explosionPrefab: {explosionPrefab != null}, damageLayer: {damageLayer}");
    }
    public static Reaction? GetReaction(ElementType baseElement, ElementType triggerElement)
    {
        if (reactions.TryGetValue((baseElement, triggerElement), out var reaction)) return reaction;
        if (reactions.TryGetValue((triggerElement, baseElement), out var reverseReaction)) return reverseReaction;
        return null;
    }

    private static void AddReaction(ElementType e1, ElementType e2, Reaction reaction)
    {
        reactions[(e1, e2)] = reaction;
        reverseLookup[reaction] = (e1, e2);
    }
    public static (ElementType, ElementType)? GetElementsFromReaction(Reaction reaction)
    {
        if (reverseLookup.TryGetValue(reaction, out var elementPair)) return elementPair;
        return null;
    }
}