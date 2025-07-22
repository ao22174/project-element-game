using ElementProject.gameEnums;
using UnityEngine;

public enum buffType{buff, debuff}

public abstract class BuffData : ScriptableObject
{
    [SerializeField] public string buffName = "DefaultBuff";
    [SerializeField] public Sprite buffSpirte;
    [SerializeField] public buffType type;
    [SerializeField] public ElementType element;
    [SerializeField] public string buffDescription ="Lorem Ipsum";
    
    public abstract Buff CreateBuffInstance(IWeaponUser user);


}
