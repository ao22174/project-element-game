using UnityEngine;
using UnityEngine.Scripting;

public class IcicleBuff : Buff
{
    public override string BuffName => "Icicle";


    public GameObject iciclePrefab;

    public override void OnAttack(GameObject target, ref float damage)
    {
        base.OnAttack(target, ref damage);
        

    }
}