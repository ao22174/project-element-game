using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewShooterData", menuName = "Data/Enemy Data/Shooter Data")]
public class ShooterData : EntityData
{
    [SerializeField] public List<WeaponData> useableWeapons;
    
}