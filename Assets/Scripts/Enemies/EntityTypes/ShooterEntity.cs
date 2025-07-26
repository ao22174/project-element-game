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

    public ShooterData shooterData;
    public Transform weaponHoldPoint;
    public GameObject weaponDisplay;
    public Weapon weapon;

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
        LookAtEnemy();
        if (weapon.ammoCount <= 0 && weapon.maxAmmo > 0)
        {
            weapon.Reload();
        }
    }

    private void LoadWeapon(int index)
    {
        weapon = WeaponFactory.CreateWeapon(shooterData.useableWeapons[index], core);
        weaponDisplay = Instantiate(weapon.weaponPrefab, weaponHoldPoint);
    }

    private void LookAtEnemy()
    {
        if (core.GetCoreComponent<Status>()?.IsFrozen ?? false) return;
        if (!PlayerInSight()) return;


        transform.localScale = new Vector3((player.position.x < transform.position.x) ? -1 : 1, 1, 1);


        if (weaponDisplay == null) return;

        Vector2 direction = ((Vector2)player.position - (Vector2)weaponHoldPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weaponHoldPoint.rotation = Quaternion.Euler(0, 0, angle);

        bool flip = angle > 90 || angle < -90;
        weaponDisplay.transform.localScale = new Vector3(flip ? -1 : 1, flip ? -1 : 1, 1);
    }
    protected override void InitializeStates()
    {

    }
    
        public Transform GetFirePoint()
    {
        if (weaponDisplay == null) throw new NullReferenceException("fireOrigin is null");
        return weaponDisplay.transform.Find("fireOrigin");
    }


}