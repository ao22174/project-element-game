using UnityEngine;
using System.Collections.Generic;

public class ShooterAttackBehaviour : MonoBehaviour, IAttackBehavior, IAimBehavior, IStartupBehaviour
{
    public Transform weaponHoldPoint;
    public WeaponHandler weaponHandler;
    public Transform pivotPoint;
    public SpriteRenderer spriteRenderer;
    public List<WeaponData> useableWeapons;

    private Transform target;
    public Core ownerCore;

    // --- Burst fire settings ---
    private int weaponIndex = 0;
    private int burstAmount = 5;       // bullets per burst
    private float burstCooldown = 2f;  // seconds between bursts
    private float timeBetweenShots = 0.15f; // delay between bullets in a burst

    private int burstShotsFired = 0;
    private float lastShotTime = 0f;
    private float nextBurstReadyTime = 0f;
    private bool bursting = false;

    // --- IAimBehavior ---
    public void AimAtTarget(Transform target)
    {
        if (weaponHandler.currentWeaponVisual == null || target == null) return;

        Vector2 direction = ((Vector2)target.position - (Vector2)weaponHoldPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pivotPoint.rotation = Quaternion.Euler(0, 0, angle);

        bool flip = angle > 90 || angle < -90;
        weaponHandler.currentWeaponVisual.transform.localScale = new Vector3(1, flip ? -1 : 1, 1);
        spriteRenderer.flipX = flip;

        this.target = target;
    }

    // --- IAttackBehavior ---
    public void DoAttack()
    {
        if (weaponHandler.currentWeapon == null || target == null) return;

        // If not in a burst, can we start a new one?
        if (!bursting && Time.time >= nextBurstReadyTime)
        {
            bursting = true;
            burstShotsFired = 0;
        }

        // If in a burst, try firing shots
        if (bursting && Time.time - lastShotTime >= timeBetweenShots)
        {
            FireOneShot();

            burstShotsFired++;
            lastShotTime = Time.time;

            // If finished burst, go on cooldown
            if (burstShotsFired >= burstAmount)
            {
                bursting = false;
                nextBurstReadyTime = Time.time + burstCooldown;
            }
        }
        weaponHandler.currentWeapon.Reload();
    }

    private void FireOneShot()
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)weaponHoldPoint.position).normalized;
        CombatStats stats = ownerCore.GetCoreComponent<Stats>().GetSnapshot();
        weaponHandler.currentWeapon.Attack(direction, weaponHandler.fireOrigin.position, stats, ownerCore.Faction);
    }

    // --- IStartupBehaviour ---
    public void OnStart()
    {
        if (useableWeapons.Count > 0)
        {
            weaponIndex = Random.Range(0, useableWeapons.Count);
            weaponHandler.Initialize(useableWeapons[weaponIndex], ownerCore);
        }
        SetupBurstFromWeapon();
    }

    private void SetupBurstFromWeapon()
    {
        if (weaponHandler.currentWeapon != null)
        {
            // Example: burst = 20% of weapon's ammo
            burstAmount = Mathf.Max(1, Mathf.RoundToInt(weaponHandler.currentWeapon.maxAmmo * 0.2f));

            // Scale burst cooldown with weapon's base cooldown
            burstCooldown = weaponHandler.currentWeapon.cooldown * Random.Range(1.0f, 2.5f) + 1f;

            // Optionally tie shot spacing to weapon fire rate
            timeBetweenShots = weaponHandler.currentWeapon.cooldown * 0.5f;
        }
    }
}
