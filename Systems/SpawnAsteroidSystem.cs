using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnAsteroidSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SolarSystemProperties>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var solarSystemEntity = SystemAPI.GetSingletonEntity<SolarSystemProperties>();
        var solarSystemAspect = SystemAPI.GetAspectRW<SolarSystemAspect>(solarSystemEntity);
        var tombstoneOffset = new float3(0f, -2f, 1f);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var builder = new BlobBuilder(Allocator.Temp);
        ref var spawnPoints = ref builder.ConstructRoot<AsteroidSpawnPointsBlob>();
        var arrayBuilder = builder.Allocate(ref spawnPoints.Value, solarSystemAspect.NumberOfAsteroidsToSpawn);

        for (int i = 0; i < solarSystemAspect.NumberOfAsteroidsToSpawn; i++)
        {
            var newAsteroidEntity = ecb.Instantiate(solarSystemAspect.AsteroidToSpawn);
            var newAsteroidTransform = solarSystemAspect.GetRandomAsteroidTransform();
            ecb.SetComponent(newAsteroidEntity, newAsteroidTransform);

            var newAsteroidSpawnPoint = newAsteroidTransform.Position;// + tombstoneOffset;
            arrayBuilder[i] = newAsteroidSpawnPoint;

        }

        var blobAsset = builder.CreateBlobAssetReference<AsteroidSpawnPointsBlob>(Allocator.Persistent);
        ecb.SetComponent(solarSystemEntity, new AsteroidSpawnPoints { Value = blobAsset });
        builder.Dispose();
        ecb.Playback(state.EntityManager);
    }
}

