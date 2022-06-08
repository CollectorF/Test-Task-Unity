using UnityEngine;

[RequireComponent(typeof(StatsSystem))]
public class BaseCharacterController : MonoBehaviour
{
    [SerializeField]
    protected CharacterStarterInfo starterInfo;

    protected StatsSystem stats;


    protected virtual void Awake()
    {
        stats = GetComponent<StatsSystem>();
    }

    protected virtual void Start()
    {
        stats.State = InitializeState();
    }

    protected virtual BaseState InitializeState()
    {
        return new BaseState(starterInfo);
    }
}