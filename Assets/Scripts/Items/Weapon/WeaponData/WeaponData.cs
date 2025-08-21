using UnityEngine;
using ElementProject.gameEnums;
using UnityEditor.Animations;
using Unity.Burst.Intrinsics;

public enum HandsNeeded
{
    OneHanded,
    TwoHanded
}
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] public string weaponName = "DefaultWeapon";
    [SerializeField] public int damage = 1;
    [SerializeField] public Rarity rarity = Rarity.Common;

    [SerializeField] public int elementBuildup = 1;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public ElementType elementType;
    [SerializeField] public float cooldown = 1;
    [SerializeField] public GameObject weaponPrefab;
    [SerializeField] public Sprite weaponIcon;
    [SerializeField] public int maxAmmo = 0;  // 0 means infinite ammo, >0 means limited ammo 
    [SerializeField] public float reloadTime = 0.5f; // Default reload time
    [SerializeField] public float scaling = 1f;
    [SerializeField] public HandsNeeded handsNeeded = HandsNeeded.OneHanded;
    
}
