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
    public PlayerStatModifiers stats;

    public Weapon(WeaponData data, Player? player)
    {
        this.data = data;
        Weaponname = data.weaponName;
        if (player == null)
        {
            Debug.Log("Player is null");
        }
        this.player = player;

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
        stats = player?.stats;
    }
    public virtual void Drop(Vector2 dropPosition)
    {
        GameObject pickupPrefab = Resources.Load<GameObject>("Prefabs/Weapon/WeaponPickup");
        if (pickupPrefab == null)
        {
            Debug.LogWarning("WeaponPickup prefab not found!");
            return;
        }
        GameObject pickupGO = GameObject.Instantiate(pickupPrefab, dropPosition, Quaternion.identity);
        WeaponPickup pickup = pickupGO.GetComponent<WeaponPickup>();
        if (pickup != null)
        {
            pickup.Initialize(this); // Pass in the current weapon to the pickup
        }
    }
}
