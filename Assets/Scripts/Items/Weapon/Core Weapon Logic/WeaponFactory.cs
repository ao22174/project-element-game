
using System;
using System.Collections.Generic;

public static class WeaponFactory
{
    private static Dictionary<Type, Func<WeaponData, Weapon>> creators 
        = new Dictionary<Type, Func<WeaponData, Weapon>>();

    static WeaponFactory()
    {
        creators[typeof(ProjectileWeaponData)] = (data) => new ProjectileWeapon((ProjectileWeaponData)data);
        creators[typeof(MeleeWeaponData)] = (data) => new MeleeWeapon((MeleeWeaponData)data);
        // Register more weapon types here...
    }

    public static Weapon CreateWeapon(WeaponData data)
    {
        var type = data.GetType();
        if (!creators.TryGetValue(type, out var creator))
        {
            throw new NotSupportedException(type + " is not supported currently in the weaponFactory, consider adding implementation");
        }
        return creator(data);
    }
}