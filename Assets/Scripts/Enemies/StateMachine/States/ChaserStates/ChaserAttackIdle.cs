using System;
using UnityEngine;

public class ChaserAttackIdle : EntityAttackIdleState
{

    ChaserEntity? chaser;
    public ChaserAttackIdle(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        chaser = entity as ChaserEntity;
        if (chaser == null)
            throw new InvalidCastException("ChaserAttack requires a ChaserEntity.");
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
        if (entity.PlayerInSight() && chaser.chaserAttack.CanFire())
        {
            stateMachine.ChangeState(chaser.chaserAttack);
        }
    }
}