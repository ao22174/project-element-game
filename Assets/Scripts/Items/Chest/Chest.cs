using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class BuffChest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private BuffSelectionUI buffSelectionUI;

    public PlayerInputHandler inputHandler;

    private bool isOpened = false;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && !isOpened && inputHandler.InteractInput)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        animator?.SetTrigger("Open");

        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);

        buffSelectionUI.ShowBuffs(() => {
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}