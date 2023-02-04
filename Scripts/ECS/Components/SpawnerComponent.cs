using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerComponent : IComponentData
{
    public Entity Prefab;
    public float systemStepTime;
    public float nextSystemTime;

    public float4 randomValue;

    public bool useGlobalProjectileTurning;
    public float3 globalTurningDirection;

    public bool killAllProjectiles;
    public float2 playerPosition;
    public float playerhitbox;
}
