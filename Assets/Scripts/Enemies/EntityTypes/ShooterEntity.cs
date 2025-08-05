using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterEntity : Entity
{
    // EXPECTED BEHAVIOUR --- Idle -> (On Sight) -> Idle -> (Out LOS) -> Chase
    public ShooterChase chaseState;
    public ShooterIdle chaserIdle;
    public ShooterAttack chaserAttack;
    public ShooterAttackIdle chaserAttackIdle;
    public SpriteRenderer spriteRenderer;

    public ShooterData? shooterData;
    public Transform weaponHoldPoint;
    public WeaponHandler weaponHandler;
    public Transform pivotPoint;

    public override void Start()
    {
        base.Start();
        chaseState = new ShooterChase(this, stateMachine, "Chasing");
        chaserIdle = new ShooterIdle(this, stateMachine, "Idle");

        chaserAttack = new ShooterAttack(this, attackStateMachine, "Attacking");
        chaserAttackIdle = new ShooterAttackIdle(this, attackStateMachine, "AttackIdle");

        stateMachine.Initialize(chaserIdle);
        attackStateMachine.Initialize(chaserAttackIdle);

        shooterData = EntityData as ShooterData;
        weaponHandler = GetComponent<WeaponHandler>();

        if (shooterData == null)
        {
            Debug.LogError($"Expected ShooterData, got {EntityData.GetType()} on {gameObject.name}");
            return;
        }

        core = GetComponentInChildren<Core>();
        LoadWeapon(UnityEngine.Random.Range(0, shooterData.useableWeapons.Count));
    }

    public override void Update()
    {
        base.Update();
        LookAtEnemy(weaponHandler.currentWeaponVisual, spriteRenderer, pivotPoint );
        if (weaponHandler.currentWeapon.ammoCount <= 0 && weaponHandler.currentWeapon.maxAmmo > 0)
        {
            weaponHandler.currentWeapon.Reload();
        }
    }

    private void LoadWeapon(int index)
    {
        weaponHandler.Initialize(shooterData.useableWeapons[index], core);
    }

    private void LookAtEnemy(GameObject weaponVisual, SpriteRenderer bodySprite, Transform pivotPoint)
    {
        if (core.GetCoreComponent<Status>()?.IsFrozen ?? false) return;
        if (!PlayerInSight()) return;

        if (weaponVisual == null) return;

        Vector2 direction = ((Vector2)player.position - (Vector2)weaponHoldPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pivotPoint.rotation = Quaternion.Euler(0, 0, angle);

        bool flip = angle > 90 || angle < -90;
        if (weaponVisual != null)
            weaponVisual.transform.localScale = new Vector3(1, flip ? -1 : 1, 1);

        bodySprite.flipX = flip;
    
    }

    protected override void InitializeStates()
    {

    }
    
        


}