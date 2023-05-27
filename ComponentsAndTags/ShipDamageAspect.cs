using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct ShipDamageAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRW<ShipWalkTimer> _shipTimer;
    private readonly RefRO<ShipDamageProperties> _damageProperties;
    private readonly RefRO<ShipHeading> _heading;

    private float DamagePerSecond => _damageProperties.ValueRO.DamagePerSecond;
    private float DamageAmplitude => _damageProperties.ValueRO.EatAmplitude;
    private float DamageFrequency => _damageProperties.ValueRO.EatFrequency;
    private float Heading => _heading.ValueRO.Value;

    private float ShipDamageTimer
    {

        get => _shipTimer.ValueRO.Value;
        set => _shipTimer.ValueRW.Value = value;
    }



    public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brainEnitity)
    {
        ShipDamageTimer += deltaTime;
        var eatAngle = DamageAmplitude * math.sin(deltaTime * ShipDamageTimer);
        _transform.ValueRW.Rotation = quaternion.Euler(eatAngle, Heading, 0);
        var damage = DamagePerSecond * deltaTime;
        var damageThisFrame = new BrainDamageBufferElement { Value = damage };
        ecb.AppendToBuffer(sortKey, brainEnitity, damageThisFrame);
    }

    public bool IsInEatingRange(float3 brainPosition, float brainRadiusSq)
    {
        return math.distancesq(brainPosition, _transform.ValueRO.Position) <= brainRadiusSq - 1;
    }
}
