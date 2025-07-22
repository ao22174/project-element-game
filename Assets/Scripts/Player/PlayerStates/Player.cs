using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementProject;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using ElementProject.gameEnums;

public class Player : MonoBehaviour, IWeaponUser
{
    //STATE MACHINE LOGIC
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerStateMachine AttackStateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerAttackIdleState AttackIdleState { get; private set; }

    //ANIMATOR AND INPUT LOGIC
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    private PickupManager pickup;

    [Serialize] public PlayerData playerData;
    public Rigidbody2D RB { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 LastInputDirection { get; set; }


    //Potentially replace with List of Weapons, for modularity in weapon amount
    public List<Weapon> weapons = new();
    public Weapon currentWeapon;
    public int currentWeaponIndex;

    //WeaponVisuals
    [SerializeField] public Transform weaponHoldPoint;
    private GameObject currentWeaponVisual;
    public float currentHealth;
    public bool isInvincible;
    public HeartDisplay heartDisplay;
    public WeaponData startingWeapon;

    public PlayerBuffs buffs;
    public PlayerStatModifiers stats;

    public GameObject rightHandTransform;
    public GameObject leftHandTransform;

    public Transform leftHandAnchor;
    public Transform rightHandAnchor;




    private void Start()
    {

        // SET STATE MACHINES
        StateMachine = new PlayerStateMachine();
        AttackStateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dash");

        AttackState = new PlayerAttackState(this, AttackStateMachine, playerData, "attack");
        AttackIdleState = new PlayerAttackIdleState(this, AttackStateMachine, playerData, "attackIdle");

        buffs = new PlayerBuffs(this, gameObject);
        stats = new PlayerStatModifiers();

        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        pickup = GetComponent<PickupManager>();
        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);
        AttackStateMachine.Initialize(AttackIdleState);
        FacingDirection = 1;
        currentHealth = stats.MaxHealth;
        heartDisplay.SetHearts(currentHealth, stats.MaxHealth);
        weapons.Add(WeaponFactory.CreateWeapon(startingWeapon, this));
        EquipWeapon(currentWeaponIndex);

    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        AttackStateMachine.CurrentState.LogicUpdate();
        CheckInteractIsPressed();
        if (currentWeapon != null) RotateWeaponTowardMouse(InputHandler.MousePosition);
        FlipTowardsMouse(InputHandler.MousePosition);
    }
    private void CheckInteractIsPressed()
    {
        if (InputHandler.InteractInput)
        {
            if (pickup.nearbyWeapons.Count > 0)
            {
                WeaponPickup weaponPickup = pickup.nearbyWeapons[0];
                if (weapons.Count >= 2) Drop(weapons[currentWeaponIndex], this.transform.position);
                weaponPickup.PickupWeapon(this);
            }
        }
    }
    void OnEnable()
    {
        CombatEvents.OnEnemyKilled += HandleEnemyKill;
    }

    void OnDisable()
    {
        CombatEvents.OnEnemyKilled -= HandleEnemyKill;
    }
    private void HandleEnemyKill(EnemyDeathInfo info)
    {
        Debug.Log("kill");
        if (info.source == OwnedBy.Player)
        {
            Debug.Log("by you");
            buffs.OnKill(gameObject, info.position);
        }
    }

    public void Drop(Weapon weapon, Vector2 dropPosition)
    {
        WeaponPickupFactory.Create(weapon, dropPosition);
    }



    public void HealthModify(float health)
    {
        currentHealth += health;
        heartDisplay.SetHearts(currentHealth, stats.MaxHealth);
        ActivateIFrames(0.5f);
    }
    public void ActivateIFrames(float duration)
    {
        if (isInvincible) return;
        StartCoroutine(IFrameCoroutine(duration));
    }

    private IEnumerator IFrameCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private void FlipTowardsMouse(Vector2 mousePosition)
    {

        if (mousePosition.x < transform.position.x && FacingDirection == 1)
        {
            Flip();
        }
        else if (mousePosition.x > transform.position.x && FacingDirection == -1)
        {
            Flip();
        }
    }

    public void EquipWeapon(int index)
    {
        currentWeaponIndex = index;
        currentWeapon = weapons[index];

        if (currentWeaponVisual != null)
            Destroy(currentWeaponVisual);

        if (currentWeapon.weaponPrefab != null)
        {
            currentWeaponVisual = Instantiate(currentWeapon.weaponPrefab, weaponHoldPoint);
            leftHandAnchor = currentWeaponVisual.transform.Find("LeftHandAnchor");
            rightHandAnchor = currentWeaponVisual.transform.Find("RightHandAnchor");
            if (leftHandAnchor != null && rightHandAnchor != null)
            {
                leftHandTransform.transform.SetParent(leftHandAnchor, false);
                rightHandTransform.transform.SetParent(rightHandAnchor, false);
            }
            else Debug.LogWarning("Missing hand anchors on weapon prefab: " + currentWeapon.Weaponname);


        }
        else Debug.LogWarning("Weapon prefab missing for: " + currentWeapon.Weaponname);

    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started && weapons.Count >= 1)
        {
            EquipWeapon(0);
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started && weapons.Count >= 2)
        {
            EquipWeapon(1);
        }
    }

    private void RotateWeaponTowardMouse(Vector2 mousePos)
    {
        if (currentWeaponVisual == null) return;

        Vector2 direction = (mousePos - (Vector2)weaponHoldPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the weapon holder (not the visual)
        weaponHoldPoint.rotation = Quaternion.Euler(0, 0, angle);

        // Flip weapon visual's local scale on Y to mirror the sprite only
        bool flip = (angle > 90 || angle < -90);
        currentWeaponVisual.transform.localScale = new Vector3(1, flip ? -1 : 1, 1);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState?.PhysicsUpdate();
        AttackStateMachine.CurrentState?.PhysicsUpdate();

    }
    public void SetVelocity(Vector2 velocity)
    {
        RB.MovePosition(RB.position + velocity * Time.fixedDeltaTime);
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public Transform GetFirePoint()
    {
        if (currentWeaponVisual == null) throw new NullReferenceException("fireOrigin is null");
        return currentWeaponVisual.transform.Find("fireOrigin");
    }


}

