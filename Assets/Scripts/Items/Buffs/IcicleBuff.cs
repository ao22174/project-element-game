using UnityEngine;
using UnityEngine.Scripting;

public class IcicleBuff : Buff
{
    public override string BuffName => "Icicle";
    private float cooldownEndTime;
    public bool OnCooldown => Time.time < cooldownEndTime;

    public override void OnAttack(GameObject target, float damage, Vector2 direction)
    {
        float random = Random.Range(0f, 1f);
        FrostArrowBuff data = (FrostArrowBuff)buffData;
        if (random > data.chanceToProc) return;

        if (OnCooldown) return;
        Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
        GameObject projectileGO = GameObject.Instantiate(data.freezeProjectile, target.transform.position, rotation);
        ProjectileFreezer bullet = projectileGO.GetComponent<ProjectileFreezer>();
        if (bullet != null)
        {
            bullet.Initialize(target.transform.position, direction, data.arrowSpeed, data.arrowDamage, 2f, data.duration, OwnedBy.Player);
            cooldownEndTime = Time.time + data.coolDown;
        }
    }
}