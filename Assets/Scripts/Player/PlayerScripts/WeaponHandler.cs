using System;
using UnityEngine;
using System.Collections.Generic;
#pragma warning disable CS8618
public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Visuals")]
    public GameObject currentWeaponVisual;
    public Transform fireOrigin;
    public Transform oneHandedHoldPoint;
    public Transform twoHandedHoldPoint;
    public Transform twoHandedMeleeHoldPoint;
    public GameObject rightHandTransform;
    public GameObject leftHandTransform;
    public Animator animator;

    [Header("Weapons")]
    public List<Weapon> weapons = new();
    public Weapon currentWeapon;
    public int currentWeaponIndex;
    private bool canAttack = true;
    private bool canWeaponSwap = true;
    public Core core;

    public void Initialize(WeaponData weaponData, Core core)
    {
        this.core = core;
        animator = GetComponent<Animator>();
        weapons.Add(WeaponFactory.CreateWeapon(weaponData));
        EquipWeapon(currentWeaponIndex);
    }

    public void AddToInventory(Weapon weapon)
    {
        if (weapons.Count >= 2) // If full inventory
        {
            WeaponPickupFactory.Create(currentWeapon, transform.position);
            weapons[currentWeaponIndex] = weapon;
        }
        else weapons.Add(weapon);

        if (weapons.Count == 0) EquipWeapon(0);
        else EquipWeapon(currentWeaponIndex);

    }
    public void SheatheWeapon()
    {
        currentWeaponVisual.SetActive(false);
        canAttack = false;
        canWeaponSwap = false;
    }
    public void UnsheatheWeapon()
    {
        currentWeaponVisual.SetActive(true);
        canAttack = true;
        canWeaponSwap = true;
    }
    public void EquipWeapon(int index)
    {
        if (!canWeaponSwap) return;
        if (index < 0 || index >= weapons.Count)
        {
            Debug.LogWarning("Invalid weapon index: " + index);
            return;
        }
        currentWeaponIndex = index;
        currentWeapon = weapons[index];
        core.GetCoreComponent<UIManager>()?.weaponDisplay?.SetWeapon(currentWeapon);

        if (currentWeaponVisual != null)
            Destroy(currentWeaponVisual);

        if (currentWeapon.weaponPrefab != null)
        {
            if (currentWeapon.handsNeeded == HandsNeeded.TwoHanded)
            {
                if (currentWeapon is MeleeWeapon)
                {
                    currentWeaponVisual = Instantiate(currentWeapon.weaponPrefab, twoHandedMeleeHoldPoint);
                }
                else
                    currentWeaponVisual = Instantiate(currentWeapon.weaponPrefab, twoHandedHoldPoint);
                leftHandTransform.SetActive(true);
                rightHandTransform.SetActive(true);
                animator.SetInteger("HandsNeeded", 2);


            }
            else if (currentWeapon.handsNeeded == HandsNeeded.OneHanded)
            {
                currentWeaponVisual = Instantiate(currentWeapon.weaponPrefab, oneHandedHoldPoint);
                animator.SetInteger("HandsNeeded", 1);

                rightHandTransform.SetActive(true);
                leftHandTransform.SetActive(false);

            }
            else
            {
                leftHandTransform.SetActive(false);
                rightHandTransform.SetActive(false);
            }
            currentWeapon.SetInstance(currentWeaponVisual);
            fireOrigin = currentWeaponVisual.transform.Find("fireOrigin");
            Transform leftHandAnchor = currentWeaponVisual.transform.Find("LeftHandAnchor");
            Transform rightHandAnchor = currentWeaponVisual.transform.Find("RightHandAnchor");
            if (leftHandAnchor != null)
            {
                leftHandTransform.transform.SetParent(leftHandAnchor, false);
            }
            if (rightHandAnchor != null)
            {
                rightHandTransform.transform.SetParent(rightHandAnchor, false);
            }
            else Debug.LogWarning("Missing hand anchors on weapon prefab: " + currentWeapon.Weaponname);


        }
        else Debug.LogWarning("Weapon prefab missing for: " + currentWeapon.Weaponname);
    }
    public void Attack()
    {
        if (!canAttack) return;
        if (currentWeapon is MeleeWeapon meleeWeapon)
        {
            meleeWeapon.Attack(transform.right, transform.position,core, currentWeaponVisual);
            core.GetCoreComponent<Buffs>().OnAttack(gameObject, currentWeapon.data.damage, currentWeaponVisual.transform.right);

        }
        else if (currentWeapon is ProjectileWeapon projectileWeapon)
        {
            projectileWeapon.Attack(fireOrigin.right, fireOrigin.position,core, currentWeaponVisual);
            core.GetCoreComponent<Buffs>().OnAttack(gameObject, currentWeapon.data.damage, fireOrigin.right);

        }
        else
        {
            Debug.LogWarning("Current weapon is not a Melee or Projectile weapon.");
        }
    }
}