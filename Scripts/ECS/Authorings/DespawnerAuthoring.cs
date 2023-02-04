using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DespawnerAuthoring : MonoBehaviour
{
    public bool destroy;
}

public class DespawnerBaker : Baker<DespawnerAuthoring>
{
    public override void Bake(DespawnerAuthoring authoring)
    {
        AddComponent(new DespawnerComponent
        {
            destroy = authoring.destroy,
            hitPlayer = false,
            value = 0
        });
    }
}