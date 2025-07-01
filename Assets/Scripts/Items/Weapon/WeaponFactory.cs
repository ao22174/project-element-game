using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class WeaponFactory
{
    private static Dictionary<Type, Func<WeaponData, Player?, Weapon>> creators 
        = new Dictionary<Type, Func<WeaponData, Player?, Weapon>>();

    static WeaponFactory()
    {
        creators[typeof(ProjectileWeaponData)] = (data, player) => new ProjectileWeapon((ProjectileWeaponData)data, player);
        // Register more weapon types here...
    }

    public static Weapon? CreateWeapon(WeaponData data, Player? player)
    {
        var type = data.GetType();
        if (creators.TryGetValue(type, out var creator))
            return creator(data, player);
        Debug.LogWarning($"No weapon creator registered for {type.Name}");
        return null;
    }
}