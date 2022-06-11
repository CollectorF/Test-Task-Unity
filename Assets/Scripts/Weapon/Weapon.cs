using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
public class Weapon : MonoBehaviour
{
    public WeaponParameters parameters;

    private FixedJoint joint;
    private Rigidbody rigidBody;
    private Rigidbody targetRigidBody;
    private Collider collider;
    private StatsSystem statsSystem;
    private GameObject collisionObject;
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
        isAlive = true;

        OnCollide += enteredObject =>
        {
            if (enteredObject.CompareTag("Weapon"))
            {
                return;
            }
            else if (enteredObject.CompareTag("Level"))
            {
                isAlive = false;
                rigidBody.Sleep();
            }
        };

        OnEnter += statsSystem => statsSystem.gameObject.CompareTag("Enemy/Enemy");
        OnActivate += statsSystem => StuckInTarget();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (isAlive)
        {
            collisionObject = collider.gameObject;
            statsSystem = collisionObject.GetComponentInParent<StatsSystem>();
            targetRigidBody = collisionObject.GetComponentInParent<Rigidbody>();
            if (statsSystem == null)
            {
                OnCollide?.Invoke(collisionObject);
                isAlive = false;
                return;
            }
            bool? shouldExecuteEffect = OnEnter?.Invoke(statsSystem);
            if (!shouldExecuteEffect.HasValue || shouldExecuteEffect.Value)
            {
                ExecuteEffect(statsSystem);
                OnActivate?.Invoke(statsSystem);
            }
        }
    }

    private void ExecuteEffect(StatsSystem targetStatSystem)
    {
        targetStatSystem.ApplyStateChange(GetSpeedStateChange());
        targetStatSystem.ApplyStateChange(GetHeahthStateChange());
        Debug.Log($"Health: {targetStatSystem.State.Health} / {targetStatSystem.State.MaxHealth} \n Speed: {targetStatSystem.State.Speed}");
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            rigidBody.MovePosition(transform.position + transform.forward * parameters.ThrowSpeed * Time.fixedDeltaTime);
        }

        //if (isAlive)
        //{
        //    rigidBody.MovePosition(position * parameters.ThrowSpeed * Time.fixedDeltaTime);
        //    position = position + Vector3.forward;
        //    if (parameters.needsTorque)
        //    {
        //        Quaternion deltaRotation = Quaternion.Euler(parameters.AngleVelocity * Time.fixedDeltaTime);
        //        rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        //    }
        //}
    }

    private BaseStateChange GetHeahthStateChange()
    {
        return new HealthStateChange(healthChangeValue);
    }
    private BaseStateChange GetSpeedStateChange()
    {
        return new SpeedStateChange(speedChangeValue);
    }

    private void StuckInTarget()
    {
        isAlive = false;
        collider.enabled = false;
        joint.connectedBody = targetRigidBody;
        transform.parent = collisionObject.transform;
        rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = false;
        rigidBody.isKinematic = false;

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}