using UnityEngine;


public class Door : MonoBehaviour
{
    public Collider2D doorCollider;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private bool isOpening;

    void Awake()
    {
        Open();
    }

    public void Close()
    {
        isOpening = false;
        spriteRenderer.enabled = true;
            doorCollider.enabled = true;
        animator.Play("close");
        // Collider will be enabled at end of animation via event
    }

    public void Open()
    {
        isOpening = true;
        

        animator.Play("open");
        // Collider will be disabled at end of animation via event
    }

    // Call this from Animation Event at end of "open" or "close"
    public void OnDoorAnimationFinished()
    {
        if (isOpening)
        {
            doorCollider.enabled = false;
        spriteRenderer.enabled = false;
        }
    }
}