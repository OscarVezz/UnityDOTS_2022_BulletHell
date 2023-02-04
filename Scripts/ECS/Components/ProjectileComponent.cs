
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileComponent : IComponentData
{
    public float velocity;

    public double spawnInitTime;
    public float lifeTime;

    public float projectileValue;
}
