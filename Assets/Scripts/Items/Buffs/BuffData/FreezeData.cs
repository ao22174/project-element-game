using UnityEngine;

[CreateAssetMenu(fileName = "FreezeBuff", menuName = "Buffs/FreezeBuff")]

public class FreezeData : BuffData
{
    [SerializeField] public GameObject freezeProjectile;
    [SerializeField] public float duration;
    [SerializeField] public float chanceToProc;
    public override Buff CreateBuffInstance()
    {
        return new IcicleBuff { buffData = this };
    }

}
