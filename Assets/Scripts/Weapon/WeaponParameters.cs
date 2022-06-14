using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "Weapons/Default")]
public class WeaponParameters : ScriptableObject
{
    public bool needsTorque;
    public float ThrowForce;
    public Vector3 AngleVelocity;
    public float SpeedDamageInHead;
    public float HealthDamageInHead;
    public float SpeedDamageInBody;
    public float HealthDamageInBody;
}
