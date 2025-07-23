using UnityEngine;
using System.Collections;
using System;

public class HealthManager : MonoBehaviour,IDamageable
{
    private float maxHealth; // Assigned in case We want to deal with increasing maxHealth
    public event Action<DamageInfo>? OnDamaged;
    public event Action<float, float>? onHealthModifed;
    public bool usesIFrames = false;

    public event Action<GameObject, OwnedBy>? onDeath;

    private float currentHealth;
    private bool isInvincible;


    public void Initialize(float maxHealth)
    {
        currentHealth = maxHealth;
        onHealthModifed?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(DamageInfo info)
    {
        if (isInvincible) return;

        currentHealth -= info.amount;
        OnDamaged?.Invoke(info);
        onHealthModifed?.Invoke(currentHealth, maxHealth);
        if(usesIFrames)
        StartCoroutine(IFrames(0.5f));

        if (currentHealth < 0)
        {
            onDeath?.Invoke(info.source, info.owner);
        }
    }

    private IEnumerator IFrames(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
}