using UnityEngine;


    public class PlayerStateMachine
    {
    public PlayerState CurrentState { get; private set; } = null!;

        public void Initialize(PlayerState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }
        public void ChangeState(PlayerState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    
    }

