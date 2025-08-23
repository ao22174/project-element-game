using NUnit.Framework.Constraints;
using UnityEngine;

public class BossIdle : State
{
    private float entryTime;
    private Boss? boss;
    public BossIdle(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        boss = entity as Boss;
    }
    public override void Enter()
    {
        base.Enter();
        entryTime = Time.time;
        boss.sprite.color = Color.red;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= entryTime + Random.Range(3.0f, 5.0f))
        {
            ChooseNextAttack();
        }
    }

    public void ChooseNextAttack()
    {
        float roll = Random.value;
        if (roll < 0.5f) stateMachine.ChangeState(boss.bossSpitState);
        else stateMachine.ChangeState(boss.bossLeap);
    }
}