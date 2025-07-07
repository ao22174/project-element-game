using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class IdleState : State
{
    private float idleDuration;
    private float idleTimer;
    public IdleState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        idleDuration = Random.Range(1f, 3f); // Idle 1–3 seconds
        idleTimer = 0f;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        idleTimer += Time.deltaTime;

        // Detect player? Transition to alert/attack state here

        if (idleTimer >= idleDuration)
        {
            if (entity.entityData.behaviourType == BehaviourType.Wander)
            {
                stateMachine.ChangeState(new WanderState(entity, stateMachine, "isWalking"));
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}