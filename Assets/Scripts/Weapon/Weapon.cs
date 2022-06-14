using UnityEngine;

[RequireComponent(typeof(VFXProcessor))]
public class Weapon : MonoBehaviour
{
    public WeaponParameters parameters;

    private Rigidbody rigidBody;
    private Collider weaponCollider;
    private StatsSystem statsSystem;
    private GameObject collisionObject;
    private VFXProcessor vfxProcessor;
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
        weaponCollider = GetComponent<Collider>();
        vfxProcessor = GetComponent<VFXProcessor>();
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
                Destroy(rigidBody);
                weaponCollider.enabled = false;
            }
        };

        OnEnter += statsSystem => statsSystem.gameObject.CompareTag("Enemy/Enemy");
        OnActivate += statsSystem => StuckInTarget();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isAlive)
        {
            collisionObject = collision.gameObject;
            statsSystem = collisionObject.GetComponentInParent<StatsSystem>();
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
                ContactPoint contact = collision.contacts[0];
                vfxProcessor.DisplayVFX(contact);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy/Enemy"))
    //    {

    //    }
    //}

    private void ExecuteEffect(StatsSystem targetStatSystem)
    {
        targetStatSystem.ApplyStateChange(GetSpeedStateChange());
        targetStatSystem.ApplyStateChange(GetHeahthStateChange());
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
        weaponCollider.enabled = false;
        transform.parent = collisionObject.transform;
        transform.position = collisionObject.transform.position;
        rigidBody.velocity = Vector3.zero;
        Destroy(rigidBody);
    }

    internal void AddImpulse()
    {
        rigidBody.AddForce(gameObject.transform.forward * parameters.ThrowForce, ForceMode.Impulse);
        if (parameters.needsTorque)
        {
            rigidBody.AddRelativeTorque(parameters.AngleVelocity);
        }
    }
}