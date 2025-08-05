using UnityEditor.MPE;
using UnityEngine;
public class ChargerCharge : State
{
    private ChargerEntity? charger;
    private Vector2 chargeDirection;
    private float chargeStartTime;
    public ChargerCharge(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName)
    {
        charger = entity as ChargerEntity;
    }

    public override void Enter()
    {
        base.Enter();
        charger.canRotate = false;
        chargeDirection = (entity.player.position - entity.transform.position).normalized;
        charger.core.GetCoreComponent<Movement>().SetVelocity(charger.chargerData.chargeSpeed, chargeDirection);
        chargeStartTime = Time.time;

    }
    public override void Exit()
    {
        base.Exit();
                charger.canRotate = true;

        charger.core.GetCoreComponent<Movement>().SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > chargeStartTime + charger.chargerData.chargeDuration)
        {
            charger.stateMachine.ChangeState(charger.restState);
        }        
    }
}