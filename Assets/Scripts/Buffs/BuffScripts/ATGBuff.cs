using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;

public class ATGBuff : Buff
{
    public override string BuffName => "ATG";
    private ATGData missileData;
    public override void OnHitEnemy(GameObject target, DamageInfo info, GameObject enemy)
    {
        Debug.Log("Attempting to launch missile");
        missileData = (ATGData)buffData;
        if (UnityEngine.Random.Range(0, 1f) > missileData.baseProcChance) return;

        base.OnHitEnemy(target, info, enemy);

        GameObject missileObj = Object.Instantiate(missileData.missile, target.transform.position, Quaternion.identity);
        Missile missile = missileObj.GetComponent<Missile>();
        missile.Initialize(new BulletInfo(core, null, core.transform.position, Vector2.zero, 3f, 20f, 5f, 0, ElementProject.gameEnums.ElementType.Fire, core.Faction), enemy.transform);

    }
}