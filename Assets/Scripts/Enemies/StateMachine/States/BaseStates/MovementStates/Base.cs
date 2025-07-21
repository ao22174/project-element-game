using System.Collections;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using UnityEngine;



public class StationaryState : State
{
    public StationaryState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Stationary Mode");
        entity.rb.linearVelocity = Vector2.zero;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // if (!entity.PlayerInSight())
        // {
        //     stateMachine.ChangeState(entity.wanderState);
        // }
        
    }

    
   
}