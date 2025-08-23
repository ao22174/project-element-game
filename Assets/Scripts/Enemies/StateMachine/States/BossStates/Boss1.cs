using UnityEngine;
public class Boss : Entity
{
    public GameObject projectilePrefab;
    public BossSpit bossSpitState;
    public BossIdle idleState;
    public BossLeap bossLeap;
    public BossData? bossData;
    public SpriteRenderer sprite;
    public Collider2D jumpHitbox;
    protected override void InitializeStates()
    {
    }

    public override void Start()
    {
        base.Start();

        bossSpitState = new BossSpit(this, stateMachine, "Spitting");
        idleState = new BossIdle(this, stateMachine, "idle");
        bossLeap  = new BossLeap(this, stateMachine, "leaping");
        bossData = EntityData as BossData;
        stateMachine.Initialize(idleState);
        
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void OnHit(DamageInfo info)
    {
        base.OnHit(info);
    }
    public override void OnDeath(DamageInfo info)
    {
        base.OnDeath(info);
    }
    public override void Awake()
    {
        base.Awake();
    }

}
