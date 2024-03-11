using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        Walking,
        Spraying
    }

    public State currentState = State.Walking;

    // Call this method to change the player's state
    public void ChangeState(State newState)
    {
        currentState = newState;
    }
}

