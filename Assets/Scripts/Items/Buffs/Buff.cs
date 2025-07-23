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

    public virtual void OnAuraActivate(GameObject target, GameObject enemy, ElementType element){}
    public virtual void OnReaction(GameObject target, GameObject enemy, ElementType baseElement, ElementType triggerElement){}

    public virtual void OnDash(GameObject target) { }
    public virtual void OnRoomEnter(GameObject target) { }
    public virtual void OnRoomClear(GameObject target) { }

    public virtual void OnApplyElement(GameObject target, ElementType element, float damage)
    {

    }

    public virtual void OnKill(GameObject target, Vector2 position) {    }

    public virtual void OnAttacked(GameObject target, GameObject? enemy = null, int damageTaken = 0) { }
}