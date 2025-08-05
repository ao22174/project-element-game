using UnityEngine;
public class ChargerIdle : IdleState
{

    private ChargerEntity? charger;
    public ChargerIdle(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {

        charger = entity as ChargerEntity;
    }

    public override void Enter()
    {
        base.Enter();
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
        Debug.Log("idle");
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);
        if (playerDist <= entity.EntityData.attackRange)
        {
            Debug.Log("windup");

            charger.stateMachine.ChangeState(charger.windupState);
        }
        else
        {
            charger.stateMachine.ChangeState(charger.chaseState);
        }
    }
}