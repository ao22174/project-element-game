using UnityEngine;
using System.Collections.Generic;
#pragma warning disable CS8618
public class InteractionManager : MonoBehaviour
{

    private List<IInteractable> interactableInRange = new List<IInteractable>();
    [SerializeField] private PlayerInputHandler inputHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            interactableInRange.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactableInRange.Contains(interactable))
            interactableInRange.Remove(interactable);
    }

    private void Update()
    {

        if (inputHandler.InteractInput && interactableInRange.Count > 0) interactableInRange[0].Interact(GetComponent<Player>());
    }
    


}