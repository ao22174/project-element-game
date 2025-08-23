using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; private set; }

    public bool CanSetVelocity { get; set; }

    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 workspace;
        public bool canKnockback;


    private Status status;
    private Status Status { get => status ?? core.GetCoreComponent(ref status); }


    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();

        FacingDirection = 1;
        CanSetVelocity = true;
    }
    public override void Init(CoreData coreData)
    {
        canKnockback = coreData.canKnockback;
        base.Init(coreData);
    }

    public override void LogicUpdate()
    {
        if (Status.IsFrozen)
        {
            SetVelocityZero();
            return;
        }
        CurrentVelocity = RB.linearVelocity;
    }

    #region Set Functions

    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }



    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void Knockback(float force, Vector2 direction, float duration = 0.2f)
    {
        StartCoroutine(ApplyKnockback(force, direction, duration));
    }

    private IEnumerator ApplyKnockback(float force, Vector2 direction, float duration)
    {
        CanSetVelocity = false; // prevent movement code from overwriting
        RB.linearVelocity = Vector2.zero; // reset to make knockback consistent
        RB.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);
        if (canKnockback)
        {
            CanSetVelocity = true;
            SetVelocityZero();
        }
    }

    private void SetFinalVelocity()
    {
        if (CanSetVelocity)
        {
            RB.linearVelocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    #endregion
}