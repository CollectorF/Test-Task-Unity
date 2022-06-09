using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponParameters parameters;

    private Rigidbody rigidBody;
    private StatsSystem statsSystem;
    private GameObject collisionObject;

    public delegate void OnCollideWithoutStatSystem(GameObject enteredObject);
    public delegate bool OnCollideWithStatSystem(StatsSystem enteredStatsSystem);
    public delegate void OnEffectedActivated(StatsSystem activatedStatsSystem);

    public event OnCollideWithoutStatSystem OnCollide;
    public event OnCollideWithStatSystem OnEnter;
    public event OnEffectedActivated OnActivate;

    private float HealthChangeValue
    {
        get => (collisionObject.gameObject.tag == "Enemy/Enemy_Head" ? parameters.HealthDamageInHead : parameters.HealthDamageInBody) * -1;
    }

    private float SpeedChangeValue
    {
        get => (collisionObject.gameObject.tag == "Enemy/Enemy_Head" ? parameters.SpeedDamageInHead : parameters.SpeedDamageInBody) * -1;
    }


    private void OnTriggerEnter(Collider collider)
    {
        collisionObject = collider.gameObject;
        statsSystem = collisionObject.GetComponentInParent<StatsSystem>();
        if (statsSystem == null)
        {
            OnCollide?.Invoke(collisionObject);
            return;
        }
        bool? shouldExecuteEffect = OnEnter?.Invoke(statsSystem);
        if (!shouldExecuteEffect.HasValue || shouldExecuteEffect.Value)
        {
            ExecuteEffect(statsSystem);
            OnActivate?.Invoke(statsSystem);
        }
    }

    private BaseStateChange GetHeahthStateChange()
    {
        return new HealthStateChange(HealthChangeValue);
    }
    private BaseStateChange GetSpeedStateChange()
    {
        return new SpeedStateChange(SpeedChangeValue);
    }

    private void ExecuteEffect(StatsSystem targetStatSystem)
    {
        targetStatSystem.ApplyStateChange(GetSpeedStateChange());
        targetStatSystem.ApplyStateChange(GetHeahthStateChange());
        Debug.Log($"Health: {targetStatSystem.State.Health} / {targetStatSystem.State.MaxHealth} \n Speed: {targetStatSystem.State.Speed}");
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        OnCollide += enteredObject =>
        {
            if (enteredObject.CompareTag("Weapon"))
            {
                return;
            }
            Die();
        };

        OnEnter += statsSystem => statsSystem.gameObject.CompareTag("Enemy/Enemy");
        OnActivate += statsSystem => Die();
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(transform.position + transform.forward * parameters.ThrowSpeed * Time.fixedDeltaTime);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}