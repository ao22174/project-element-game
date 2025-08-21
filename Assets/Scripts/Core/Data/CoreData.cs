using UnityEngine;
using System;
using ElementProject.gameEnums;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.Profiling;

[System.Serializable]
public enum Faction { Player, Enemy, Neutral }
[System.Serializable]
public struct elementalResistances
{
    public ElementType element;
    [Range(0f, 1f)]
    public float res;

    public elementalResistances(ElementType element, float res)
    {
        this.element = element;
        this.res = res;
    }
}

[System.Serializable]
public struct elementalDamageBonuses
{
    public ElementType element;
    public float bonus;
    public elementalDamageBonuses(ElementType element, float bonus)
    {
        this.element = element;
        this.bonus = bonus;
    }
}
[CreateAssetMenu(fileName = "CoreData", menuName = "Data/CoreData")]

public class CoreData : ScriptableObject
{
    [Header("Basic Info")]
    public string entityName = "Enemy";
    [SerializeField] public RuntimeAnimatorController animator;
    public Faction faction = Faction.Neutral;

    [Header("Resistances")]

    [SerializeField]
    public List<elementalResistances> resistances = new List<elementalResistances>
    {
        new elementalResistances(ElementType.Fire, 0f),
        new elementalResistances(ElementType.Air, 0f),
        new elementalResistances(ElementType.Water, 0f),
        new elementalResistances(ElementType.Lightning, 0f),
        new elementalResistances(ElementType.Nature, 0f),
        new elementalResistances(ElementType.Earth, 0f),
        new elementalResistances(ElementType.Frost, 0f),
    };

    [SerializeField]
    public List<elementalDamageBonuses> bonuses = new List<elementalDamageBonuses>
    {
        new elementalDamageBonuses(ElementType.Fire, 0f),
        new elementalDamageBonuses(ElementType.Air, 0f),
        new elementalDamageBonuses(ElementType.Water, 0f),
        new elementalDamageBonuses(ElementType.Lightning, 0f),
        new elementalDamageBonuses(ElementType.Nature, 0f),
        new elementalDamageBonuses(ElementType.Earth, 0f),
        new elementalDamageBonuses(ElementType.Frost, 0f),
    };

    [Header("Base Stats")]
    public float baseMaxHealth = 100f;
    public float baseAttack = 1f;
    public float baseDefense = 1f;
    public float baseCritRate = 0f;
    public float baseCritMultiplier = 1f;
    public float baseAttackSpeed = 1f;


    [Header("Movement Settings")]
    public float movementSpeed = 2f;
    public bool canKnockback = true;
    public bool canFreeze = true;
}