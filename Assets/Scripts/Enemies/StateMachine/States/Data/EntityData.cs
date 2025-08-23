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

public class EntityData : CoreData
{
    [Header("Entity Based Information")]
    [Header("Basic Info")]
    [SerializeField]public GameObject enemyPrefab;
    [SerializeField]public ElementType elementType = ElementType.None;
    [SerializeField]public BehaviourType behaviourType = BehaviourType.Wander;

    public float attackRange = 4f;
    public bool usesProjectile = true;
    public GameObject projectilePrefab;
    public float projectileSpeed = 6f;
    public bool aimAtPlayer = true;

    [Header("Movement Settings")]
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
    public bool hasPathfinding = true;

    [Header("Effects & Audio")]
    [SerializeField]public GameObject deathEffectPrefab;
    [SerializeField]public AudioClip attackSFX;
    [SerializeField]public AudioClip deathSFX;

    [SerializeField]public GameObject iceCube;

    [Header("Collision & Environment")]
    [SerializeField]public LayerMask obstacleMask;
    public bool stopAtWalls = true;
    public bool usesPathfinding = false;

    [Header("Advanced / Boss Settings")]
    public bool isBoss = false;
    public float phaseChangeHealthThreshold = 0.5f; // 50% HP
    public GameObject[] minionsToSummon;
}