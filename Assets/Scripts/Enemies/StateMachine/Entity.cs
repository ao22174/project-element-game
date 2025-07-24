using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Entity : MonoBehaviour, IFreezable
{
    //--- DATA OF ENEMY --- 
    [SerializeField] private EntityData entityData;
    public EntityData EntityData => entityData;

    //--- STATE MACHINES --- 
    public FiniteStateMachine attackStateMachine;
    public FiniteStateMachine stateMachine;

    // --- GLOBAL STATES ---
    public EntityAttackFreezeState attackFrozenState;
    public FreezeState frozenState;

    public State freezeAttackReturnState;
    public State freezeReturnState;
    // --- ENEMY CORE ---
    public Core core;
    public Rigidbody2D rb { get; private set; } = null!;
    public Animator anim { get; private set; } = null!;
    public GameObject aliveGO { get; private set; } = null!;
    public Transform player { get; private set; } = null!;
    [SerializeField] private Seeker seeker;
    public Path path;
    public EnemySpawner enemySpawner;
    [SerializeField] private ElementalStatus elementStatus;

    // --- ENEMY DYNAMIC INFO ---
    public float currentHealth;

    public void InitializeEnemy(EntityData data, EnemySpawner spawner)
    {
        entityData = data;
        enemySpawner = spawner;
    }
    private void Awake()
    {
        // --- ASSIGN STATE MACHINES ---
        stateMachine = new FiniteStateMachine();
        attackStateMachine = new FiniteStateMachine();

        attackFrozenState = new EntityAttackFreezeState(this, attackStateMachine, "frozen");
        frozenState = new FreezeState(this, stateMachine, "frozen");

        // --- ASSIGN CORE ---
        aliveGO = transform.Find("Alive").GameObject();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        elementStatus = GetComponent<ElementalStatus>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        core = GetComponentInChildren<Core>();
    }

    public virtual void Start()
    {
        InitializeStates();
        if (EntityData.usesPathfinding)
            InvokeRepeating("UpdatePath", 0f, 0.5f);
        core.GetCoreComponent<Combat>().OnDeath +=  OnDeath;

    }


    protected abstract void InitializeStates();

    void UpdatePath()
    {
        if (player == null || seeker == null) return;
        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public virtual void OnHit(){}

    public virtual void OnDeath(DamageInfo info)
    {
        GameObject sourceObj = info.core.transform.parent.gameObject;
        Faction faction = info.faction;
        //CHANGE THAT 0f to overflow damage later
        enemySpawner.UpdateAlive(this);
        CombatEvents.EnemyKilled(new EnemyDeathInfo(EntityData, sourceObj, faction, transform.position, GetType().Name, 0f));
        Destroy(gameObject);

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
    }
    public bool PlayerInSight()
    {
        if (player == null) return false;

        float dist = Vector2.Distance(player.position, transform.position);
        if (dist > EntityData.attackRange) return false;

        if (EntityData.usesLineOfSight)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                                                player.position - transform.position,
                                                dist,
                                                EntityData.obstacleMask);
            return hit.collider == null;
        }
        return true;
    }

    public void ApplyFreeze(float duration)
    {
        frozenState.setDuration(duration);
        attackFrozenState.setDuration(duration);

        stateMachine.ChangeState(frozenState);
        attackStateMachine.ChangeState(attackFrozenState);
    }


    public virtual void Update()
    {
        if (stateMachine.currentState != null && attackStateMachine.currentState != null)

        {
            stateMachine.currentState.LogicUpdate();
            attackStateMachine.currentState.LogicUpdate();
        }
    }
    public virtual void FixedUpdate()
    {
        if (stateMachine.currentState != null && attackStateMachine.currentState != null)

        {
            stateMachine.currentState.PhysicsUpdate();
            attackStateMachine.currentState.PhysicsUpdate();
        }


    }


}