using UnityEngine;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour
{
    public List<WeaponPickup> nearbyWeapons = new List<WeaponPickup>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            WeaponPickup weaponPickup = collision.GetComponent<WeaponPickup>();
            if (weaponPickup != null && !nearbyWeapons.Contains(weaponPickup)) nearbyWeapons.Add(weaponPickup);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            WeaponPickup weaponPickup = collision.GetComponent<WeaponPickup>();
            if (weaponPickup != null && nearbyWeapons.Contains(weaponPickup)) nearbyWeapons.Remove(weaponPickup);
        }
    }

    
}