using UnityEngine;

public class EnemyController : BaseCharacterController
{

    public BaseState State => stats.State;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    [ContextMenu("Damage")]
    private void Damage()
    {
        stats.ApplyStateChange(new HealthStateChange(-10));
        Debug.Log($"Health: {stats.State.Health} / {stats.State.MaxHealth}");
    }

    [ContextMenu("Speed")]
    private void Speed()
    {
        stats.ApplyStateChange(new SpeedStateChange(1));
        Debug.Log($"Speed: {stats.State.Speed}");
    }
}