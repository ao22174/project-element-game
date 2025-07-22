using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBuffs
{
    public Player player;
    public GameObject gameObject;
    private Dictionary<string, Buff> activeBuffs = new();
    public bool HasBuff(string name) => activeBuffs.ContainsKey(name);

    public int GetStackCount(string name)
    {
        return activeBuffs.TryGetValue(name, out var buff) ? buff.stackCount : 0;
    }
    public PlayerBuffs(Player player, GameObject gameObject)
    {
        this.player = player;
        this.gameObject = gameObject;
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
        clone.buffData = newBuff.buffData; // Assign the data
            clone.user = player;
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

    public void OnAttack(GameObject gameObject, float damage, Vector2 direction = default)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnAttack(gameObject, damage, direction);
    }

    public void OnHitEnemy(GameObject enemy)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnHitEnemy(gameObject, enemy);
    }
    public void OnDash()
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnDash();
    }

    public void OnKill(GameObject target, Vector2 position)
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnKill(target, position);
    }


    public void OnUpdate()
    {
        foreach (var buff in activeBuffs.Values)
            buff.OnUpdate(gameObject);
    }
}