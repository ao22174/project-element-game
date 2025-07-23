using UnityEngine;
using ElementProject.gameEnums;
using System.Threading;


public struct DamageInfo
{
    public float amount;
    public GameObject source;
    public OwnedBy owner;
    public Vector2 direction;
    public ElementType element;
    public bool isCrit;
    public int elementBuildup;

    public DamageInfo(GameObject source = null, Vector2 direction = default, ElementType element = ElementType.None, bool isCrit = false, OwnedBy ownedBy = OwnedBy.All, float amount = 0f, int elementBuildup = 0)
    {
        this.amount = amount;
        this.source = source;
        this.owner = ownedBy;
        this.direction = direction;
        this.element = element;
        this.isCrit = isCrit;
        this.elementBuildup = elementBuildup;
    }
}
public interface IDamageable
{
    void TakeDamage(DamageInfo info);
}