using UnityEngine;
using System;

public class ChaserEntity : Entity
{
    // EXPECTED BEHAVIOUR --- Idle -> (On Sight) -> Idle -> (Out LOS) -> Chase
    public ChaserChase chaseState;
    public ChaserIdle chaserIdle;

    public ChaserAttack chaserAttack;
    public ChaserAttackIdle chaserAttackIdle;


    public override void Start()
    {
        base.Start();
        chaseState = new ChaserChase(this, stateMachine, "Chasing");
        chaserIdle = new ChaserIdle(this, stateMachine, "Idle");

        chaserAttack = new ChaserAttack(this, attackStateMachine, "Attacking");
        chaserAttackIdle = new ChaserAttackIdle(this, attackStateMachine, "AttackIdle");

        stateMachine.Initialize(chaserIdle);
        attackStateMachine.Initialize(chaserAttackIdle);
        
    }
    protected override void InitializeStates()
    {
        
    }
}