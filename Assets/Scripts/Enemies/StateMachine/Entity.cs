using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityData entityData;
    public FiniteStateMachine stateMachine;
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public Transform player { get; private set; }
    public WanderState wanderState;
    public IdleState idleState;
    public Seeker seeker;
    public Path path;
    public RoomInstance currentRoom;

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
        seeker = GetComponent<Seeker>();
        currentHealth = entityData.healthPoints;

        stateMachine.Initialize(idleState);
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
        rb.AddForce(direction.normalized * 20f);
        if (currentHealth <= 0)
        {
            Debug.Log("I DIE");
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

    [HideInInspector] public Vector2 spawnPosition;
    [HideInInspector] public Vector2 wanderDirection;


    public virtual void Update()
    {
        
        stateMachine.currentState.LogicUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    

}