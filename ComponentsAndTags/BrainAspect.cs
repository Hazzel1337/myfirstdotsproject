using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct BrainAspect : IAspect
{
    public readonly Entity Entity;
    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRW<BrainHealth> _brainHealth;
    private readonly DynamicBuffer<BrainDamageBufferElement> _brainDamageBuffer;

    public void DamageBrain()
    {
        

        foreach (var brainDamageBufferElement in _brainDamageBuffer)
        {
            _brainHealth.ValueRW.Value -= brainDamageBufferElement.Value;
        }
        _brainDamageBuffer.Clear();

        var ltw = _transform.ValueRW.Scale = _brainHealth.ValueRO.Value / _brainHealth.ValueRO.Value;
        _transform.ValueRW.Scale = ltw;
    }
}
