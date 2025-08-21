using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class BuffChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private BuffSelectionUI buffSelectionUI;
    private bool isOpened;

    public PlayerInputHandler inputHandler;

    void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player != null) inputHandler = player.InputHandler;
        isOpened = false;

        buffSelectionUI = GameObject.FindGameObjectWithTag("BuffManager").GetComponent<BuffSelectionUI>();
    }

    public void Interact(Player player)
    {
        if (isOpened) return;
        OpenChest();
        isOpened = true;
    }


    private void OpenChest()
    {
        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);

        // Pass a callback to ShowBuffs that destroys the chest when buff is chosen
        buffSelectionUI.ShowBuffs(() =>
        {
            animator?.SetTrigger("Open");
        });
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}