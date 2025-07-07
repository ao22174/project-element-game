using System.Collections;
using System.Collections.Generic;
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
    public virtual void Start()
    {
        aliveGO = transform.Find("Alive").GameObject();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        stateMachine = new FiniteStateMachine();
        wanderState = new WanderState(this, stateMachine, "isWandering");
        idleState = new IdleState(this, stateMachine, "isWandering");


        stateMachine.Initialize(idleState);
    }

    [HideInInspector] public Vector2 spawnPosition;
    [HideInInspector] public Vector2 wanderDirection;

    private void OnDrawGizmos()
    {
        if (entityData == null) return;

        // Draw detection radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, entityData.attackRange);

        // Draw wander radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPosition != Vector2.zero ? spawnPosition : transform.position, entityData.wanderRadius);

        // Draw LOS ray
        if (player != null && entityData.usesLineOfSight)
        {
            Vector2 dir = player.position - transform.position;
            float dist = dir.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dist, entityData.obstacleMask);
            Gizmos.color = hit.collider == null ? Color.red : Color.gray;
            Gizmos.DrawLine(transform.position, hit.collider == null ? player.position : hit.point);
        }

        // If currently in WanderState, show next movement target
        if (stateMachine != null && stateMachine.currentState is WanderState)
        {
            Gizmos.color = Color.yellow;
            Vector2 destination = (Vector2)transform.position + wanderDirection.normalized * 1.5f;
            Gizmos.DrawLine(transform.position, destination);
            Gizmos.DrawSphere(destination, 0.1f);
        }
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    

}