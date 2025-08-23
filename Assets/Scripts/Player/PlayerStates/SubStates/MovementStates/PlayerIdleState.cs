using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerIdleState : PlayerState
{
    protected Vector2 input;
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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
        input = player.InputHandler.MovementInput;
        if (input != Vector2.zero)
        {

            stateMachine.ChangeState(player.MoveState);
        }
        if(player.InputHandler.DashInput)
        {
            if(player.DashState.CheckIfCanDash()) stateMachine.ChangeState(player.DashState);
        }

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    } 
}

