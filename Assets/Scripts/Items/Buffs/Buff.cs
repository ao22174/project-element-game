using ElementProject.gameEnums;
using UnityEngine;

public abstract class Buff
{
    public BuffData buffData = null!;
    public IWeaponUser user;
    public int stackCount = 1;
    public abstract string BuffName { get; }
    public virtual void OnApply(GameObject target) { }
    public virtual void OnRemove(GameObject target) { }
    public virtual void OnUpdate(GameObject target) { }
    public virtual void OnAttack(GameObject target, float damage, Vector2 direction = default ){ }
    public virtual void OnHitEnemy(GameObject target, GameObject enemy) { }

    public virtual void OnDash() { }

    public virtual void OnApplyElement(GameObject target, ElementType element)
    {
        
    }

    public virtual void OnKill(GameObject target, Vector2 position) {    }

    public virtual void OnAttacked(GameObject target, GameObject? enemy = null, int damageTaken = 0) { }
}