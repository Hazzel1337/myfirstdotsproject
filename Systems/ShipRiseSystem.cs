using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using System.Resources;

[BurstCompile]
[UpdateAfter(typeof(SpawnShipSystem))]
public partial struct ShipRiseSystem : ISystem
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
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingelton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        new ShipRiseJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingelton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            
        }.ScheduleParallel();
    }

   
}

[BurstCompile]
public partial struct ShipRiseJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ECB;
    [BurstCompile]
    private void Execute(ShipRiseAspect shipRiseAspect, [ChunkIndexInQuery] int sortKey)
    {
        shipRiseAspect.Rise(DeltaTime);
        if (!shipRiseAspect.IsAboveGround) return;

        shipRiseAspect.SetAtGroundLevel();
        ECB.RemoveComponent<ShipRiseRate>(sortKey,shipRiseAspect.Entity);
        ECB.SetComponentEnabled<ShipWalkProperties>(sortKey, shipRiseAspect.Entity, true);
    }
}
