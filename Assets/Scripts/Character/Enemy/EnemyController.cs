using UnityEngine;
using CleverCrow.Fluid.BTs.Trees;


public class EnemyController : BaseCharacterController
{
    [SerializeField]
    private BehaviorTree behaviorTree;
    public BaseState State => stats.State;
    private bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
        OnDie += controller => isDead = true;
    }

    protected override void Start()
    {
        base.Start();
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Sequence("Move towards player")
                .Condition("If not dead", () => !isDead)
                    .FollowPlayer(gameManager.Player.transform, agent, 1)
            .End()
        .Build();
    }

    protected void Update()
    {
        if (!isDead)
        {
            behaviorTree.Tick();
        }
    }

    protected override void OnStateUpdate(BaseState oldState, BaseState newState)
    {
        base.OnStateUpdate(oldState, newState);
        if (oldState == null || oldState.Speed != newState.Speed)
        {
            agent.speed = newState.Speed * 0.33f;
        }
    }
}