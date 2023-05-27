using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using System.Globalization;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(ShipWalkSystem))]
public partial struct ShipDamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
        var brainScale = SystemAPI.GetComponent<LocalTransform>(brainEntity).Scale;
        var brainRadius = brainScale * 5f + 1f;

        new ShipDamageJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            BrainEntity = brainEntity,
            BrainRadiusSq = brainRadius * brainRadius

        }.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct ShipDamageJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity BrainEntity;
        public float BrainRadiusSq;

        private void Execute(ShipDamageAspect shipDamageAspect, [EntityIndexInQuery] int sortKey)
        {
            if (shipDamageAspect.IsInEatingRange(float3.zero, BrainRadiusSq)) shipDamageAspect.Eat(DeltaTime, ECB, sortKey, BrainEntity);
            else
            {
                ECB.SetComponentEnabled<ShipDamageProperties>(sortKey, shipDamageAspect.Entity, false);
                ECB.SetComponentEnabled<ShipWalkProperties>(sortKey, shipDamageAspect.Entity, true);
            }
        }
    }
}
