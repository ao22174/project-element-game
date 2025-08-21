using UnityEngine;


public class PlayerMoveState : PlayerState
{
    protected Vector2 input;
    private Movement movement => player.core.GetCoreComponent<Movement>();
    private Stats stats => player.core.GetCoreComponent<Stats>();
    private float dustTimer;
[SerializeField] private float dustRate = 0.25f; // spawn every 0.1s
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
        movement.SetVelocity(stats.MovementSpeed, input);

        if (input == Vector2.zero)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        if (player.InputHandler.DashInput)
        {
            if (player.DashState.CheckIfCanDash()) stateMachine.ChangeState(player.DashState);
        }
        dustTimer += Time.deltaTime;
        if (dustTimer >= dustRate)
        {
            GameObject.Instantiate(player.dust, player.dustSpawn.position, Quaternion.identity);
            dustTimer = 0f;
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

