using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SolarSystemMono : MonoBehaviour
{
    public float2 SpaceStationSqaureDimension;
    public int NumberOfAsteroidsToSpawn;
    public GameObject AsteroidPrefab;
    public uint RandomSeed;
    public GameObject ShipPrefab;
    public float ShipSpawnRate;
}

public class SolarSystemBaker : Baker<SolarSystemMono>
{
    public override void Bake(SolarSystemMono authoring)
    {
        var solarSystemEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(solarSystemEntity,
            new SolarSystemProperties
            {
                SpaceStationSqaureDimension = authoring.SpaceStationSqaureDimension,
                NumberOfAsteroidsToSpawn = authoring.NumberOfAsteroidsToSpawn,
                AsteroidPrefab = GetEntity(authoring.AsteroidPrefab, TransformUsageFlags.Dynamic),
                ShipPrefab = GetEntity(authoring.ShipPrefab, TransformUsageFlags.Dynamic),
                ShipSpawnRate = authoring.ShipSpawnRate
            });
        AddComponent(solarSystemEntity, new SolarSystemRandom
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed)
        });
        AddComponent<AsteroidSpawnPoints>(solarSystemEntity);
        AddComponent<ShipSpawnTimer>(solarSystemEntity);
    }
}

