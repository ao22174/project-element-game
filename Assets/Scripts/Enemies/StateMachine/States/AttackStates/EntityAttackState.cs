using Unity.VisualScripting;
using UnityEngine;

public class EntityAttackState : State
{
    private float attackTime;
    public EntityAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
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
        if (entity.entityData.projectilePrefab == null) return;

        Vector2 direction = (entity.player.position - entity.transform.position).normalized;
        GameObject bullet = GameObject.Instantiate(entity.entityData.projectilePrefab, entity.transform.position, Quaternion.identity);
        ProjectileBullet bulletProj = bullet.GetComponent<ProjectileBullet>();
        bulletProj.Initialize(entity.transform.position, direction, entity.entityData.projectileSpeed, entity.entityData.damage, 1f, OwnedBy.Enemy);
        attackTime = Time.time;
        stateMachine.ChangeState(entity.attackIdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanFire()
    {
        return Time.time >= attackTime + entity.entityData.attackCooldown;
    }

}