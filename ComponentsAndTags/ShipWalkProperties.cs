using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ShipWalkProperties : IComponentData, IEnableableComponent
{
    public float WalkSpeed;
    public float WalkAmplitude;
    public float WalkFreuqency;
}

public struct ShipDamageProperties : IComponentData, IEnableableComponent
{
    public float DamagePerSecond;
    public float EatAmplitude;
    public float EatFrequency;
}

public struct ShipWalkTimer : IComponentData
{
    public float Value;
}

public struct ShipHeading : IComponentData
{
    public float Value;
}

public struct NewShipTag : IComponentData { }


