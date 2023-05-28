using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct ShipDamageAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRW<ShipWalkTimer> _shipTimer;
    private readonly RefRO<ShipDamageProperties> _damageProperties;
    private readonly RefRO<ShipHeading> _heading;

    private float DamagePerSecond => _damageProperties.ValueRO.DamagePerSecond;
    private float DamageAmplitude => _damageProperties.ValueRO.EatAmplitude;
    private float DamageFrequency => _damageProperties.ValueRO.EatFrequency;
    private float Heading => _heading.ValueRO.Value;

    private float ShipDamageTimer
    {

        get => _shipTimer.ValueRO.Value;
        set => _shipTimer.ValueRW.Value = value;
    }



    public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brainEnitity)
    {
        ShipDamageTimer += deltaTime;
        var eatAngle = DamageAmplitude * math.sin(deltaTime * ShipDamageTimer);
        _transform.ValueRW.Rotation = quaternion.Euler(eatAngle, Heading, 0);
        var damage = DamagePerSecond * deltaTime;
        var damageThisFrame = new BrainDamageBufferElement { Value = damage };
        ecb.AppendToBuffer(sortKey, brainEnitity, damageThisFrame);
    }

    public bool IsInEatingRange(float3 brainPosition, float brainRadiusSq)
    {
        return math.distancesq(brainPosition, _transform.ValueRO.Position) <= brainRadiusSq - 1;
    }

}


/*Found 1 leak(s) from callstack:
0x000002a9169c98cb(Mono JIT Code) Unity.Collections.NativeArray`1 < Unity.Entities.Content.RuntimeContentManagerProfilerFrameData >:Allocate(int, Unity.Collections.Allocator, Unity.Collections.NativeArray`1 < Unity.Entities.Content.RuntimeContentManagerProfilerFrameData > &)
0x000002a9169c97b3(Mono JIT Code) Unity.Collections.NativeArray`1 < Unity.Entities.Content.RuntimeContentManagerProfilerFrameData >:.ctor(int, Unity.Collections.Allocator, Unity.Collections.NativeArrayOptions)
0x000002a9169c95e3(Mono JIT Code) Unity.Entities.Content.RuntimeContentManager:PrepareArray(Unity.Collections.NativeArray`1 < Unity.Entities.Content.RuntimeContentManagerProfilerFrameData > &, int)(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / Content / RuntimeContentManager.cs:556)
0x000002a9169c785b(Mono JIT Code) Unity.Entities.Content.RuntimeContentManager:SendProfilerFrameData()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / Content / RuntimeContentManager.cs:578)
0x000002a8ac1c976b(Mono JIT Code)(wrapper native - to - managed) Unity.Entities.Content.RuntimeContentManager:SendProfilerFrameData()
0x00007fffcc231ed3(22e6b039d3fcfa38159f3a2b69aee0c) Unity.Entities.Content.RuntimeContentManager.Unity.Entities.Content.ProcessQueuedCommands_00001A30$BurstDirectCall.Invoke(at C:/ Users / hazzel / Unity Projects / dotsHelpMeJessus / Library / PackageCache / com.unity.burst@1.8.4 /.Runtime / unknown / unknown:0)
0x00007fffcc2317df(22e6b039d3fcfa38159f3a2b69aee0c) fe2a28e6fd637e205a531ea3dcc79baa
0x000002a9169c768d(Mono JIT Code)(wrapper managed - to - native) Unity.Entities.Content.RuntimeContentManager / Unity.Entities.Content.ProcessQueuedCommands_00001A30$BurstDirectCall: wrapper_native_indirect_000002A90495A370(intptr &)
0x000002a9169c73c3(Mono JIT Code) Unity.Entities.Content.RuntimeContentManager / Unity.Entities.Content.ProcessQueuedCommands_00001A30$BurstDirectCall: Invoke()
0x000002a9169c733b(Mono JIT Code) Unity.Entities.Content.RuntimeContentManager:ProcessQueuedCommands()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / Content / RuntimeContentManager.cs:627)
0x000002a9169c71d3(Mono JIT Code) Unity.Entities.Content.RuntimeContentSystem:OnUpdate()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / Content / RuntimeContentSystem.cs:99)
0x000002a9169c0b13(Mono JIT Code) Unity.Entities.SystemBase:Update()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / SystemBase.cs:420)
0x000002a9169c203c(Mono JIT Code) Unity.Entities.ComponentSystemGroup:UpdateAllSystems()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / ComponentSystemGroup.cs:710)
0x000002a9169c1a73(Mono JIT Code) Unity.Entities.ComponentSystemGroup:OnUpdate()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / ComponentSystemGroup.cs:666)
0x000002a9169c1873(Mono JIT Code) Unity.Entities.InitializationSystemGroup:OnUpdate()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / DefaultWorld.cs:170)
0x000002a9169c0b13(Mono JIT Code) Unity.Entities.SystemBase:Update()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / SystemBase.cs:420)
0x000002a9169c082d(Mono JIT Code) Unity.Entities.ScriptBehaviourUpdateOrder / DummyDelegateWrapper:TriggerUpdate()(at./ Library / PackageCache / com.unity.entities@1.0.0 - pre.65 / Unity.Entities / ScriptBehaviourUpdateOrder.cs:528)
0x000002a91f8d2918(Mono JIT Code)(wrapper runtime - invoke) object:runtime_invoke_void__this__(object, intptr, intptr, intptr)
0x00007fffd696e084(mono - 2.0 - bdwgc) mono_jit_runtime_invoke(at C:/ build / output / Unity - Technologies / mono / mono / mini / mini - runtime.c:3445)

*/

