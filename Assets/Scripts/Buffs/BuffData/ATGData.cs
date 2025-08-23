using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

    [CreateAssetMenu(fileName = "ATG", menuName = "Buffs/ATGData")]

    public class ATGData: BuffData
    {
        [SerializeField] public GameObject missile;
        [SerializeField] public float damage = 30f;
        [SerializeField] public float damageRadius= 3f;
    [SerializeField] public float baseProcChance = 0.2f;
    
        public override Buff CreateBuffInstance()
    {
        return new ATGBuff { buffData = this };
    }

    }
