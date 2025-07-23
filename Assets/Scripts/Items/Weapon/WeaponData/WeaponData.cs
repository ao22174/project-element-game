using UnityEngine;
using ElementProject.gameEnums;

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
    
}
