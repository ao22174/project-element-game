using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Entity : MonoBehaviour
{
    //--- DATA OF ENEMY --- 
    public EntityData entityData;
    //--- STATE MACHINES --- 
    public FiniteStateMachine attackStateMachine;
    public FiniteStateMachine stateMachine;

    // --- GLOBAL STATES ---
    public EntityAttackFreezeState attackFrozenState;
    public FreezeState frozenState;

    public State freezeAttackReturnState;
    public State freezeReturnState;
    // --- ENEMY CORE ---
    public Rigidbody2D rb { get; private set; } = null!;
    public Animator anim { get; private set; } = null!;
    public GameObject aliveGO { get; private set; } = null!;
    public Transform player { get; private set; } = null!;
    public Seeker seeker;
    public Path path;
    public EnemySpawner enemySpawner;
    
    // --- ENEMY DYNAMIC INFO ---
    public float currentHealth;


    public virtual void Start()
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();

        currentHealth = entityData.healthPoints;

        InitializeStates();
        if (entityData.usesPathfinding)
            InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    protected abstract void InitializeStates();

    void UpdatePath()
    {
        if (player == null || seeker == null) return;
        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public virtual void Hit(float damage, GameObject sourceObj, OwnedBy source)
    {
        currentHealth -= damage;
        if (source == OwnedBy.Player) OnHit();
        if (currentHealth <= 0)
        {
            enemySpawner.UpdateAlive(this);
            onDeath(sourceObj, source);
            Destroy(this.gameObject);

        }
    }

    public virtual void OnHit()
    {

    }

    public virtual void onDeath(GameObject sourceObj, OwnedBy source)
    {
        CombatEvents.EnemyKilled(new EnemyDeathInfo(entityData, sourceObj, source, transform.position, type: this.GetType().Name, -currentHealth));
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
        if (dist > entityData.attackRange) return false;

        if (entityData.usesLineOfSight)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                                                player.position - transform.position,
                                                dist,
                                                entityData.obstacleMask);
            return hit.collider == null;
        }
        return true;
    }

    public void EnterFreezeState(float duration)
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