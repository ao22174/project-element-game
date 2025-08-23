using System;
using ElementProject.gameEnums;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;
#pragma warning disable CS8618
public abstract class Weapon
{
    public WeaponData data;
    public int elementBuildup;
    public string Weaponname;
    protected WeaponType weaponType;
    public ElementType elementType;
    public float cooldown;
    public float attackTime;
    public GameObject weaponPrefab;
    public Sprite icon;
     public Animator weaponAnimator;
    public int ammoCount;
    public int maxAmmo;
    public float reloadTime = 0.5f; // Default reload time
    public HandsNeeded handsNeeded = HandsNeeded.OneHanded;
    public float damageScaling;

    float GetEffectiveCooldown(Core ownerCore)
    {
        float atkSpeed = ownerCore.GetCoreComponent<Stats>()?.AttackSpeed ?? 1f;
        float cooldown = data.cooldown / atkSpeed;
        return cooldown;
    }
    public Weapon(WeaponData data)
    {
        this.data = data;
        damageScaling = data.damageScaling;
        Weaponname = data.weaponName;
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



    public virtual void Attack(Vector2 direction, Vector2 position, Core ownerCore, CombatStats combatStats, GameObject? weaponVisual = null)
    {
        if (CanAttack(ownerCore))
        {
            ammoCount--;
        }
    }

    public virtual void Reload()
    {
        if (ammoCount < data.maxAmmo)
        {
            ammoCount = data.maxAmmo;
        }
    }
    public bool CanAttack(Core ownerCore)
    {
        var status = ownerCore.GetComponent<Status>();
        bool isFrozen = status != null && status.IsFrozen;
        return !isFrozen && Time.time >= attackTime + GetEffectiveCooldown(ownerCore) && (ammoCount > 0 || maxAmmo == 0);
    }
}
