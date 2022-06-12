using UnityEngine;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;

public class EnemyController : BaseCharacterController
{
    [SerializeField]
    private BehaviorTree behaviorTree;
    public BaseState State => stats.State;
    internal bool isDead = false;
    internal bool isKnockedOut = false;

    protected override void Awake()
    {
        base.Awake();
        OnDie += controller => isDead = true;
        OnKnockOut += controller => isKnockedOut = true;
    }

    protected override void Start()
    {
        base.Start();
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Selector()

                .Sequence("Move towards player")
                    .Condition("If not dead", () =>
                    {
                        return !isDead && !isKnockedOut;
                    })
                    .FollowPlayer(gameManager.Player.transform, agent, 1, this)
                .End()

                .Sequence("Lie down")
                    .Condition("If is injured", () =>
                    {
                        return !isDead && isKnockedOut;
                    })
                    .Do("Lie down", () =>
                    {
                        SetAgentEnabledState(false);
                        return TaskStatus.Success;
                    })
                    .WaitTime(starterInfo.delayOnInjury)
                    .Do("Get up again", () =>
                    {
                        SetAgentEnabledState(true);
                        isKnockedOut = false;
                        return TaskStatus.Success;
                    })
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

    private void SetAgentEnabledState(bool state)
    {
        agent.enabled = state;
    }
}