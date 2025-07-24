using System;
using ElementProject.gameEnums;
using UnityEngine;
public abstract class Weapon
{
    public WeaponData data;
    public float damage;
    public int elementBuildup;
    public string Weaponname;
    protected WeaponType weaponType;
    public ElementType elementType;
    public float cooldown;
    public float attackTime;
    public GameObject weaponPrefab;
    public Core core;

    float GetEffectiveCooldown()
    {
        float atkSpeed = core.GetCoreComponent<Stats>()?.attackSpeed ?? 1f;
        float cooldown = data.cooldown / atkSpeed;
        return cooldown;
    }
    public Weapon(WeaponData data, Core core)
    {
        this.core = core;
        this.data = data;
        SetOwner(core);
        Weaponname = data.weaponName;
        damage = data.damage;
        weaponType = data.weaponType;
        elementType = data.elementType;
        cooldown = data.cooldown;
        weaponPrefab = data.weaponPrefab;
        elementBuildup = data.elementBuildup;
    }
    


    public virtual void Attack(Vector2 direction, Vector2 position) { }
    public abstract float CalculateDamage();
    public void SetOwner(Core core) => this.core = core;
    public bool CanAttack() => Time.time >= attackTime + GetEffectiveCooldown();

    
}
