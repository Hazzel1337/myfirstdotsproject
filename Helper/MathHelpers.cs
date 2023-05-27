using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class MathHelpers 
{
    public static float GetHeading(float3 objectPosition, float3 targetPosition)
    { 
        var x = objectPosition.x -targetPosition.x;
        var y = objectPosition.z -targetPosition.z;
        return math.atan2(x, y) + math.PI;
    }
}
