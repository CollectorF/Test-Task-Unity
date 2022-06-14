using System.Collections.Generic;
using UnityEngine;


public class StatsSystem : MonoBehaviour
{
    public delegate void StateChanged(BaseState oldState, BaseState newState);

    public event StateChanged OnStateChanged;

    private BaseState state;

    public BaseState State
    {
        get => state;
        set
        {
            OnStateChanged?.Invoke(State, value);
            state = value;
        }
    }

    public void ApplyStateChange(BaseStateChange change)
    {
        State = change.ApplyChange(State);
    }
}