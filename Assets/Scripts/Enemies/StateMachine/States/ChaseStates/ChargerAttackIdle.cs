using System;
using UnityEngine;

public class ChargerAttackIdle : EntityAttackIdleState
{

    ChargerEntity charger;
    public ChargerAttackIdle(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        charger = entity as ChargerEntity;
      
        if (charger == null)
            throw new InvalidCastException("ChaserAttack requires a ShooterEntity.");
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}