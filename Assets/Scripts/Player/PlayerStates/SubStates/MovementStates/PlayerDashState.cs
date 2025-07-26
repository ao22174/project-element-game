using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class PlayerDashState : PlayerState
{
    private float dashStartTime;
    private Vector2 dashDirection;
    private float dashEndTime;
    private Movement movement => player.core.GetCoreComponent<Movement>();
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        dashDirection = player.LastInputDirection;  
        dashStartTime = Time.time;
        movement.SetVelocity(playerData.dashSpeed, dashDirection);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= dashStartTime + playerData.dashDuration)
        {
            dashEndTime = Time.time;
            stateMachine.ChangeState(player.MoveState);
        }
    }
    public override void PhysicsUpdate()
    {
        movement.SetVelocity(playerData.dashSpeed, dashDirection);
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public bool CheckIfCanDash()
    {
        return Time.time >= dashEndTime + playerData.dashCooldown;
    }
}

