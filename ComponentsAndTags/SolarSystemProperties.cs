using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct SolarSystemProperties : IComponentData
{
    public float2 SpaceStationSqaureDimension;
    public int NumberOfAsteroidsToSpawn;
    public Entity AsteroidPrefab;
    public Entity ShipPrefab;
    public float ShipSpawnRate;
}

public struct ShipSpawnTimer : IComponentData
{
    public float Value;
}
