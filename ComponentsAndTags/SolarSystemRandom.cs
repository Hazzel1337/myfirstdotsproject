using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

public struct SolarSystemRandom : IComponentData
{
    public Random Value;
}
