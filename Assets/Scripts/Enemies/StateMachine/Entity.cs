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

    public virtual EntityAttackIdleState GetAttackIdleState() => null;


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
        Debug.Log("updating");
        if (player == null || seeker == null) return;
        Debug.Log("attemping");
        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public virtual void HitBullet(float damage, Vector2 direction)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            enemySpawner.UpdateAlive(this);
            Destroy(this.gameObject);
        }
    }

    private void OnPathComplete(Path p)
    {
        Debug.Log("gets here");
        if (!p.error)
        {
            Debug.Log("woor");
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

        Debug.Log("Freezing for: " + duration + " seconds");
        stateMachine.ChangeState(frozenState);
        attackStateMachine.ChangeState(attackFrozenState);
    }


    public virtual void Update()
    {
        if (stateMachine.currentState != null && attackStateMachine.currentState != null)

        {
            Debug.Log("updatei");
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