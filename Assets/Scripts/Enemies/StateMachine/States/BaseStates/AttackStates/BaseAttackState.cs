using Unity.VisualScripting;
using UnityEngine;

public class EntityAttackState : State
{

    public EntityAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
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
        
        OnAttack();
        
    }

    public virtual void OnAttack(){}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    

}