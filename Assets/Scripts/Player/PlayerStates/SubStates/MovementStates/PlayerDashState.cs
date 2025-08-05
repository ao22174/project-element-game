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
        player.weaponHandler.SheatheWeapon();
        base.Enter();
       dashDirection = player.LastInputDirection;

        if (dashDirection == Vector2.zero)
                dashDirection = Vector2.right; // or Vector2.right, or whatever makes sense
        dashStartTime = Time.time;
    }
    public override void Exit()
    {
         player.weaponHandler.UnsheatheWeapon();

        base.Exit();

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= dashStartTime + playerData.dashDuration)
        {
            movement.SetVelocity(0f, Vector2.zero);
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
        return Time.time >= dashStartTime + playerData.dashCooldown;
    }
}

