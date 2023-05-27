using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct AsteroidSpawnPoints : IComponentData
{ 
    public BlobAssetReference<AsteroidSpawnPointsBlob> Value;

}

public struct AsteroidSpawnPointsBlob
{
    public BlobArray<float3> Value;
}
