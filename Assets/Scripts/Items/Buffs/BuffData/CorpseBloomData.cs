using UnityEditor.EditorTools;
using UnityEngine;

    [CreateAssetMenu(fileName = "CorpseBloom", menuName = "Buffs/CorpseBloom")]

    public class CorpseBloomData : BuffData
    {
        [SerializeField] public GameObject bloom;
        [SerializeField] public float damage = 30f;
        [SerializeField] public float damageRadius= 3f;
        [SerializeField] public float detonationTime = 3f;


        [SerializeField] public float killsRequired = 3f; 
        public override Buff CreateBuffInstance(IWeaponUser user)
        {
            return new CorpseBloomBuff { buffData = this, user = user };
        }

    }
