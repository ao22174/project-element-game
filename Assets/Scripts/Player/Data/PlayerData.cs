using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName ="Data/Player Data/ Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Serialize] public float movementSpeed;
        [Serialize] public float dashSpeed;
        [Serialize] public float dashCooldown;
        [Serialize] public float dashDuration;
    }
