using System;
using UnityEngine;
using System.Collections.Generic;
public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Visuals")]
    public GameObject currentWeaponVisual;
    public Transform fireOrigin;
    public Transform oneHandedHoldPoint;
    public Transform twoHandedHoldPoint;
    public GameObject rightHandTransform;
    public GameObject leftHandTransform;
    public Animator animator;

    [Header("Weapons")]
    public List<Weapon> weapons = new();
    public Weapon currentWeapon;
    public int currentWeaponIndex;
    public Core core;

    public void Initialize(WeaponData weaponData, Core core)
    {
        this.core = core;
        animator = GetComponent<Animator>();
        weapons.Add(WeaponFactory.CreateWeapon(weaponData, core));
        EquipWeapon(currentWeaponIndex);
    }

    public void AddToInventory(Weapon weapon)
    {
        weapon.SetOwner(core);
        if (weapons.Count >= 2) // If full inventory
        {
            WeaponPickupFactory.Create(currentWeapon, transform.position);
            weapons[currentWeaponIndex] = weapon;
        }
        else weapons.Add(weapon);

        if (weapons.Count == 0) EquipWeapon(0);
        else EquipWeapon(currentWeaponIndex);

    }
    public void EquipWeapon(int index)
    {
        if(index < 0 || index >= weapons.Count)
        {
            Debug.LogWarning("Invalid weapon index: " + index);
            return;
        }
        currentWeaponIndex = index;
        currentWeapon = weapons[index];
        core.GetCoreComponent<UIManager>()?.weaponDisplay.SetWeapon(currentWeapon);

        if (currentWeaponVisual != null)
            Destroy(currentWeaponVisual);

        if (currentWeapon.weaponPrefab != null)
        {
            if (currentWeapon.handsNeeded == HandsNeeded.TwoHanded)
            {
                leftHandTransform.SetActive(true);
                rightHandTransform.SetActive(true);
                animator.SetInteger("HandsNeeded", 2);

                currentWeaponVisual = Instantiate(currentWeapon.weaponPrefab, twoHandedHoldPoint);

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
}