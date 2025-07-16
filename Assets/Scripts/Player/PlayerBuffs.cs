using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBuffs : MonoBehaviour
{
    private Dictionary<string, Buff> activeBuffs = new();

    public void AddBuff(Buff newBuff)
    {
        string name = newBuff.BuffName;

        if (activeBuffs.ContainsKey(name))
        {
            activeBuffs[name].stackCount++;
        }
        else
        {
            Buff clone = (Buff)Activator.CreateInstance(newBuff.GetType());
            clone.OnApply(gameObject);
            activeBuffs[name] = clone;
        }
    }

    public void RemoveBuff(string name)
    {
        if (activeBuffs.ContainsKey(name))
        {
            activeBuffs[name].OnRemove(gameObject);
            activeBuffs.Remove(name);
        }
    }

    public void OnAttack(ref float damage)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnAttack(gameObject, ref damage);
    }

    public void OnHitEnemy(GameObject enemy)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnHitEnemy(gameObject, enemy);
    }

    void Update()
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnUpdate(gameObject);
    }
}