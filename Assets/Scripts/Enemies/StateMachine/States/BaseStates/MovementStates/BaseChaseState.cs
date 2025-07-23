
using UnityEngine;
using Pathfinding;
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
        if (playerDist > entity.EntityData.attackRange + 0.5f || !entity.PlayerInSight())
        {
            if (entity.path == null)
            {
                return; 
                }
            ;
            if (currentWaypoint >= entity.path.vectorPath.Count) return;
            Vector2 direction = ((Vector2)entity.path.vectorPath[currentWaypoint] - entity.rb.position).normalized;
            Vector2 force = direction * entity.EntityData.movementSpeed;

            entity.rb.linearVelocity = force;

            float waypointDistance = Vector2.Distance(entity.rb.position, entity.path.vectorPath[currentWaypoint]);
            if (waypointDistance < nextWaypointDistance) currentWaypoint++;

        }
        else if (playerDist <= entity.EntityData.attackRange && entity.PlayerInSight())
        {
            entity.rb.linearVelocity = Vector2.zero;
        }
    }

    
   
}