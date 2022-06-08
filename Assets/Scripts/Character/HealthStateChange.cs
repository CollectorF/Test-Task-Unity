public class HealthStateChange : BaseStateChange
{
    protected float healthAdjustment;

    public HealthStateChange(float healthAdjustment)
    {
        this.healthAdjustment = healthAdjustment;
    }

    public override BaseState ApplyChange(BaseState targetState)
    {
        return base.ApplyChange(targetState).Mutate(
            health: targetState.Health + healthAdjustment
        );
    }
}