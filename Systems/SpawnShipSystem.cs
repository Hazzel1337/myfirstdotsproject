using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnShipSystem : ISystem
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
        state.CompleteDependency();

        var DeltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingelton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        new SpawnShipJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged)

        }.Run();
    }

    [BurstCompile]
    public partial struct SpawnShipJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        private void Execute(SolarSystemAspect solarSystemAspect)
        {

            solarSystemAspect.ShipSpawnTimer -= DeltaTime;
            if (!solarSystemAspect.IsItTimeToSpawnShip) return;
            if(!solarSystemAspect.AsteroidSpawnPointInitalized()) return;
            solarSystemAspect.ShipSpawnTimer = solarSystemAspect.ShipSpawnRate;
            var newShip = ECB.Instantiate(solarSystemAspect.ShipPrefab);
            var newShipTransform = solarSystemAspect.GetShipSpawnPoint();
            ECB.SetComponent(newShip, newShipTransform);
        }
    }
}
