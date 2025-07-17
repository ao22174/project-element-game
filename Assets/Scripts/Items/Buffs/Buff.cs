using UnityEngine;

public abstract class Buff
{
    public BuffData buffData;
    public int stackCount = 1;
    public abstract string BuffName { get; }
    public virtual void OnApply(GameObject target) { }
    public virtual void OnRemove(GameObject target) { }
    public virtual void OnUpdate(GameObject target) { }
    public virtual void OnAttack(GameObject target, ref float damage) { }
    public virtual void OnHitEnemy(GameObject target, GameObject enemy) { }

    public virtual void OnDash() { }

    public virtual void OnAttacked(GameObject target, GameObject enemy = null, int damageTaken = 0) {}
}