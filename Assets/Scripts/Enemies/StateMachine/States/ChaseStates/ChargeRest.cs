using UnityEngine;
public class ChargerRest : State
{
    private ChargerEntity? charger;
    private float restTimeStart;
    public ChargerRest(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {
        charger = entity as ChargerEntity;
    }

    public override void Enter()
    {
        base.Enter();
        restTimeStart = Time.time;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > restTimeStart + charger.chargerData.chargeRestTime)
        {
            charger.stateMachine.ChangeState(charger.idleState);
        }
    }
}