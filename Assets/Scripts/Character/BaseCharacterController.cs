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
    private MeshRenderer[] meshRenderers;

    public delegate void DieEvent(BaseCharacterController controller);

    public event DieEvent OnDie;


    protected virtual void Awake()
    {
        stats = GetComponent<StatsSystem>();
        agent = GetComponent<NavMeshAgent>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        stats.OnStateChanged += OnStateUpdate;
    }

    protected virtual void Start()
    {
        stats.State = InitializeState();
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
            agent.isStopped = true;
            agent.enabled = false;
            foreach (var item in meshRenderers)
            {
                item.material.color = Color.gray;
            }
        }
    }
}