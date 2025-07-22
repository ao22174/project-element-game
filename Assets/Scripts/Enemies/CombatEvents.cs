using UnityEngine;
using System;

public static class CombatEvents
{
    public static event Action<EnemyDeathInfo> OnEnemyKilled;

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
    public OwnedBy source;
    public string enemyType;
    public float overkillDamage;

    public EnemyDeathInfo(EntityData enemy, GameObject killer, OwnedBy source, Vector2 position, string type, float overkill = 0f)
    {
        this.source = source;
        this.enemy = enemy;
        this.killer = killer;
        this.position = position;
        this.enemyType = type;
        this.overkillDamage = overkill;
    }
}