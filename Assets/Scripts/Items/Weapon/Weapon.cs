using System;
using System.Runtime.InteropServices;
using ElementProject.gameEnums;
using UnityEngine;
public abstract class Weapon
{
    public WeaponData data;
    protected IWeaponUser? owner;
    public float damage;
    public int elementBuildup;
    public string Weaponname;
    protected WeaponType weaponType;
    public ElementType elementType;
    public float cooldown;
    public float attackTime;
    public GameObject weaponPrefab;

    float GetEffectiveCooldown()
    {
        if (owner is Player player)
            return cooldown / (1 + player.stats.attackSpeedBonus);

        else if (owner is Entity entity)
            return cooldown + entity.EntityData.attackCooldown;
        
        else return cooldown;
    }
    public Weapon(WeaponData data, IWeaponUser? owner = null)
    {
        this.data = data;
        SetOwner(owner);
        Weaponname = data.weaponName;
        damage = data.damage;
        weaponType = data.weaponType;
        elementType = data.elementType;
        cooldown = data.cooldown;
        weaponPrefab = data.weaponPrefab;
        elementBuildup = data.elementBuildup;
    }
    


    public virtual void Attack(Vector2 direction) { }
    public abstract float CalculateDamage();
    public void SetOwner(IWeaponUser? owner) => this.owner = owner;
    public bool CanAttack() => Time.time >= attackTime + GetEffectiveCooldown();

    
}
