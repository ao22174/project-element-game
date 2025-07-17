using UnityEngine;

public enum buffType{buff, debuff}

public abstract class BuffData : ScriptableObject
{
    [SerializeField] public string buffName = "DefaultBuff";
    [SerializeField] public Sprite buffSpirte;
    [SerializeField] public buffType type;
    
    public abstract Buff CreateBuffInstance();


}
