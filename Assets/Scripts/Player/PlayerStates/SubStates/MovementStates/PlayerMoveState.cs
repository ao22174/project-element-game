using UnityEngine;


public class PlayerMoveState : PlayerState
{
    protected Vector2 input;
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        player.LastInputDirection = input;

        player.SetVelocity(input * playerData.movementSpeed);
        // player.CheckIfShouldFlip(input);

        if(input == Vector2.zero)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        if(player.InputHandler.DashInput)
        {
            if(player.DashState.CheckIfCanDash())   stateMachine.ChangeState(player.DashState);
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

