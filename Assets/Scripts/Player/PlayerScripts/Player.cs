using UnityEngine;


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

    //Player Controller Components
    [SerializeField] public Animator Anim { get; private set; }
    [SerializeField] public PlayerInputHandler InputHandler { get; private set; }
    [SerializeField] public PlayerData playerData {get; private set; }
    [SerializeField] public AimController aimController { get; private set;   }
    [SerializeField]public WeaponHandler weaponHandler { get; private set; }

    //Potentially replace with List of Weapons, for modularity in weapon amount
    public Core core { get; private set; }
    public WeaponData startingWeapon;
    public Vector2 LastInputDirection => InputHandler.MovementInput.normalized;
    


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
        aimController = GetComponent<AimController>();
        weaponHandler = GetComponent<WeaponHandler>();
    }


    private void Start()
    {
        StateMachine.Initialize(IdleState);
        AttackStateMachine.Initialize(AttackIdleState);
        weaponHandler.Initialize(startingWeapon, core);

    }

    private void Update()
    {
        core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
        AttackStateMachine.CurrentState.LogicUpdate();

        aimController.AimAt(InputHandler.MousePosition, weaponHandler.currentWeaponVisual, GetComponent<SpriteRenderer>());
    }

    void OnEnable()
    {
        CombatEvents.OnEnemyKilled += HandleEnemyKill;
        InputHandler.OnWeaponChange += weaponHandler.EquipWeapon;
    }

    void OnDisable()
    {
        CombatEvents.OnEnemyKilled -= HandleEnemyKill;
        InputHandler.OnWeaponChange -= weaponHandler.EquipWeapon;

    }
    private void HandleEnemyKill(EnemyDeathInfo info)
    {
        if (info.faction == Faction.Player)
        {
            core.GetCoreComponent<Buffs>()?.OnKill(info.killer, info.position);
        }
    }

    

    

    private void FixedUpdate()
    {
        StateMachine.CurrentState?.PhysicsUpdate();
        AttackStateMachine.CurrentState?.PhysicsUpdate();
    }

    



}

