using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public event Action<int>? OnWeaponChange;
    public Vector2 MovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public bool DashInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool ReloadInput { get; private set; }
    private InputAction interact;
    private InputAction dash;
    private InputAction reload;
    void Start()
    {
        interact = InputSystem.actions.FindAction("Interact");
        dash = InputSystem.actions.FindAction("Dash");
        reload = InputSystem.actions.FindAction("Reload");
    }
    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        InteractInput = interact.WasPressedThisFrame();
        DashInput = dash.WasPressedThisFrame();
        ReloadInput = reload.WasPressedThisFrame();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        FireInput = context.ReadValueAsButton(); // Works for hold and release
    }
    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnWeaponChange?.Invoke(0);
        }
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnWeaponChange?.Invoke(1);
        }
    }



}