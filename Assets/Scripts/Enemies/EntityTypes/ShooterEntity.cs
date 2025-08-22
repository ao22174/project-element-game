using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
public class ShooterEntity : Entity
{
    // EXPECTED BEHAVIOUR --- Idle -> (On Sight) -> Idle -> (Out LOS) -> Chase
    public ShooterChase chaseState;
    public ShooterIdle chaserIdle;
    public ShooterAttack chaserAttack;
    public ShooterAttackIdle chaserAttackIdle;
    public SpriteRenderer spriteRenderer;

    public ShooterData? shooterData;
    public List<IAimBehavior> aimBehaviors = new List<IAimBehavior>();

    public override void Awake()
    {
        base.Awake();
        aimBehaviors = new List<IAimBehavior>(GetComponentsInChildren<IAimBehavior>());
    }
    public override void Start()
    {
        base.Start();
        chaseState = new ShooterChase(this, stateMachine, "Chasing");
        chaserIdle = new ShooterIdle(this, stateMachine, "Idle");

        chaserAttack = new ShooterAttack(this, attackStateMachine, "Attacking");
        chaserAttackIdle = new ShooterAttackIdle(this, attackStateMachine, "AttackIdle");

        stateMachine.Initialize(chaserIdle);
        attackStateMachine.Initialize(chaserAttackIdle);

        shooterData = EntityData as ShooterData;

        if (shooterData == null)
        {
            Debug.LogError($"Expected ShooterData, got {EntityData.GetType()} on {gameObject.name}");
            return;
        }

        core = GetComponentInChildren<Core>();
    }

    public override void Update()
    {
        base.Update();
        foreach (IAimBehavior aimBehavior in aimBehaviors)
        {
            aimBehavior.AimAtTarget(player);
        }
    }

    protected override void InitializeStates()
    {

    }




}