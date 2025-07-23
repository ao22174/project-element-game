using System;
using ElementProject;
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
    public ProjectileWeapon(WeaponData data, IWeaponUser owner) : base(data, owner)
    {
        if (data is ProjectileWeaponData projData) projectileData = projData;
        else throw new ArgumentException("WeaponData must be of type ProjectileWeaponData", nameof(data));
        
        projectileCount = projectileData.projectileCount;
        spreadAngle = projectileData.spreadAngle;
        projectileSpeed = projectileData.projectileSpeed;
        projectileLifetime = projectileData.projectileLifetime;
        projectilePrefab = projectileData.projectilePrefab;
    }

    public override void Attack(Vector2 direction)
    {
        base.Attack(direction);
        if (!CanAttack()) return;
        
        attackTime = Time.time;
        if (owner == null) throw new Exception("null");
        
        float startingSpread = -spreadAngle * (projectileCount - 1) / 2f;
        projectileSpawn = owner.GetFirePoint().transform.position;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startingSpread + (spreadAngle * i);
            Vector2 rotatedDirection = Utilities.RotateVector(direction, angle);
            GameObject projectileGO = GameObject.Instantiate(projectilePrefab, projectileSpawn, Quaternion.identity);
            ProjectileBullet bullet = projectileGO.GetComponent<ProjectileBullet>();
            if (bullet != null)
                bullet.Initialize(new BulletInfo(projectileSpawn,rotatedDirection, projectileSpeed, CalculateDamage(), projectileLifetime, elementBuildup, elementType, owner));
            
        }
    }
     
    public override float CalculateDamage()
    {
    float baseDamage = damage;

        if (owner is Player player)
        {
            baseDamage = damage * (1 + player.stats.percentAttackBonus) + player.stats.flatAttackBonus * player.stats.GetDamageBonus(elementType);
        }
        else if (owner is Entity)
        {
            Debug.LogWarning("need to implement enemy damage calculation");
        }

    return baseDamage;
}
}