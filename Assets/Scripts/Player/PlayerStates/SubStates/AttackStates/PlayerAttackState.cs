using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        player.weaponHandler.Attack();
        stateMachine.ChangeState(player.AttackIdleState);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    }

