using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WanderState : State
{
    private float wanderTimer;
    private Vector2 wanderDirection;
    private Vector2 spawnPosition;
    private float decisionCooldown = 2f; // Time between direction changes

    private float shootCooldownTimer;
    private bool canShoot => shootCooldownTimer >= entity.entityData.attackCooldown;

    public WanderState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        wanderTimer = 0f;
        spawnPosition = entity.transform.position;
        entity.spawnPosition = entity.transform.position;
        PickNewDirection();
    }
    public override void Exit()
    {
        base.Exit();
        entity.rb.linearVelocity = Vector2.zero;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        wanderTimer += Time.deltaTime;
        shootCooldownTimer += Time.deltaTime;

        if (wanderTimer >= decisionCooldown)
        {
            PickNewDirection();
            wanderTimer = 0f;
        }

        if (Vector2.Distance(entity.transform.position, spawnPosition) > entity.entityData.wanderRadius)
        {
            // Walk back toward center
            wanderDirection = (spawnPosition - (Vector2)entity.transform.position).normalized;
            entity.wanderDirection = wanderDirection; // <— for gizmo use
        }

        entity.rb.linearVelocity = wanderDirection * entity.entityData.movementSpeed;

        if (PlayerInSight() && canShoot)
        {
            Shoot();
            shootCooldownTimer = 0f;
        }
    }

    private bool PlayerInSight()
    {
        if (entity.player == null) return false;

        float dist = Vector2.Distance(entity.player.position, entity.transform.position);
        if (dist > entity.entityData.attackRange) return false;

        if (entity.entityData.usesLineOfSight)
        {
            RaycastHit2D hit = Physics2D.Raycast(entity.transform.position,
                                                entity.player.position - entity.transform.position,
                                                dist,
                                                entity.entityData.obstacleMask);
            return hit.collider == null;
        }

        return true;
    }
    private void PickNewDirection()
    {
        Vector2 toPlayer = entity.player.position - entity.transform.position;

        for (int i = 0; i < 5; i++)
        {
            Vector2 candidate = Random.insideUnitCircle.normalized * 0.5f;

            // Slightly bias away from direct path to player
            if (entity.player != null)
            {
                candidate -= toPlayer.normalized * 0.3f;
                candidate.Normalize();
            }

            RaycastHit2D hit = Physics2D.Raycast(entity.transform.position, candidate, 1f, entity.entityData.obstacleMask);
            if (!hit)
            {
                wanderDirection = candidate;
                return;
            }
        }

        wanderDirection = Vector2.zero;
    }
    private void Shoot()
    {
        if (!entity.entityData.projectilePrefab) return;

        Vector2 direction = (entity.player.position - entity.transform.position).normalized;
        GameObject bullet = GameObject.Instantiate(entity.entityData.projectilePrefab, entity.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * entity.entityData.projectileSpeed;
    }
}