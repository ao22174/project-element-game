using UnityEngine;
using ElementProject.gameEnums;

public class FrostArrow : Buff
{
    public override string BuffName => "Icicle";
    private float cooldownEndTime;
    public bool OnCooldown => Time.time < cooldownEndTime;

    public override void OnAttack(GameObject target, float damage, Vector2 direction)
    {
        float random = Random.Range(0f, 1f);
        FrostArrowBuff data = (FrostArrowBuff)buffData;
        if (random > data.chanceToProc || OnCooldown) return;
        Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
        GameObject projectileGO = GameObject.Instantiate(data.freezeProjectile, target.transform.position, rotation);
        ProjectileIceArrow bullet = projectileGO.GetComponent<ProjectileIceArrow>();
        if (bullet != null)
        {
            bullet.Initialize(new BulletInfo(core, target.transform.position, direction, data.arrowSpeed, data.arrowDamage, 2f, data.elementBuildup, ElementType.Frost, core.Faction), data.duration);
            cooldownEndTime = Time.time + data.coolDown;
        }
    }
}