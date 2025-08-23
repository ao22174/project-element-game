using UnityEngine;
using ElementProject.gameEnums;
using System.Threading;


public struct DamageInfo
{
    public float amount;
    public Core sourceCore;
    public Faction faction;
    public Vector2 direction;
    public ElementType element;
    public bool isCrit;
    public int elementBuildup;

    public DamageInfo(Core core = null, Vector2 direction = default, ElementType element = ElementType.None, bool isCrit = false, Faction faction = Faction.Neutral, float amount = 0f, int elementBuildup = 0)
    {
        this.amount = amount;
        this.sourceCore = core;
        this.faction = faction;
        this.direction = direction;
        this.element = element;
        this.isCrit = isCrit;
        this.elementBuildup = elementBuildup;
    }
}
public interface IDamageable
{
    public Faction Faction { get; }
    void TakeDamage(DamageInfo info);
}