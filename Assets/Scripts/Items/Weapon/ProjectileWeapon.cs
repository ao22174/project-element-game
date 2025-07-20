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
    public ProjectileWeapon(WeaponData data, Player? player) : base(data, player)
    {
        if (data is ProjectileWeaponData projData)
        {
            projectileData = projData;
        }
        else
        {
            throw new ArgumentException("WeaponData must be of type ProjectileWeaponData", nameof(data));
        }
        projectileCount = projectileData.projectileCount;
        spreadAngle = projectileData.spreadAngle;
        projectileSpeed = projectileData.projectileSpeed;
        projectileLifetime = projectileData.projectileLifetime;
        projectilePrefab = projectileData.projectilePrefab;
    }

    public override void Attack(Vector2 direction)
    {
        if (player == null)
        {
            Debug.Log("Player Does not exist");
            return;
        }
        float startingSpread = -spreadAngle * (projectileCount - 1) / 2f;
        projectileSpawn = player.GetFireOrigin().transform.position;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startingSpread + (spreadAngle * i);
            Vector2 rotatedDirection = Utilities.RotateVector(direction, angle);
            GameObject projectileGO = GameObject.Instantiate(projectilePrefab, projectileSpawn, Quaternion.identity);
            ProjectileBullet bullet = projectileGO.GetComponent<ProjectileBullet>();
            Debug.DrawRay(projectileSpawn, direction * 5f, Color.red, 2f);
            if (bullet != null)
            {
                bullet.Initialize(projectileSpawn, rotatedDirection, projectileSpeed, CalculateDamage(), projectileLifetime, OwnedBy.Player);
            }
        }
    }
    public override float CalculateDamage() => (damage * (1 + (player != null ? player.stats.percentAttackBonus : 0f)) +
                                                            (player != null ? player.stats.flatAttackBonus : 0f)) *
                                                            (1 + (player != null ? player.stats.GetDamageBonus(elementType) : 0f));
}