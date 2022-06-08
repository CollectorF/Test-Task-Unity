public class PlayerController : BaseCharacterController
{
    public PlayerState State => (PlayerState)stats.State;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override BaseState InitializeState()
    {
        return new PlayerState(starterInfo);
    }
}