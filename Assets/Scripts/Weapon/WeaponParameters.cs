using System;
using UnityEngine;

[Serializable]
public struct WeaponParameters
{
    public bool needsTorque;
    public float ThrowSpeed;
    public Vector3 AngleVelocity;
    public float SpeedDamageInHead;
    public float HealthDamageInHead;
    public float SpeedDamageInBody;
    public float HealthDamageInBody;
}
