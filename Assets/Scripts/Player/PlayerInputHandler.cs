using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public bool DashInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool InteractInput { get; private set; }

    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            DashInput = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        FireInput = context.ReadValueAsButton(); // Works for hold and release
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractInput = true;
        }
    }

    // These are called by gameplay systems after processing the input:
    public void UseDashInput() => DashInput = false;
    public void UseInteractInput() => InteractInput = false;
}