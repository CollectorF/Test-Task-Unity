public class SpeedStateChange : BaseStateChange
{
    protected float speedAdjustment;

    public SpeedStateChange(float speedAdjustment)
    {
        this.speedAdjustment = speedAdjustment;
    }

    public override BaseState ApplyChange(BaseState targetState)
    {
        return base.ApplyChange(targetState).Mutate(
            speed: targetState.Speed + speedAdjustment
        );
    }
}