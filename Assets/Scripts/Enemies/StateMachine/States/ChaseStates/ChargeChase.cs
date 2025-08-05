using UnityEngine;
public class ChargerChase : BaseChaseState
{
    private ChargerEntity? charger;
    public ChargerChase(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float playerDist = Vector2.Distance(entity.player.position, entity.transform.position);
        if (playerDist <= entity.EntityData.attackRange && entity.PlayerInSight())
        {
            stateMachine.ChangeState(charger.idleState);
        }
    }
}