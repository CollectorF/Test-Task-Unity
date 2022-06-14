using UnityEngine;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;


public class EnemyController : BaseCharacterController
{
    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private Rigidbody GetupForcePoint;


    public BaseState State => stats.State;
    private Animator animator;
    internal Canvas targetCanvas;
    internal bool isDead = false;
    internal bool isKnockedOut = false;
    private bool firstShot = false;

    public void SetFirstShotState(bool state)
    {
        firstShot = state;
    }

    protected override void Awake()
    {
        base.Awake();
        OnDie += controller => isDead = true;
        OnKnockOut += controller => isKnockedOut = true;
        animator = GetComponent<Animator>();
        targetCanvas = GetComponentInChildren<Canvas>();
        SetRagdollParts();
    }

    protected override void Start()
    {
        base.Start();
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .Selector()

                .Sequence("Wait for first shot")
                    .Condition("If there is no first shot", () =>
                        {
                            return !firstShot;
                        })
                    .Wait(1)
                .End()

                .Sequence("Move towards player")
                    .Condition("If not dead", () =>
                    {
                        return !isDead && !isKnockedOut;
                    })
                    .FollowPlayer(gameManager.Player.transform, agent, 1.5f, this)
                .End()

                .Sequence("Lie down")
                    .Condition("If is injured", () =>
                    {
                        return !isDead && isKnockedOut;
                    })
                    .Do("Lie down", () =>
                    {
                        SetRagdollCondition(true);
                        return TaskStatus.Success;
                    })
                    .WaitTime(starterInfo.delayOnInjury)
                    .Do("Add impulse to get up", () =>
                    {
                        AddImpulse(500);
                        return TaskStatus.Success;
                    })
                    .WaitTime(1f)
                    .Do("Get up again", () =>
                    { 
                        SetRagdollCondition(false);
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
            animator.SetFloat("Speed", agent.velocity.magnitude, 0.05f, Time.deltaTime);
        }
        else
        {
            SetRagdollCondition(true);
            agent.enabled = false;
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

    private void SetRagdollParts()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var item in colliders)
        {
            if (item.gameObject != gameObject)
            {
                item.isTrigger = false;
            }
        }
    }

    private void SetRagdollCondition(bool state)
    {
        animator.enabled = !state;
        agent.enabled = !state;
    }

    private void AddImpulse(float impulse)
    {
        GetupForcePoint.AddForce(Vector3.up * impulse, ForceMode.Impulse);
    }
}