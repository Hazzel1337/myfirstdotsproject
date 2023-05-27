using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShipMono : MonoBehaviour
{
    public float RiseRate;
    public float WalkSpeed;
    public float WalkAmpltiude;
    public float WalkFrequency;

    public float EatDamage;
    public float EatAmplitude;
    public float EatFrequency;
}

public class ShipBaker : Baker<ShipMono>
{
    public override void Bake(ShipMono authoring)
    {
        var shipEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<ShipRiseRate>(shipEntity, new ShipRiseRate {Value = authoring.RiseRate });
        AddComponent<ShipWalkProperties>(shipEntity, new ShipWalkProperties
        {
            WalkSpeed = authoring.WalkSpeed,
            WalkAmplitude = authoring.WalkAmpltiude,
            WalkFreuqency = authoring.WalkFrequency
        });

        AddComponent<ShipWalkTimer>(shipEntity);
        AddComponent<ShipHeading>(shipEntity);
        AddComponent<NewShipTag>(shipEntity);

        AddComponent<ShipDamageProperties>(shipEntity, new ShipDamageProperties
        {
            DamagePerSecond = authoring.EatDamage,
            EatAmplitude = authoring.EatAmplitude,
            EatFrequency = authoring.EatFrequency,
        });
    }
}