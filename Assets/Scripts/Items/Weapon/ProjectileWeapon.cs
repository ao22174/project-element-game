using System;
using ElementProject;
using Unity.VisualScripting;
using UnityEngine;


public class ProjectileWeapon : Weapon
{
    Vector2 projectileSpawn;
    private ProjectileWeaponData projectileData;
    private int projectileCount;
    private float spreadAngle;
    private float projectileSpeed;
    private float projectileLifetime;
    private GameObject projectilePrefab;
    private float randomSpread;

    private GameObject muzzleFlashPrefab;

    public ProjectileWeapon(WeaponData data, Core core) : base(data, core)
    {
        if (data is ProjectileWeaponData projData) projectileData = projData;
        else throw new ArgumentException("WeaponData must be of type ProjectileWeaponData", nameof(data));
        projectileCount = projectileData.projectileCount;
        spreadAngle = projectileData.spreadAngle;
        projectileSpeed = projectileData.projectileSpeed;
        projectileLifetime = projectileData.projectileLifetime;
        projectilePrefab = projectileData.projectilePrefab;
        randomSpread = projectileData.RandomSpread;
        muzzleFlashPrefab = projectileData.muzzleFlashPrefab; 
    }

    public override void Attack(Vector2 direction, Vector2 position, GameObject weaponVisual)
    {

        Debug.Log("attemping to attack");
        if (!CanAttack()) return;
        GameObject.FindObjectOfType<CameraMouseOffset>().Shake(-direction, 0.5f, 0.2f);        if (muzzleFlashPrefab != null) GameObject.Instantiate(muzzleFlashPrefab, weaponVisual.transform.Find("fireOrigin"));
        base.Attack(direction, position);

        attackTime = Time.time;

        float startingSpread = -spreadAngle * (projectileCount - 1) / 2f;


        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startingSpread + (spreadAngle * i) + UnityEngine.Random.Range(-randomSpread, randomSpread);
            Vector2 rotatedDirection = Utilities.RotateVector(direction, angle);

            GameObject projectileGO = GameObject.Instantiate(projectilePrefab, position, Quaternion.identity);
            ProjectileBullet bullet = projectileGO.GetComponent<ProjectileBullet>();

            if (bullet != null)
                bullet.Initialize(new BulletInfo(core, position, rotatedDirection, projectileSpeed, CalculateDamage(), projectileLifetime, elementBuildup, elementType, core.Faction));
        }
         if (weaponAnimator != null)
    {
        weaponAnimator.transform.localRotation = Quaternion.identity; 
        weaponAnimator.Play("WeaponRecoil", 0, 0f);
        weaponAnimator.Update(0f); // Ensure it applies immediately
    }
       
    }

    public override float CalculateDamage()
    {
        float baseDamage = damage;
        return baseDamage;
    }
}