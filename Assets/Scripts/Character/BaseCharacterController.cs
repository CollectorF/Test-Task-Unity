using UnityEngine;

[RequireComponent(typeof(StatsSystem))]
public class BaseCharacterController : MonoBehaviour
{
    [SerializeField]
    protected CharacterStarterInfo starterInfo;

    protected StatsSystem stats;

    public delegate void DieEvent(BaseCharacterController controller);

    public event DieEvent OnDie;


    protected virtual void Awake()
    {
        stats = GetComponent<StatsSystem>();
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
        }
    }
}