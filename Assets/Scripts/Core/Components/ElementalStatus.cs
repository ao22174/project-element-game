
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
    [SerializeField] private float buildupThreshold = 100f;

    private void Start()
    {
        currentAura = ElementType.None;
        UIManager?.ShowElementalIndicator(false);
        buildups.Clear(); 
    }
   
    
    public void ApplyElementalBuildup(ElementType type, float amount, Core sourceCore)
    {
        if (!buildups.ContainsKey(type) && type != ElementType.None)
        
            buildups[type] = new ElementalBuildup { Amount = 0, DecayRate = 10f };
            
        if (buildups.ContainsKey(type))
        {
            buildups[type].Amount += amount;

            if (buildups[type].Amount >= buildupThreshold)
            {
                ApplyAura(type, sourceCore);
                buildups[type].Amount = 0;
            }
        }
        
    }

    private void ApplyAura(ElementType type, Core source)
    {
        if (currentAura != ElementType.None && currentAura != type)
        {
            Debug.Log($"Triggering reaction: {currentAura} + {type}");
            UIManager?.ShowDamageNumber(ElementalReactionSystem.GetReactionName(currentAura, type), type);

            ElementalReactionSystem.TryTriggerReaction(core, source, currentAura, type);
            UIManager?.ShowElementalIndicator(false);

            currentAura = ElementType.None;
        }


        else
        {
            currentAura = type;
            Debug.Log($"Aura applied: {type}");
            UIManager?.SetElementIcon(type);
            UIManager?.ShowElementalIndicator(true);
            
        }
    }

    public ElementType? GetCurrentAura() => currentAura;
}