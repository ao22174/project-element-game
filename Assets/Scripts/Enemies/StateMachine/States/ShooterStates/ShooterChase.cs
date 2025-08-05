
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
public class ShooterChase : BaseChaseState
{
        private ShooterEntity? chaser;

    public ShooterChase(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
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
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);
        if (playerDist <= entity.EntityData.attackRange && entity.PlayerInSight())
        {
            stateMachine.ChangeState(chaser.chaserIdle);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    


}