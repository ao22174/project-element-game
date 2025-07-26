using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementProject;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using ElementProject.gameEnums;

public class Player : MonoBehaviour
{
    //STATE MACHINE LOGIC
    public PlayerStateMachine StateMachine { get; private set; } = null!;
    public PlayerStateMachine AttackStateMachine { get; private set; } = null!;
    public PlayerIdleState IdleState { get; private set; } = null!;
    public PlayerMoveState MoveState { get; private set; } = null!;
    public PlayerDashState DashState { get; private set; } = null!;
    public PlayerAttackState AttackState { get; private set; } = null!;
    public PlayerAttackIdleState AttackIdleState { get; private set; } = null!;

    //ANIMATOR AND INPUT LOGIC
     [SerializeField]public Animator Anim { get; private set; }
     [SerializeField]public PlayerInputHandler InputHandler { get; private set; }
    [SerializeField] public PlayerData playerData;
    public int FacingDirection { get; private set; }
    public Vector2 LastInputDirection { get; set; }


    //Potentially replace with List of Weapons, for modularity in weapon amount
    public List<Weapon> weapons = new();
    public Weapon currentWeapon;
    public int currentWeaponIndex;

    //WeaponVisuals
    [SerializeField] public Transform weaponHoldPoint;
    private GameObject currentWeaponVisual;
    public Core core { get; private set; }
    public WeaponData startingWeapon;
    public GameObject rightHandTransform;
    public GameObject leftHandTransform;

    public Transform leftHandAnchor;
    public Transform rightHandAnchor;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        AttackStateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dash");

        AttackState = new PlayerAttackState(this, AttackStateMachine, playerData, "attack");
        AttackIdleState = new PlayerAttackIdleState(this, AttackStateMachine, playerData, "attackIdle");
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        core = GetComponentInChildren<Core>();
    }


    private void Start()
    {
        StateMachine.Initialize(IdleState);
        AttackStateMachine.Initialize(AttackIdleState);
        FacingDirection = 1;
        weapons.Add(WeaponFactory.CreateWeapon(startingWeapon, core));
        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
        AttackStateMachine.CurrentState.LogicUpdate();


        if (currentWeapon != null) RotateWeaponTowardMouse(InputHandler.MousePosition);
        FlipTowardsMouse(InputHandler.MousePosition);
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
        if (info.faction == Faction.Player)
        {
            Debug.Log("by you");
           core.GetCoreComponent<Buffs>()?.OnKill(info.killer, info.position);
        }
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
        core.GetCoreComponent<UIManager>()?.weaponDisplay.SetWeapon(currentWeapon);

        if (currentWeaponVisual != null)
            Destroy(currentWeaponVisual);

        if (currentWeapon.weaponPrefab != null)
        {
            currentWeaponVisual = Instantiate(currentWeapon.weaponPrefab, weaponHoldPoint);
                currentWeapon.SetInstance(currentWeaponVisual);
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
        bool flip = angle > 90 || angle < -90;
        currentWeaponVisual.transform.localScale = new Vector3(1, flip ? -1 : 1, 1);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState?.PhysicsUpdate();
        AttackStateMachine.CurrentState?.PhysicsUpdate();
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

