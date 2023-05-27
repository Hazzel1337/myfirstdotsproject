using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using System.Threading;
using Unity.Collections;

[BurstCompile]
public partial struct IniTalizeShipSystem : ISystem
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
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var item in SystemAPI.Query<ShipWalkAspect>().WithAll<NewShipTag>())
        {
            ecb.RemoveComponent<NewShipTag>(item.Entity);
            ecb.SetComponentEnabled<ShipWalkProperties>(item.Entity, false);
            ecb.SetComponentEnabled<ShipDamageProperties>(item.Entity, false);
        }

        ecb.Playback(state.EntityManager);
        
    }

}
