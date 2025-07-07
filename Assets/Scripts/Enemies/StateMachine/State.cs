using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    protected string animBoolName;


    protected float startTime;

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
    }
    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {

    }
}