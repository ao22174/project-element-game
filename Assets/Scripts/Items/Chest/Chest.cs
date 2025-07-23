using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class BuffChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private BuffSelectionUI buffSelectionUI;

    public PlayerInputHandler inputHandler;
    private bool playerNotAssigned;

    private bool isOpened = false;
    private bool playerInRange = false;

    void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player != null) inputHandler = player.InputHandler;
        else playerNotAssigned = true;

        buffSelectionUI = GameObject.FindGameObjectWithTag("BuffManager").GetComponent<BuffSelectionUI>();
    }

    public void Interact(Player player)
    {
        OpenChest();
    }


    private void OpenChest()
    {
        isOpened = true;
        Debug.Log("Opening");

        animator?.SetTrigger("Open");

        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);


        buffSelectionUI.ShowBuffs(() =>
        {
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