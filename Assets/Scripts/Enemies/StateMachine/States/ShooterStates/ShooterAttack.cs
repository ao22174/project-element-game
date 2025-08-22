using System;
using UnityEngine;

public class ShooterAttack : EntityAttackState
{
    private float attackTime;
     private ShooterEntity? chaser;

    public ShooterAttack(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        chaser = entity as ShooterEntity;
        if (chaser == null)
            throw new InvalidCastException("ShooterAttackState requires a ShooterEntity.");
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
        foreach (IAttackBehavior attack in chaser.attackBehaviors)
        {
            attack.DoAttack();
        }
        attackTime = Time.time;
        if (chaser != null)
            stateMachine.ChangeState(chaser.chaserAttackIdle);
    }
}