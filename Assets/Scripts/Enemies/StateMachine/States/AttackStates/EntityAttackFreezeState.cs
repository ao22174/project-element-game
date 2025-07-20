using Unity.VisualScripting;
using UnityEngine;

public class EntityAttackFreezeState : State
{
    float frozenTil;
    bool isFrozen => Time.time < frozenTil;
    public EntityAttackFreezeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
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

        if (isFrozen) return;
        stateMachine.ChangeState(entity.attackIdleState);
        

    }
    public void setDuration(float duration)
    {
        frozenTil = duration + Time.time;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}