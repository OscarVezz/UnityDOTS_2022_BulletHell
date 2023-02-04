using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

class SpawnerAuthoring : MonoBehaviour
{
    [Header("System Specifications")]
    public float systemStepTime;


    [Header("Remenants")]
    public bool useGlobalProjectileTurning;
    public float3 globalTurningDirection;


    [Space]
    public GameObject Prefab;
    public bool killAllProjectiles;
}

class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        AddComponent(new SpawnerComponent
        {
            useGlobalProjectileTurning = authoring.useGlobalProjectileTurning,
            globalTurningDirection = authoring.globalTurningDirection,


            // By default, each authoring GameObject turns into an Entity.
            // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
            Prefab = GetEntity(authoring.Prefab),
            killAllProjectiles = authoring.killAllProjectiles,
            systemStepTime = authoring.systemStepTime,
            nextSystemTime = 0.0f,

            randomValue = 0.0f
        });
    }
}

