using System.Collections;
using System.Collections.Generic;
using ElementProject.gameEnums;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D hitboxCollider;
    private bool isActive = false;

    private float knockback;
    private float damageScaling;
    private CombatStats combatStats;
    private ElementType elementType;
    private Core attackerCore;
    private int elementBuildup;
    private HashSet<IDamageable> alreadyHit = new();

    private void Awake()
    {
        if (hitboxCollider == null)
            hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false;
    }

    public void ActivateHitbox(float delay, float duration, float knockback, float damageScaling, CombatStats combatStats, ElementType elementType, int elementBuildup, Core attackerCore)
    {
        this.knockback = knockback;
        this.elementBuildup = elementBuildup;
        this.damageScaling = damageScaling;
        this.combatStats = combatStats;
        this.elementType = elementType;
        this.attackerCore = attackerCore;

        alreadyHit.Clear(); // clear previous hits for this swing
        StartCoroutine(DoHitbox(delay, duration));
    }

    private IEnumerator DoHitbox(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        isActive = true;
        hitboxCollider.enabled = true;

        yield return new WaitForSeconds(duration);

        hitboxCollider.enabled = false;
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.TryGetComponent(out IDamageable target))
        {
            Vector2 origin = transform.position;
            Vector2 targetPos = other.transform.position;
            Vector2 dir = (targetPos - origin).normalized;
            float distance = Vector2.Distance(origin, targetPos);

            LayerMask wallMask = LayerMask.GetMask("Wall"); 
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, wallMask);

            if (hit.collider != null)
            {
                Debug.Log("Melee blocked by wall");
                return;
            }
            if (target.Faction == attackerCore.Faction) return;
            if (alreadyHit.Contains(target)) return;
            alreadyHit.Add(target);
            GameObject.FindObjectOfType<CameraMouseOffset>().Shake(dir, 0.5f, 0.2f);
            if (other.TryGetComponent(out Core hitCore))
            {
                target.TakeDamage(new DamageInfo
                (attackerCore,
                dir,
                elementType,
                false,
                attackerCore.Faction,
                 DamageCalculator.CalculateGenericDamage(combatStats, damageScaling, elementType, hitCore),
                 elementBuildup));
            }
            else target.TakeDamage(new DamageInfo(attackerCore, dir, elementType, false, attackerCore.Faction,  DamageCalculator.CalculateGenericDamage(combatStats, damageScaling, elementType, hitCore), elementBuildup));
        }
    }
}
