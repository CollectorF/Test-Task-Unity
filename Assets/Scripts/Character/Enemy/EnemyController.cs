using UnityEngine;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : BaseCharacterController
{
    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private Rigidbody GetupForcePoint;


    public BaseState State => stats.State;
    private Animator animator;
    internal bool isDead = false;
    internal bool isKnockedOut = false;
    private List<Collider> ragdollColliders;
    private BoxCollider mainCollider;

    protected override void Awake()
    {
        base.Awake();
        OnDie += controller => isDead = true;
        OnKnockOut += controller => isKnockedOut = true;
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<BoxCollider>();
        SetRagdollParts();
    }

    protected override void Start()
    {
        base.Start();
        behaviorTree = new BehaviorTreeBuilder(gameObject)
            .WaitTime(2)
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
                        SetRagdollState(true);
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
                        SetRagdollState(false);
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
            SetRagdollState(true);
        }
    }

    private void FixedUpdate()
    {
        Debug.Log($"Velocity magnitude: {agent.velocity.magnitude}");
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
        ragdollColliders = new List<Collider>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var item in colliders)
        {
            if (item.gameObject != gameObject)
            {
                item.isTrigger = true;
                ragdollColliders.Add(item);
            }
        }
    }

    private void SetRagdollState(bool state)
    {
        animator.enabled = !state;
        agent.enabled = !state;
        foreach (var item in ragdollColliders)
        {
            item.isTrigger = !state;
        }
        mainCollider.enabled = !state;
    }

    private void AddImpulse(float impulse)
    {
        GetupForcePoint.AddForce(Vector3.up * impulse, ForceMode.Impulse);
    }
}