using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeState : State
{
    float frozenTil;
    bool isFrozen => Time.time < frozenTil;
    GameObject iceCube;
    public FreezeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        iceCube = UnityEngine.Object.Instantiate(entity.entityData.iceCube, entity.transform.position, quaternion.identity, entity.transform);
    }

    public override void Exit()
    {
        base.Exit();
        UnityEngine.Object.Destroy(iceCube);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isFrozen) return;
        stateMachine.ChangeState(entity.idleState);
        

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