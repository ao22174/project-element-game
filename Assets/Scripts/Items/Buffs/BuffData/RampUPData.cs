using UnityEngine;

[CreateAssetMenu(fileName = "RampUpData", menuName = "Buffs/RampUPData")]

public class RampUPData: BuffData
{
    [SerializeField] public float maxAttackSpeedBeforeReset =20f;
    [SerializeField] public float increments = 1f;
    public override Buff CreateBuffInstance(IWeaponUser user)
    {
        return new RampingAttackSpeedBuff { buffData = this, user = user };
    }

}
