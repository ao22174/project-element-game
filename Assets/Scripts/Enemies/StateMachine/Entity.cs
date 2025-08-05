using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
#pragma warning disable CS8618


public abstract class Entity : MonoBehaviour
{
    //--- DATA OF ENEMY --- 
    [SerializeField] private EntityData entityData;
    public EntityData EntityData => entityData;

    //--- STATE MACHINES --- 
    public FiniteStateMachine attackStateMachine;
    public FiniteStateMachine stateMachine;

    // --- ENEMY CORE ---
    public Core core;
    public Rigidbody2D rb { get; private set; } = null!;
    public Animator anim { get; private set; } = null!;
    public GameObject aliveGO { get; private set; } = null!;
    public Transform player { get; private set; } = null!;
    [SerializeField] private Seeker seeker;
    public Path path;
    public EnemySpawner enemySpawner;
    public bool canRotate = false;

    // --- ENEMY DYNAMIC INFO ---

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

        // --- ASSIGN CORE ---
        aliveGO = transform.Find("Alive").GameObject();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        core = GetComponentInChildren<Core>();
    }

    public virtual void Start()
    {
        InitializeStates();
        if (EntityData.usesPathfinding)
            InvokeRepeating("UpdatePath", 0f, 0.5f);
        core.GetCoreComponent<Combat>().OnDeath += OnDeath;
        core.GetCoreComponent<Combat>().OnTakeDamage += OnHit;

    }


    protected abstract void InitializeStates();

    void UpdatePath()
    {
        if (player == null || seeker == null) return;
        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public virtual void OnHit(DamageInfo info)
    {
        info.sourceCore.GetCoreComponent<Buffs>().OnHitEnemy(info, gameObject);
    }

    public virtual void OnDeath(DamageInfo info)
    {
        GameObject sourceObj = info.sourceCore.transform.parent.gameObject;
        Faction faction = info.faction;
        //CHANGE THAT 0f to overflow damage later
        enemySpawner?.UpdateAlive(this);
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
        if (player == null ) return false;

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



    public virtual void Update()
    {
        if (core.GetCoreComponent<Status>()?.IsFrozen ?? false) return;
        if (stateMachine.currentState != null)
        {
            core.LogicUpdate();
            stateMachine.currentState.LogicUpdate();
        }
        if (attackStateMachine.currentState != null)
        {
            attackStateMachine.currentState.LogicUpdate();
        }
        if (PlayerInSight() && canRotate)
        {
            lookAtPlayer();    
        }
    }

    public void lookAtPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.position.x <= transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public virtual void FixedUpdate()
    {
        if (core.GetCoreComponent<Status>()?.IsFrozen ?? false) return;
        if (stateMachine.currentState != null)
        {
            core.LogicUpdate();
            stateMachine.currentState.LogicUpdate();
        }
        if (attackStateMachine.currentState != null)
        {
            attackStateMachine.currentState.LogicUpdate();
        }


    }


}