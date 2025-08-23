using UnityEngine;
public interface IAttackBehavior
{
    void DoAttack();
}

public interface IAimBehavior
{
    void AimAtTarget(Transform target);
}

public interface IStartupBehaviour
{
    void OnStart();
}