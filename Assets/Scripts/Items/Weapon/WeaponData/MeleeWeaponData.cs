using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Weapons/Melee Weapon")]
public class MeleeWeaponData : WeaponData
{
   [SerializeField] public float attackRange = 1.5f; // How far the attack reaches (can influence hitbox size)
[SerializeField] public float attackDuration = 0.2f; // Time the hitbox stays active
[SerializeField] public float attackDelay = 0.1f; // Delay before hitbox activates
[SerializeField] public float knockbackForce = 5f; // How much knockback is applied on hit
    [SerializeField] public int comboCount = 1;
}