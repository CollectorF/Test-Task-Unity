using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponParameters parameters;

    private FixedJoint joint;
    private Rigidbody rigidBody;
    private Rigidbody targetRigidBody;
    private RaycastHit hitPoint;
    private Collider collider;
    private StatsSystem statsSystem;
    private GameObject collisionObject;
    private float speed;
    private bool isAlive;

    public delegate void OnCollideWithoutStatSystem(GameObject enteredObject);
    public delegate bool OnCollideWithStatSystem(StatsSystem enteredStatsSystem);
    public delegate void OnEffectedActivated(StatsSystem activatedStatsSystem);

    public event OnCollideWithoutStatSystem OnCollide;
    public event OnCollideWithStatSystem OnEnter;
    public event OnEffectedActivated OnActivate;

    private float healthChangeValue
    {
        get => (collisionObject.gameObject.tag == "Enemy/Enemy_Head" ? parameters.HealthDamageInHead : parameters.HealthDamageInBody) * -1;
    }

    private float speedChangeValue
    {
        get => (collisionObject.gameObject.tag == "Enemy/Enemy_Head" ? parameters.SpeedDamageInHead : parameters.SpeedDamageInBody) * -1;
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        joint = GetComponent<FixedJoint>();
        collider = GetComponent<Collider>();
        speed = parameters.ThrowSpeed;
        isAlive = true;

        OnCollide += enteredObject =>
        {
            if (enteredObject.CompareTag("Weapon"))
            {
                return;
            }
        };

        OnEnter += statsSystem => statsSystem.gameObject.CompareTag("Enemy/Enemy");
        OnActivate += statsSystem => StuckInTarget();
    }

    private void FixedUpdate()
    {
        MoveTowards();
    }


    private void OnTriggerEnter(Collider collider)
    {
        collisionObject = collider.gameObject;
        statsSystem = collisionObject.GetComponentInParent<StatsSystem>();
        targetRigidBody = collisionObject.GetComponentInParent<Rigidbody>();
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

    private void ExecuteEffect(StatsSystem targetStatSystem)
    {
        targetStatSystem.ApplyStateChange(GetSpeedStateChange());
        targetStatSystem.ApplyStateChange(GetHeahthStateChange());
        Debug.Log($"Health: {targetStatSystem.State.Health} / {targetStatSystem.State.MaxHealth} \n Speed: {targetStatSystem.State.Speed}");
    }

    private BaseStateChange GetHeahthStateChange()
    {
        return new HealthStateChange(healthChangeValue);
    }
    private BaseStateChange GetSpeedStateChange()
    {
        return new SpeedStateChange(speedChangeValue);
    }

    public void SetTargetPoint(RaycastHit raycastHit)
    {
        hitPoint = raycastHit;
    }

    public void MoveTowards()
    {
        if (isAlive)
        {
            rigidBody.MovePosition(transform.position + hitPoint.point * speed * Time.fixedDeltaTime);
        }
    }

    private void StuckInTarget()
    {
        isAlive = false;
        collider.enabled = false;
        joint.connectedBody = targetRigidBody;
        transform.parent = collisionObject.transform;
        rigidBody.useGravity = false;
        rigidBody.isKinematic = false;

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}