using System;
using UnityEngine;

public class ChaserIdle : IdleState
{
    private ChaserEntity? chaser;
    public ChaserIdle(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {
        chaser = entity as ChaserEntity;
        
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
         stateMachine.ChangeState(chaser.chaseState);
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}