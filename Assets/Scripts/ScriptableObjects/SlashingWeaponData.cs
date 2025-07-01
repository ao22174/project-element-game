using UnityEngine;


[CreateAssetMenu(fileName = "NewProjectileWeapon", menuName = "Weapons/Projectile Weapon")]
public class SlashingWeaponData : WeaponData
{
    public float SlashSize;
    public int projectileCount;
    public float spreadAngle;

    public float projectileLifetime;
}