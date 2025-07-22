using System;
using UnityEngine;

public class ShooterAttackIdle : EntityAttackIdleState
{

    ShooterEntity? chaser;
    public ShooterAttackIdle(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        chaser = entity as ShooterEntity;
        if (chaser == null)
            throw new InvalidCastException("ChaserAttack requires a ShooterEntity.");
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("Wow");
        base.Exit();
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateMachine.ChangeState(chaser.chaserAttack);
    }
}