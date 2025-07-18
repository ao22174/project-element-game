using System.Collections;
using System.Collections.Generic;
using ElementProject;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //Data reference
    public EntityData entityData;
    //StateMachine Reference
    public FiniteStateMachine attackStateMachine;
    public EntityAttackIdleState attackIdleState;
    public EntityAttackState attackState;
    public FiniteStateMachine stateMachine;
    public StationaryState stationaryState;

    public EntityAttackFreezeState attackFrozenState;
    public FreezeState frozenState;
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public Transform player { get; private set; }
    public WanderState wanderState;
    public IdleState idleState;
    public Seeker seeker;
    public Path path;
    public EnemySpawner enemySpawner;
    public float currentHealth;


    public virtual void Start()
    {
        aliveGO = transform.Find("Alive").GameObject();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        stateMachine = new FiniteStateMachine();
        wanderState = new WanderState(this, stateMachine, "isWandering");
        idleState = new IdleState(this, stateMachine, "isWandering");

        attackStateMachine = new FiniteStateMachine();
        attackIdleState = new EntityAttackIdleState(this, attackStateMachine, "attackIdling");
        attackState = new EntityAttackState(this, attackStateMachine, "attacking");
        stationaryState = new StationaryState(this, stateMachine, "Stationary");
        attackFrozenState = new EntityAttackFreezeState(this, attackStateMachine, "frozen");
        frozenState = new FreezeState(this, stateMachine, "frozen");
        

        seeker = GetComponent<Seeker>();
        currentHealth = entityData.healthPoints;

        stateMachine.Initialize(idleState);
        attackStateMachine.Initialize(attackIdleState);
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (player == null || seeker == null) return;

        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public void HitBullet(float damage, Vector2 direction)
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

        Debug.Log("Freezing for: " + duration + " seconds");
        stateMachine.ChangeState(frozenState);
        attackStateMachine.ChangeState(attackFrozenState);
    }


    public virtual void Update()
    {

        stateMachine.currentState.LogicUpdate();
        attackStateMachine.currentState.LogicUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
                attackStateMachine.currentState.PhysicsUpdate();

    }
    

}