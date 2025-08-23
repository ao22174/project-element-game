using UnityEngine;
using System;

public static class CombatEvents
{
    public static event Action<EnemyDeathInfo>? OnEnemyKilled;

    public static void EnemyKilled(EnemyDeathInfo info)
    {
        OnEnemyKilled?.Invoke(info);
    }
}

public class EnemyDeathInfo
{
    public EntityData enemy;
    public GameObject killer;
    public Vector2 position;
    public Faction faction;
    public string enemyType;
    public float overkillDamage;

    public EnemyDeathInfo(EntityData enemy, GameObject killer,Faction faction, Vector2 position, string enemyType, float overkillDamage = 0f)
    {
        this.faction = faction;
        this.enemy = enemy;
        this.killer = killer;
        this.position = position;
        this.enemyType = enemyType;
        this.overkillDamage = overkillDamage;
    }
}