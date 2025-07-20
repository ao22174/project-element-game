using System;
using System.Runtime.InteropServices;
using ElementProject.gameEnums;
using UnityEngine;
public abstract class Weapon
{
    public WeaponData data;
    protected Player? player;
    protected float damage;
    public string Weaponname;
    protected WeaponType weaponType;
    protected ElementType elementType;
    public float cooldown;
    public GameObject weaponPrefab;
    public Weapon(WeaponData data, Player? player = null)
    {
        this.data = data;
        SetOwner(player);
        Weaponname = data.weaponName;
        damage = data.damage;
        weaponType = data.weaponType;
        elementType = data.elementType;
        cooldown = data.cooldown;
        weaponPrefab = data.weaponPrefab;
    }


    public abstract void Attack(Vector2 direction);
    
    public abstract float CalculateDamage();
    public void SetOwner(Player? player)
    {
        this.player = player;
    }
}
