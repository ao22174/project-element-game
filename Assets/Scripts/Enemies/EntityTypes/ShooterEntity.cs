using UnityEngine;
using System;

public class ShooterEntity : Entity
{
    BaseChaseState wanderState;
    IdleState idleState;

    StationaryState stationaryState;

    public EntityAttackState attackState { get; private set; }

    public EntityAttackIdleState attackIdleState { get; private set; }

    public EntityAttackFreezeState attackFreezeState { get; private set; }

    public override void Start()
    {
        base.Start();
        wanderState = new BaseChaseState(this, stateMachine, "isWandering");
        idleState = new IdleState(this, stateMachine, "isWandering");

        attackStateMachine = new FiniteStateMachine();
        attackIdleState = new EntityAttackIdleState(this, attackStateMachine, "attackIdling");
        attackState = new EntityAttackState(this, attackStateMachine, "attacking");
        stationaryState = new StationaryState(this, stateMachine, "Stationary");
        attackFreezeState = new EntityAttackFreezeState(this, stateMachine, "Frozen");

        stateMachine.Initialize(idleState);
        attackStateMachine.Initialize(attackIdleState);
    }

    protected override void InitializeStates()
    {
        throw new NotImplementedException();
    }
}