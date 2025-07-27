using System;
using ElementProject.gameEnums;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;
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
    public Sprite icon;
     public Animator weaponAnimator;
    public int ammoCount;
    public int maxAmmo;
    public float reloadTime = 0.5f; // Default reload time
    public HandsNeeded handsNeeded = HandsNeeded.OneHanded;

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
        icon = data.weaponIcon;
        maxAmmo = data.maxAmmo;
        ammoCount = maxAmmo;
        reloadTime = data.reloadTime > 0 ? data.reloadTime : 0.5f; // Ensure reload time is positive
        handsNeeded = data.handsNeeded;
    }
    
    public void SetInstance(GameObject instantiatedVisual)
{
    weaponAnimator = instantiatedVisual.GetComponent<Animator>();
}



    public virtual void Attack(Vector2 direction, Vector2 position, GameObject? weaponVisual = null)
    {
        if (CanAttack())
        {
            ammoCount--;
        }
    }
    public abstract float CalculateDamage();
    public void SetOwner(Core core) => this.core = core;

    public virtual void Reload()
    {
        if (ammoCount < data.maxAmmo)
        {
            ammoCount = data.maxAmmo;
        }
    }
    public bool CanAttack()
    {
        var status = core.GetComponent<Status>();
        bool isFrozen = status != null && status.IsFrozen;
        return !isFrozen && Time.time >= attackTime + GetEffectiveCooldown() && (ammoCount > 0 || maxAmmo == 0);
    }
}
