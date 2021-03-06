using System;
using UnityEngine;

[Serializable]
public class BaseState
{
    public float Speed { get; }
    public float Health { get; }
    public float MaxHealth { get; }

    public BaseState(float speed, float health, float maxHealth)
    {
        Speed = Mathf.Clamp(speed, 0, Mathf.Infinity);
        MaxHealth = maxHealth;
        Health = Mathf.Clamp(health, 0, maxHealth);
    }

    public BaseState(CharacterStarterInfo starerInfo)
        : this(starerInfo.Speed, starerInfo.MaxHealth, starerInfo.MaxHealth)
    {

    }

    public virtual BaseState Mutate(
        float? speed = null,
        float? health = null,
        float? maxHealth = null
    )
    {
        return new BaseState(
            speed ?? Speed,
            health ?? Health,
            maxHealth ?? MaxHealth
        );
    }
}