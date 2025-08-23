using System;
using ElementProject;
using Unity.VisualScripting;
using UnityEngine;
#pragma warning disable CS8618

public class MeleeWeapon : Weapon
{
    public float attackRange = 1.5f; // How far the attack reaches (can influence hitbox size)
    public float attackDuration =   0.2f; // Time the hitbox stays active
    public float attackDelay =  0.1f; // Delay before hitbox activates
     public float knockbackForce = 5f; // How much knockback is applied on hit

    public int comboCount = 1;
    private int currentCombo = 0;
    public MeleeWeapon(WeaponData data) : base(data)
    {
        if (data is not MeleeWeaponData meleeData)
            throw new ArgumentException("WeaponData must be of type MeleeWeaponData", nameof(data));
        attackRange = meleeData.attackRange;
        attackDuration = meleeData.attackDuration;
        attackDelay = meleeData.attackDelay;
        knockbackForce = meleeData.knockbackForce;
        comboCount = meleeData.comboCount;

    }

    public override void Attack(Vector2 direction, Vector2 position, Core ownerCore,CombatStats combatStats, GameObject? weaponVisual)
    {
        if (!CanAttack(ownerCore)) return;
        if (weaponVisual == null)
        {
            Debug.LogWarning("Weapon visual is null.");
            return;
        }
        attackTime = Time.time;

        currentCombo = (currentCombo + 1) % comboCount;

    // Activate the correct hitbox
    var hitboxObj = weaponVisual.transform.Find($"Hitbox_{currentCombo}");
    if (hitboxObj == null)
    {
        Debug.LogWarning($"Hitbox_{currentCombo} not found on weapon prefab.");
        return;
    }
        if (weaponAnimator != null)
        {
            weaponAnimator.Play("Attack1", 0, 0f);
    }
 
        var meleeHitbox = hitboxObj.GetComponent<MeleeHitbox>();
        if (meleeHitbox != null)
          meleeHitbox.ActivateHitbox(attackDelay, attackDuration,  knockbackForce, damageScaling, combatStats, elementType, elementBuildup, ownerCore);
}

    
}