
using UnityEngine;
using Pathfinding;


/*
Plan of Cleanup
1. Organisation of State Machines
- WanderState rename to RangeMoveState, will constantly move to the player until they are both in LOS and in range, then switches to the stationary state 
- New state StationaryState, will just stand there if the player is close enough, when player out of LOS or too far, enter RangeMoveState
- New State of AttackedState, will get knocked back

2. Detachment of attacking
- Attacking will be its own statemachine
- Attacking independent of Movement
- Linked by distance/range/behaviour


*/
public class ChaserChase : BaseChaseState
{
    public ChaserChase(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName) { }

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