using UnityEngine;

public class BossSpit : State
{
    private Boss boss;

    private enum Phase { Telegraph, Attack, Cooldown }
    private Phase phase;
    private float phaseTimer;

    private int wavesFired;
    private bool offsetPattern;

    // Tunables
    private float telegraphDuration = 1.0f;
    private float intervalBetweenWaves = 1.0f;
    private int totalWaves = 4;
    private int projectileCount = 12;
    private float projectileSpeed = 2f;
    private float cooldownDuration = 1.0f;

    public BossSpit(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        boss = entity as Boss;
    }

    public override void Enter()
    {
        base.Enter();

        // Start with telegraph
        phase = Phase.Telegraph;
        phaseTimer = telegraphDuration;
        wavesFired = 0;
        offsetPattern = false;

        boss.anim.Play("Telegraph"); // optional telegraph anim
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        phaseTimer -= Time.deltaTime;

        switch (phase)
        {
            case Phase.Telegraph:
                boss.sprite.color = Color.darkRed;

                if (phaseTimer <= 0f)
                {
                    phase = Phase.Attack;
                    phaseTimer = 0f; // trigger attack immediately
                }
                break;

            case Phase.Attack:
                if (phaseTimer <= 0f && wavesFired < totalWaves)
                {
                                    boss.sprite.color = Color.blue;

                    FireCircle(offsetPattern);
                    offsetPattern = !offsetPattern;
                    wavesFired++;
                    phaseTimer = intervalBetweenWaves;
                }
                else if (wavesFired >= totalWaves)
                {
                    phase = Phase.Cooldown;
                    phaseTimer = cooldownDuration;
                }
                break;

            case Phase.Cooldown:
                if (phaseTimer <= 0f)
                {
                    stateMachine.ChangeState(boss.idleState); // go back to wander/idle
                }
                break;
        }
    }

    private void FireCircle(bool offset)
    {
        float angleStep = 360f / projectileCount;
        float startAngle = offset ? angleStep / 2f : 0f;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Spawn projectile
            GameObject projectileGO = GameObject.Instantiate(boss.projectilePrefab, boss.transform.position, Quaternion.identity);
            ProjectileBullet bullet = projectileGO.GetComponent<ProjectileBullet>();
            CombatStats combatStats  = boss.core.GetCoreComponent<Stats>().GetCombatStats(ElementProject.gameEnums.ElementType.Water);

            if (bullet != null)
                bullet.Initialize(new BulletInfo(boss.core, combatStats, boss.transform.position, dir, projectileSpeed, 0.5f, 10f, 50, ElementProject.gameEnums.ElementType.Water, boss.core.Faction));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
