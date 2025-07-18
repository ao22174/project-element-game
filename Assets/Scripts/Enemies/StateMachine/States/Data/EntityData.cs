using System;
using UnityEngine;
using ElementProject.gameEnums;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]

public enum BehaviourType
{
    Charge,
    Orbit,
    Wander,
    Stationary,
}

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Data/Enemy Data/Base Data")]
public class EntityData : ScriptableObject
{
    [Header("Basic Info")]
    public string entityName = "Enemy";
    public GameObject enemyPrefab;
    public ElementType elementType;
    public BehaviourType behaviourType;
    public RuntimeAnimatorController animator;
    [Header("Conditions")]
    public float freezeResistance = 0.8f;
    public float stunResistance = 0.6f;    
    public float poisonResistance = 0.4f;
    

    [Header("Health & Combat")]
    public float healthPoints = 10f;
    public float damage = 1f;
    public float attackCooldown = 1.5f;
    public float attackRange = 4f;
    public bool usesProjectile = true;
    public GameObject projectilePrefab;
    public float projectileSpeed = 6f;
    public bool aimAtPlayer = true;

    [Header("Movement Settings")]
    public float movementSpeed = 2f;
    public float wanderRadius = 5f;
    public float orbitDistance = 3f;
    public float chargeSpeed = 7f;
    public float chargeCooldown = 3f;
    public float chaseRange = 8f;

    [Header("AI Behavior")]
    [Range(0, 1)] public float aggression = 0.5f; // 0 = passive, 1 = aggressive
    public float reactionTime = 0.2f; // delay before engaging
    public float awarenessRadius = 6f;
    public bool canDodge = false;
    public bool usesLineOfSight = true;

    [Header("Effects & Audio")]
    public GameObject deathEffectPrefab;
    public AudioClip attackSFX;
    public AudioClip deathSFX;

    public GameObject iceCube;

    [Header("Collision & Environment")]
    public LayerMask obstacleMask;
    public bool stopAtWalls = true;
    public bool usesPathfinding = false;

    [Header("Advanced / Boss Settings")]
    public bool isBoss = false;
    public float phaseChangeHealthThreshold = 0.5f; // 50% HP
    public GameObject[] minionsToSummon;
}