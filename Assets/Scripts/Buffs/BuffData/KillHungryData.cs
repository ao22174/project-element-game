using UnityEditor.EditorTools;
using UnityEngine;

    [CreateAssetMenu(fileName = "KillHungry", menuName = "Buffs/KillHungry")]

    public class KillHungryData : BuffData
    {
        [SerializeField] public float attackIncreaseOnKill = 5f;
    [SerializeField] public float attackIncreaseOnKillPerStack = 1f;
        public override Buff CreateBuffInstance()
    {
        return new KillHungry { buffData = this };
    }

    }
