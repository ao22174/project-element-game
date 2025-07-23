
using UnityEngine;
using ElementProject.gameEnums;
using System.Collections.Generic;
using UnityEditor;
public class ElementalStatus : MonoBehaviour
{
    private class ElementalBuildup
    {
        public float Amount;
        public float DecayRate;
    }

    private Dictionary<ElementType, ElementalBuildup> buildups = new();
    private ElementType currentAura = ElementType.None;
    [SerializeField] GameObject waterIndicator;
    [SerializeField] private float buildupThreshold = 100f;

    private void Start()
    {
        if (waterIndicator != null) waterIndicator.SetActive(false);   
    }
    private void Update()
    {
        // Decay all buildups over time
        foreach (var kvp in buildups)
        {
            kvp.Value.Amount -= kvp.Value.DecayRate * Time.deltaTime;
            kvp.Value.Amount = Mathf.Max(0, kvp.Value.Amount);
        }
        if (currentAura != null && waterIndicator != null)
        {
            Debug.Log(currentAura);
            waterIndicator.SetActive(true);
        }
    }
    public void ApplyElementalBuildup(ElementType type, float amount)
    {
        if (!buildups.ContainsKey(type))
            buildups[type] = new ElementalBuildup { Amount = 0, DecayRate = 10f };

        buildups[type].Amount += amount;

        if (buildups[type].Amount >= buildupThreshold)
        {
            ApplyAura(type);
            buildups[type].Amount = 0;
        }
    }

    private void ApplyAura(ElementType type)
    {
        if (currentAura != ElementType.None)
        {
            ElementalReactionSystem.TryTriggerReaction(gameObject, null, currentAura, type);
            currentAura = ElementType.None;
        }

        else
        {
            currentAura = type;
            Debug.Log($"Aura applied: {type}");
        }
    }

    public ElementType? GetCurrentAura() => currentAura;
}