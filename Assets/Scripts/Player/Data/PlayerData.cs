using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName ="Data/Player Data/ Base Data")]
    public class PlayerData : CoreData
    {
        [Header("Additional Player Movement Settings")]
        [Serialize] public float dashSpeed;
        [Serialize] public float dashCooldown;
        [Serialize] public float dashDuration;
    }
