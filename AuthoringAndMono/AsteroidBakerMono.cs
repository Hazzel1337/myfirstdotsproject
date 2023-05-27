using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Mathematics;

public class AsteroidBakerMono : MonoBehaviour
{
    public GameObject Renderer;
}

public class TombstoneBaker : Baker<AsteroidBakerMono>
{
    public override void Bake(AsteroidBakerMono authoring)
    {
        var tombstoneEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(tombstoneEntity, new TombstoneRenderer
        {
            Value = GetEntity(authoring.Renderer, TransformUsageFlags.Dynamic)
        });
    }
}

[MaterialProperty("TombstoneOffset")]
public struct TombstoneOffset : IComponentData 
{
    public float2 Value;
}

public struct TombstoneRenderer : IComponentData
{
    public Entity Value;
}

