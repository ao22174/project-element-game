using ElementProject.gameEnums;
using UnityEditor;
using UnityEngine;
public class Overload : Reaction
{
    public override float baseScaling => 0.3f;
    public override string ReactionName => "Overload";
    private GameObject explosionPrefab;
    private float damageRadius = 0.5f;
    private LayerMask damageLayer;

    public Overload(LayerMask damageLayer, GameObject explosionPrefab)
    {
        this.damageLayer = damageLayer;
        this.explosionPrefab = explosionPrefab;
    }   

    public override void Apply(Core core, Core source)
    {
        CombatStats stats;
        if (source != null) stats = source.GetCoreComponent<Stats>().GetCombatStats(ElementType.Fire);
        else stats = new CombatStats(5f, 1f, 0f);

            core.GetCoreComponent<Combat>()?.TakeDamage(new DamageInfo
            {
                amount = DamageCalculator.CalculateReactionDamage(stats, baseScaling, ElementType.Fire, ElementType.Lightning, core),
                element = ElementType.Lightning,
                sourceCore = source
            });
        Debug.Log(explosionPrefab != null ? "Explosion prefab is set." : "Explosion prefab is not set.");
        Explosion explosion = GameObject.Instantiate(explosionPrefab, core.transform.position, Quaternion.identity).GetComponent<Explosion>();
        if (explosion != null)
        {
            explosion.Initialize(core.transform.position, damageRadius, 50f, ElementType.Lightning,damageLayer, stats,source);
        }
        else
        {
            Debug.LogError("Explosion component not found on the explosion prefab.");
        }
    }

}