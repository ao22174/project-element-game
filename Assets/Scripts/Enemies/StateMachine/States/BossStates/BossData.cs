using UnityEngine;
using System;
using System.Collections.Generic;
using ElementProject.gameEnums;


[CreateAssetMenu(fileName = "NewBossData", menuName = "Data/Enemy Data/Boss Data")]
public class BossData : EntityData
{
    public float jumpDamageScaling = 0.5f;
    public int jumpBuildup = 50;
}