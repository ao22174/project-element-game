using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileWeapon", menuName = "Weapons/Projectile Weapon")]
public class ProjectileWeaponData : WeaponData
{
    public GameObject projectilePrefab = null!;
    public float projectileSpeed;
    public int projectileCount;
    public float spreadAngle;

    public float projectileLifetime;
}