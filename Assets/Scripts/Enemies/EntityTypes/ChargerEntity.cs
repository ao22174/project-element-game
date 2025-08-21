using UnityEngine;
public class ChargerEntity : Entity
{
    public ChargerData? chargerData;
    public ChargerCharge chargeState;
    public ChargerChase chaseState;
    public ChargerIdle idleState;
    public ChargerWindup windupState;
    public ChargerRest restState;
    public ChargerAttackIdle attackIdleState;
    public Collider2D hurtbox;


    public override void Start()
    {
        base.Start();
        attackIdleState = new ChargerAttackIdle(this, attackStateMachine, "attackIdle");
        chargeState = new ChargerCharge(this, stateMachine, "charge");
        chaseState = new ChargerChase(this, stateMachine, "chase");
        idleState = new ChargerIdle(this, stateMachine, "idle");
        windupState = new ChargerWindup(this, stateMachine, "windup");
        restState = new ChargerRest(this, stateMachine, "rest");


        stateMachine.Initialize(idleState);
        attackStateMachine.Initialize(attackIdleState);
        chargerData = EntityData as ChargerData;
        if (chargerData == null)
        {
            Debug.LogError($"Expected ShooterData, got {EntityData.GetType()} on {gameObject.name}");
            return;
        }
        core = GetComponentInChildren<Core>();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void InitializeStates()
    {
    }
    
}