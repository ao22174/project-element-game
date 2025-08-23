using UnityEngine;

public class BossLeap : State
{
    private Boss? boss;
    private enum Phase { Telegraph, Air, Drop, Cooldown }
    private Phase phase;
    private float phaseTimer;

    private float telegraphDuration = 2f;
    private float airDuration = 5f;
    private float dropSpeed = 15f;
    private float cooldownDuration = 3f;
    private float shadowMoveSpeed = 2f;
    public BossLeap(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        boss = entity as Boss;
    }

    public override void Enter()
    {
        base.Enter();
        phase = Phase.Telegraph;
        phaseTimer = telegraphDuration;

    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        phaseTimer -= Time.deltaTime;
        switch (phase)
        {
            case Phase.Telegraph:
                if (phaseTimer <= 0f)
                {
                    // Hide boss sprite, "jump off screen"
                    boss.sprite.enabled = false;
                    phase = Phase.Air;
                    phaseTimer = airDuration;
                }
                break;

            case Phase.Air:
                Vector2 direction = (boss.player.transform.position - boss.transform.position).normalized;
                boss.core.GetCoreComponent<Movement>().SetVelocity(1.5f, direction);
                if (phaseTimer <= 0f)
                {
                    // Start drop
                    phase = Phase.Drop;
                    phaseTimer = 2f;

                }
                break;

            case Phase.Drop:
                // Boss falls onto shadow
                boss.sprite.enabled = true;
                boss.jumpHitbox.enabled = true;
                boss.core.GetCoreComponent<Movement>().SetVelocityZero();
                if (phaseTimer <= 0f)
                {
                    phase = Phase.Cooldown;
                    phaseTimer = cooldownDuration;
                }
                break;

            case Phase.Cooldown:
                boss.jumpHitbox.enabled = false;
                if (phaseTimer <= 0f)
                {
                    stateMachine.ChangeState(boss.idleState);
                }
                break;
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}