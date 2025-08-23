using System;
using UnityEngine;

public class ShooterIdle : IdleState
{
    private ShooterEntity? chaser;
    public ShooterIdle(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {
        chaser = entity as ShooterEntity;
        
    }

    public override void Enter()
    {
        base.Enter();
        
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("changing state");
    }
    public override void LogicUpdate()
    {

        base.LogicUpdate();
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);
        if(playerDist > entity.EntityData.attackRange || !entity.PlayerInSight())
         stateMachine.ChangeState(chaser.chaseState);
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}