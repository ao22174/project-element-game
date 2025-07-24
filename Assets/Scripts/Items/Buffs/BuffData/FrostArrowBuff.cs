    using UnityEngine;

    [CreateAssetMenu(fileName = "FreezeBuff", menuName = "Buffs/FreezeBuff")]

    public class FrostArrowBuff : BuffData
    {
        [SerializeField] public GameObject freezeProjectile;
        [SerializeField] public float duration = 4f;
        [SerializeField] public float chanceToProc= 0.1f;
        [SerializeField] public float arrowDamage= 10f;
        [SerializeField] public float arrowSpeed = 5f;

        [SerializeField] public float coolDown = 1f;
        [SerializeField] public int elementBuildup = 20;
        public override Buff CreateBuffInstance()
    {
        return new IcicleBuff { buffData = this};
    }

    }
