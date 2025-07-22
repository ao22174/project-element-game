using UnityEngine;
using System;

public class CorpseBloomBuff : Buff
{
    public override string BuffName => "Corpsebloom";

    public override void OnKill(GameObject target, Vector2 position)
    {
        base.OnKill(target, position);
        if (buffData is not CorpseBloomData data)
            throw new InvalidCastException($"BuffData is not of type CorpseBloomData. It is {buffData.GetType().Name} instead.");

        GameObject bloomObj = GameObject.Instantiate(data.bloom, position, Quaternion.identity);
        bloomObj.GetComponent<Bloom>().Initialize(data.damage, data.damageRadius, data.detonationTime);

    }
}