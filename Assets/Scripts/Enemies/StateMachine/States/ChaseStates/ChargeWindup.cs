using UnityEngine;
public class ChargerWindup : State
{
    private ChargerEntity? charger;
    private float chargeStartTime;
    public ChargerWindup(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {
        charger = entity as ChargerEntity;
    }

    public override void Enter()
    {
        base.Enter();
        chargeStartTime = Time.time;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);

        if (Time.time > chargeStartTime + charger.chargerData.chargeWindupTime)
        {
            charger.stateMachine.ChangeState(charger.chargeState);
        }
        else if (playerDist > charger.chargerData.attackRange)
        {
            charger.stateMachine.ChangeState(charger.idleState);
        }
    }
}