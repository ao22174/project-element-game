using UnityEngine;
using System;

public static class WeaponPickupFactory
{
    public static WeaponPickup Create(Weapon weapon, Vector2 position)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Weapon/WeaponPickup");
        if (prefab == null)
        {
            throw new NullReferenceException("WeaponPickup prefab not found!");
        }

        GameObject instance = GameObject.Instantiate(prefab, position, Quaternion.identity);
        WeaponPickup pickup = instance.GetComponent<WeaponPickup>();

        if (pickup == null)
        {
            throw new NullReferenceException("WeaponPickup component missing on prefab!");
        }

        pickup.Initialize(weapon);
        return pickup;
    }
}