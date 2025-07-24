using System;
using System.Collections.Generic;
using UnityEngine;
using ElementProject.gameEnums;

public class Buffs : CoreComponent
{
    private readonly Dictionary<string, Buff> activeBuffs = new();

    public bool HasBuff(string name) => activeBuffs.ContainsKey(name);

    public int GetStackCount(string name)
    {
        return activeBuffs.TryGetValue(name, out var buff) ? buff.stackCount : 0;
    }

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
            clone.buffData = newBuff.buffData;
            clone.core = core;
            clone.OnApply(core.gameObject);
            activeBuffs[name] = clone;
        }
    }

    public void RemoveBuff(string name)
    {
        if (activeBuffs.TryGetValue(name, out var buff))
        {
            buff.OnRemove(core.gameObject);
            activeBuffs.Remove(name);
        }
    }

    public void OnAttack(GameObject target, float damage, Vector2 direction = default)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnAttack(target, damage, direction);
    }

    public void OnHitEnemy(GameObject enemy)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnHitEnemy(core.gameObject, enemy);
    }

    public void OnDash()
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnDash(core.gameObject);
    }

    public void OnKill(GameObject target, Vector2 position)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnKill(target, position);
    }

    public void OnReaction(GameObject target, GameObject reactor, ElementType primary, ElementType secondary)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnReaction(target, reactor, primary, secondary);
    }

    public void OnUpdate()
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnUpdate(core.gameObject);
    }
}