using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct DespawnerComponent : IComponentData
{
    public bool destroy;
    public bool hitPlayer;

    public float value;
}
