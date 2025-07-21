
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
public class BaseChaseState : State
{

    int currentWaypoint = 0;

    public float nextWaypointDistance = 0.15f;

    public BaseChaseState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            entity.path = p;
            currentWaypoint = 0;
        }
    }
    public override void Exit()
    {
        base.Exit();
        entity.rb.linearVelocity = Vector2.zero;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        ChasePlayer();
    }

    public void ChasePlayer()
    {
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);
        Debug.Log("trying to chase");
        if (playerDist > entity.entityData.attackRange + 0.5f || !entity.PlayerInSight())
        {
            Debug.Log("get here");
            if (entity.path == null)
            {
                Debug.Log("path is null");
                return;
                }
            ;
            if (currentWaypoint >= entity.path.vectorPath.Count) return;
            Debug.Log("moving");
            Vector2 direction = ((Vector2)entity.path.vectorPath[currentWaypoint] - entity.rb.position).normalized;
            Vector2 force = direction * entity.entityData.movementSpeed;

            entity.rb.linearVelocity = force;

            float waypointDistance = Vector2.Distance(entity.rb.position, entity.path.vectorPath[currentWaypoint]);
            if (waypointDistance < nextWaypointDistance) currentWaypoint++;

        }
        else if (playerDist <= entity.entityData.attackRange && entity.PlayerInSight())
        {
            entity.rb.linearVelocity = Vector2.zero;
        }
    }

    
   
}