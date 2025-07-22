using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class WeaponFactory
{
    private static Dictionary<Type, Func<WeaponData, IWeaponUser?, Weapon>> creators 
        = new Dictionary<Type, Func<WeaponData, IWeaponUser?, Weapon>>();

    static WeaponFactory()
    {
        creators[typeof(ProjectileWeaponData)] = (data,  owner) => new ProjectileWeapon((ProjectileWeaponData)data, owner);
        // Register more weapon types here...
    }

    public static Weapon CreateWeapon(WeaponData data, IWeaponUser owner)
    {
        var type = data.GetType();
        if (!creators.TryGetValue(type, out var creator))
        {
            throw new NotSupportedException(type + " is not supported currently in the weaponFactory, consider adding implementation");
        }
        return creator(data, owner);
    }
}