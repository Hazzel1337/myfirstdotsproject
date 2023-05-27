using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct ShipWalkAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _trasform;
    private readonly RefRW<ShipWalkTimer> _walkTimer;
    private readonly RefRO<ShipWalkProperties> _walkProperties;
    private readonly RefRO<ShipHeading> _heading;

    private float WalkSpeed => _walkProperties.ValueRO.WalkSpeed;
    private float WalkAmplitude => _walkProperties.ValueRO.WalkAmplitude;
    private float WalkFrequency => _walkProperties.ValueRO.WalkFreuqency;
    private float Heading => _heading.ValueRO.Value;


    private float WalkTimer
    {
        get => _walkTimer.ValueRO.Value;
        set => _walkTimer.ValueRW.Value = value;
    }

    public void Walk(float deltaTime)
    { 
        WalkTimer += deltaTime;
        _trasform.ValueRW.Position += _trasform.ValueRO.Forward() * WalkSpeed * deltaTime;

        var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
        _trasform.ValueRW.Rotation = quaternion.Euler(0, Heading, swayAngle);
        
    }

    public bool IsInStoppingRange(float3 brainPosition, float brainRadiusSQ)
    { 
        return math.distancesq(brainPosition, _trasform.ValueRO.Position) <= brainRadiusSQ;
        
    }

}
