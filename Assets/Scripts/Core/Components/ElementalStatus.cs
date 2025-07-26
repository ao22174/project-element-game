
using UnityEngine;
using ElementProject.gameEnums;
using System.Collections.Generic;
using UnityEditor;
public class ElementalStatus : CoreComponent
{
    private UIManager? UIManager { get => uiManager ?? core.GetCoreComponent(ref uiManager); }
    private UIManager uiManager;
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
        currentAura = ElementType.None;
        buildups.Clear(); 
    }
    private void Update()
    {
        if (currentAura != ElementType.None && waterIndicator != null)
        {
            waterIndicator.SetActive(true);
        }
        else
        {
            waterIndicator?.SetActive(false);
        }
    }
    public void ApplyElementalBuildup(ElementType type, float amount)
    {
        if (!buildups.ContainsKey(type) && type != ElementType.None)
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
        if (currentAura != ElementType.None && currentAura != type)
        {
            Debug.Log($"Triggering reaction: {currentAura} + {type}");
            UIManager?.ShowDamageNumber(ElementalReactionSystem.GetReactionName(currentAura,type), type);

            ElementalReactionSystem.TryTriggerReaction(core, null, currentAura, type);
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