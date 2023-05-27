using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct SolarSystemAspect:IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<SolarSystemProperties> solarSystemProperity;

    private readonly RefRW<SolarSystemRandom> _solarSystemRandom;

    private readonly RefRO<LocalTransform> _transform;
    private LocalTransform Transform => _transform.ValueRO;

    public int NumberOfAsteroidsToSpawn => solarSystemProperity.ValueRO.NumberOfAsteroidsToSpawn;

    public Entity AsteroidToSpawn => solarSystemProperity.ValueRO.AsteroidPrefab;

    private readonly RefRW<AsteroidSpawnPoints> _asteroidSpawnPoints;

    private int AsteroidSpawnPointCount => _asteroidSpawnPoints.ValueRO.Value.Value.Value.Length;

    private readonly RefRW<ShipSpawnTimer>  shipSpawnTimer;


    public LocalTransform GetRandomAsteroidTransform()
    {
        return new LocalTransform
        {
            Position = GetRandomAsteroidSpawnPoint(),
            Rotation = GetRandomRotation(),
            Scale = GetRandomScale(0.5f)
        };
    }

    private float3 GetRandomAsteroidSpawnPoint()
    {
        float3 randomPosition;
        do
        {
            randomPosition = _solarSystemRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
        } while (math.distancesq(Transform.Position, randomPosition) <= BRAIN_SAFTEY_RADIUS_SQ);
        return randomPosition;
    }

    private float3 MinCorner => Transform.Position - HalfDimensions;
    private float3 MaxCorner => Transform.Position + HalfDimensions;

    private const float BRAIN_SAFTEY_RADIUS_SQ = 100;

    private float3 HalfDimensions => new float3()
    {
        x = solarSystemProperity.ValueRO.SpaceStationSqaureDimension.x * 0.5f,
        y = 0f,
        z = solarSystemProperity.ValueRO.SpaceStationSqaureDimension.y * 0.5f
    };

    public bool AsteroidSpawnPointInitalized()
    { 
        return _asteroidSpawnPoints.ValueRO.Value.IsCreated && AsteroidSpawnPointCount > 0;
    }

    public float ShipSpawnTimer
    {
        get => shipSpawnTimer.ValueRO.Value;
        set => shipSpawnTimer.ValueRW.Value = value;
    }

    public bool IsItTimeToSpawnShip => ShipSpawnTimer <= 0f;

    private const float lowerBound = -0.25f;
    private const float upperBound = 0.25f;

    private quaternion GetRandomRotation() => quaternion.RotateY(_solarSystemRandom.ValueRW.Value.NextFloat(lowerBound, upperBound));
    private float GetRandomScale(float min) => _solarSystemRandom.ValueRW.Value.NextFloat(min, 1f);

    public float ShipSpawnRate => solarSystemProperity.ValueRO.ShipSpawnRate;

    public Entity ShipPrefab => solarSystemProperity.ValueRO.ShipPrefab;

    public LocalTransform GetShipSpawnPoint()
    {
        var spawnPosition = GetRandomAsteroidSpawnPoint();
        return new LocalTransform
        {
            Position = spawnPosition,
            Rotation = quaternion.RotateY(MathHelpers.GetHeading(spawnPosition, Transform.Position)),
            Scale = 1f

        };
    }

    


}
