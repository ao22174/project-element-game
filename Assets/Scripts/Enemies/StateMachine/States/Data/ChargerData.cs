using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewChargerData", menuName = "Data/Enemy Data/Charger Data")]
public class ChargerData : EntityData
{
    public float chargeWindupTime;
    public float chargeRestTime;
    public float chargeDuration;
}