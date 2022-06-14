using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StatsSystem))]
[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacterController : MonoBehaviour
{
    [SerializeField]
    protected CharacterStarterInfo starterInfo;

    protected StatsSystem stats; 
    protected NavMeshAgent agent;
    protected GameManager gameManager;
    private SkinnedMeshRenderer[] meshRenderers;

    public delegate void DieEvent(BaseCharacterController controller);
    public delegate void InjuryEvent(BaseCharacterController controller);

    public event DieEvent OnDie;
    public event InjuryEvent OnKnockOut;

    protected virtual void Awake()
    {
        stats = GetComponent<StatsSystem>();
        agent = GetComponent<NavMeshAgent>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        stats.OnStateChanged += OnStateUpdate;
    }

    protected virtual void Start()
    {
        stats.State = InitializeState();
        agent.enabled = true;
    }

    protected virtual BaseState InitializeState()
    {
        return new BaseState(starterInfo);
    }
    protected virtual void OnStateUpdate(BaseState oldState, BaseState newState)
    {
        if (newState.Health <= 0)
        {
            OnDie?.Invoke(this);
            foreach (var item in meshRenderers)
            {
                item.material.color = Color.gray;
            }
        }
        if (oldState != null && oldState.Health != newState.Health)
        {
            OnKnockOut?.Invoke(this);
        }
    }
}