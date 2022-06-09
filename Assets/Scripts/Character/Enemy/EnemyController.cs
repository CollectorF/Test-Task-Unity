using UnityEngine;
using UnityEngine.AI;

public class EnemyController : BaseCharacterController
{
    [SerializeField]
    private NavMeshAgent agent;
    private Collider[] colliders;
    public BaseState State => stats.State;
    private bool isDead = false;


    protected override void Awake()
    {
        base.Awake();
        OnDie += controller => isDead = true;
        colliders = GetComponentsInChildren<Collider>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnStateUpdate(BaseState oldState, BaseState newState)
    {
        base.OnStateUpdate(oldState, newState);
        if (oldState == null || oldState.Speed != newState.Speed)
        {
            agent.speed = newState.Speed;
        }
    }
}