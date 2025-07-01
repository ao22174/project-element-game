using UnityEngine;

    public class PlayerAttackIdleState : PlayerState
    {
        public PlayerAttackIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
    public override void Enter()
    {
        Debug.Log("enterintg IdleStateWeapon");
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.InputHandler.FireInput && player.AttackState.CanFire())
        {
            Debug.Log("Entering attack State");
            stateMachine.ChangeState(player.AttackState);
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
