using Unity.VisualScripting;
using UnityEngine;
using System;

public class EntityAttackIdleState : State
{
    public EntityAttackIdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }



    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (entity.PlayerInSight() && entity.attackState.CanFire())
        {
            stateMachine.ChangeState(entity.attackState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}