using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(ShipRiseSystem))]
public partial struct ShipWalkSystem : ISystem
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
        var ecbSingelton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
        var brainScale = SystemAPI.GetComponent<LocalTransform>(brainEntity).Scale;
        var brainRadius = brainScale * 5f + 0.5f;
        new ShipWalkJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = ecbSingelton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            BrainRadiusSq = brainRadius * brainRadius
            
        }.ScheduleParallel();

        
    }


    public partial struct ShipWalkJob : IJobEntity
    {
        public float DeltaTime;
        public float BrainRadiusSq;
        public EntityCommandBuffer.ParallelWriter ECB;
        private void Execute(ShipWalkAspect shipWalkAspect, [EntityIndexInQuery] int sortKey)
        {
            shipWalkAspect.Walk(DeltaTime);
            if (shipWalkAspect.IsInStoppingRange(float3.zero, BrainRadiusSq))
            {
                ECB.SetComponentEnabled<ShipWalkProperties>(sortKey, shipWalkAspect.Entity, false);
                ECB.SetComponentEnabled<ShipDamageProperties>(sortKey, shipWalkAspect.Entity, true);
            }

        }
    }
     
}


