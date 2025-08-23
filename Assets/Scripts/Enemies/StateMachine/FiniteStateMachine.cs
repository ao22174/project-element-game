using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS8618

public class FiniteStateMachine
{
    public State currentState { get; private set; }
    public void Initialize(State startingState)
    {
        Debug.Log(currentState);
        currentState = startingState;
                Debug.Log(currentState);

        currentState.Enter();
    }
    public void ChangeState(State state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}