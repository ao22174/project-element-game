using ElementProject.gameEnums;
using UnityEditor;
using UnityEngine;
public class Overload : Reaction
{
    public override float baseDamage => 20f;
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
        core.GetCoreComponent<Combat>()?.TakeDamage(new DamageInfo
        {
            amount = DamageCalculator.CalculateReactionDamage(source, this, core),
            element = ElementType.Lightning,
            sourceCore = source
        });
        Debug.Log(explosionPrefab != null ? "Explosion prefab is set." : "Explosion prefab is not set.");
        Explosion explosion = GameObject.Instantiate(explosionPrefab, core.transform.position, Quaternion.identity).GetComponent<Explosion>();
        if (explosion != null)
        {
            explosion.Initialize(core.transform.position, damageRadius, 50f, ElementType.Lightning,damageLayer, source);
        }
        else
        {
            Debug.LogError("Explosion component not found on the explosion prefab.");
        }
    }

}