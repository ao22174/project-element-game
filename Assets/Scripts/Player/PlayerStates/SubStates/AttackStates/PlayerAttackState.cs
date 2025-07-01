using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float fireEndTime;
    private Vector2 fireDirection;

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
        //This is during the attack, so acting as a trigger for the gun, returing back to an unfired weapon when the weapon is used.
        //Weaponbehaviour is kept within the weapons
        base.LogicUpdate();
        fireDirection = player.GetFireOrigin().transform.right;
        player.currentWeapon.Attack(fireDirection);
        fireEndTime = Time.time;
    
        stateMachine.ChangeState(player.AttackIdleState);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public bool CanFire()
    {
        return Time.time >= fireEndTime + player.currentWeapon?.cooldown;
    }
    }

