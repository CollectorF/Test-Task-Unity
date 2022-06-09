public abstract class BaseStateChange
{
    public delegate void StateChanged(BaseStateChange change);

    public event StateChanged OnStateChange;

    public virtual BaseState ApplyChange(BaseState targetState)
    {
        OnStateChange?.Invoke(this);

        return targetState;
    }
}