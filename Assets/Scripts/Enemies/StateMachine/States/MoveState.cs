using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;
    public MoveState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, D_MoveState stateData) : base(entity, finiteStateMachine, animBoolName)
    {
        this.stateData = stateData;
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
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}