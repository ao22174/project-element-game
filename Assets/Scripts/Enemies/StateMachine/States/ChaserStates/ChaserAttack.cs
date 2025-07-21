using System;
using UnityEngine;

public class ChaserAttack : EntityAttackState
{
    private float attackTime;
     private ChaserEntity? chaser;

    public ChaserAttack(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        chaser = entity as ChaserEntity;
        if (chaser == null)
            throw new InvalidCastException("ChaserAttack requires a ChaserEntity.");
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnAttack()
    {
        if (entity.entityData.projectilePrefab == null) return;
        Vector2 direction = (entity.player.position - entity.transform.position).normalized;
        GameObject bullet = GameObject.Instantiate(entity.entityData.projectilePrefab, entity.transform.position, Quaternion.identity);
        ProjectileBullet bulletProj = bullet.GetComponent<ProjectileBullet>();
        bulletProj.Initialize(entity.transform.position, direction, entity.entityData.projectileSpeed, entity.entityData.damage, 1f, OwnedBy.Enemy);
        attackTime = Time.time;
        if (chaser != null)
            stateMachine.ChangeState(chaser.chaserAttackIdle);
    }

    public bool CanFire()
    {
        return Time.time >= attackTime + entity.entityData.attackCooldown;
    }
}