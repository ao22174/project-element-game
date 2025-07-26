using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class WeaponFactory
{
    private static Dictionary<Type, Func<WeaponData, Core, Weapon>> creators 
        = new Dictionary<Type, Func<WeaponData, Core, Weapon>>();

    static WeaponFactory()
    {
        creators[typeof(ProjectileWeaponData)] = (data,  core) => new ProjectileWeapon((ProjectileWeaponData)data, core);
        // Register more weapon types here...
    }

    public static Weapon CreateWeapon(WeaponData data, Core core)
    {
        var type = data.GetType();
        if (!creators.TryGetValue(type, out var creator))
        {
            throw new NotSupportedException(type + " is not supported currently in the weaponFactory, consider adding implementation");
        }
        return creator(data, core);
    }
}