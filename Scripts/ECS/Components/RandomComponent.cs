using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct RandomComponent : IComponentData
{
    public Unity.Mathematics.Random random;
}
