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

    public ProjectileWeapon(WeaponData data, Core core) : base(data, core)
    {
        if (data is ProjectileWeaponData projData) projectileData = projData;
        else throw new ArgumentException("WeaponData must be of type ProjectileWeaponData", nameof(data));
        projectileCount = projectileData.projectileCount;
        spreadAngle = projectileData.spreadAngle;
        projectileSpeed = projectileData.projectileSpeed;
        projectileLifetime = projectileData.projectileLifetime;
        projectilePrefab = projectileData.projectilePrefab;
    }

    public override void Attack(Vector2 direction, Vector2 position)
    {
        base.Attack(direction, position);
        if (!CanAttack()) return;

        attackTime = Time.time;

        float startingSpread = -spreadAngle * (projectileCount - 1) / 2f;


        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startingSpread + (spreadAngle * i);
            Vector2 rotatedDirection = Utilities.RotateVector(direction, angle);

            GameObject projectileGO = GameObject.Instantiate(projectilePrefab, position, Quaternion.identity);
            ProjectileBullet bullet = projectileGO.GetComponent<ProjectileBullet>();

            if (bullet != null)
                bullet.Initialize(new BulletInfo(core, position, rotatedDirection, projectileSpeed, CalculateDamage(), projectileLifetime, elementBuildup, elementType, core.Faction));
        }
    }

    public override float CalculateDamage()
    {
        float baseDamage = damage;
        return baseDamage;
    }
}