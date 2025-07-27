using UnityEngine;
using ElementProject.gameEnums;
using UnityEditor.Animations;

public enum HandsNeeded
{
    OneHanded,
    TwoHanded
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] public string weaponName = "DefaultWeapon";
    [SerializeField] public int damage = 1;

    [SerializeField] public int elementBuildup = 1;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public ElementType elementType;
    [SerializeField] public float cooldown = 1;
    [SerializeField] public GameObject weaponPrefab;
    [SerializeField] public Sprite weaponIcon;
    [SerializeField] public int maxAmmo = 0;  // 0 means infinite ammo, >0 means limited ammo 
    [SerializeField] public float reloadTime = 0.5f; // Default reload time

    [SerializeField] public HandsNeeded handsNeeded = HandsNeeded.OneHanded;
    
}
