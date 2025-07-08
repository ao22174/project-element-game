using System.Collections;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using UnityEngine;
using Pathfinding;
using UnityEditor.Callbacks;

/*
Plan of Cleanup


*/
public class WanderState : State
{
    private float wanderTimer;
    private Vector2 wanderDirection;
    private Vector2 spawnPosition;
    private float decisionCooldown = 2f; // Time between direction changes

    private float shootCooldownTimer;
    int currentWaypoint = 0;
    bool reachedEnd = false;
    public bool attacked;

    public float nextWaypointDistance = 0.15f;
    private bool canShoot => shootCooldownTimer >= entity.entityData.attackCooldown;

    public WanderState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        wanderTimer = 0f;
        spawnPosition = entity.transform.position;
        entity.spawnPosition = entity.transform.position;

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
        shootCooldownTimer += Time.deltaTime;
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);

        if (playerDist > entity.entityData.attackRange + 0.5f || !PlayerInSight())
        {
            if (entity.path == null)
            {
                Debug.Log("Path is null");
                return;
            }
            if (currentWaypoint >= entity.path.vectorPath.Count)
            {
                reachedEnd = true;
            }
            else
            {
                reachedEnd = false;
            }
            Vector2 direction = ((Vector2)entity.path.vectorPath[currentWaypoint] - entity.rb.position).normalized;
            Vector2 force = direction * entity.entityData.movementSpeed;

            entity.rb.linearVelocity = force;

            float waypointDistance = Vector2.Distance(entity.rb.position, entity.path.vectorPath[currentWaypoint]);
            if (waypointDistance < nextWaypointDistance)
            {
                Debug.Log("next waypoint!");
                currentWaypoint++;
            }
        }
        else if (playerDist <= entity.entityData.attackRange && PlayerInSight())
        {
            entity.rb.linearVelocity = Vector2.zero;

            if (PlayerInSight() && canShoot)
            {
                Shoot();
                shootCooldownTimer = 0f;
            }
        }
    }

    private bool PlayerInSight()
    {
        if (entity.player == null) return false;

        float dist = Vector2.Distance(entity.player.position, entity.transform.position);
        Debug.Log(dist);
        if (dist > entity.entityData.attackRange) return false;

        if (entity.entityData.usesLineOfSight)
        {
            RaycastHit2D hit = Physics2D.Raycast(entity.transform.position,
                                                entity.player.position - entity.transform.position,
                                                dist,
                                                entity.entityData.obstacleMask);
            return hit.collider == null;
        }
        Debug.Log("CanShoot");
        return true;
    }
    private void Shoot()
    {
        if (!entity.entityData.projectilePrefab) return;

        Vector2 direction = (entity.player.position - entity.transform.position).normalized;
        GameObject bullet = GameObject.Instantiate(entity.entityData.projectilePrefab, entity.transform.position, Quaternion.identity);
        ProjectileBullet bulletProj = bullet.GetComponent<ProjectileBullet>();
        bulletProj.Initialize(entity.transform.position, direction, entity.entityData.projectileSpeed, entity.entityData.damage, 1f, OwnedBy.Enemy);
    }
}