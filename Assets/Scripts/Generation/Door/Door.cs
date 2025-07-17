using UnityEngine;


public class Door : MonoBehaviour
{
    public Collider2D doorCollider;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        Open();
    }

    public void Close()
    {
        spriteRenderer.enabled = true;
        doorCollider.enabled = true;
    }
    public void Open()
    {
        spriteRenderer.enabled = false;
        doorCollider.enabled = false;
    }
}