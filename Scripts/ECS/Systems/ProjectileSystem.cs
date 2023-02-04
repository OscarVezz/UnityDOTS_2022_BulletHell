using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
[UpdateAfter(typeof(SpawnerSystem))]
public partial struct ProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        new MoveJob
        {
            globalProjectileComponent = SystemAPI.GetSingleton<SpawnerComponent>(),
            deltaTime = deltaTime,
            elapsedTime = SystemAPI.Time.ElapsedTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public SpawnerComponent globalProjectileComponent;
    public float deltaTime;
    public double elapsedTime;

    public void Execute(SimpleProjectileMovementAspect simpleMovementAspect)
    {
        simpleMovementAspect.Move(deltaTime, globalProjectileComponent, elapsedTime);
    }
}

[BurstCompile]
public partial struct Randomize : IJobEntity
{
    public RefRW<RandomComponent> randomComponent;

    public void Execute(SimpleProjectileMovementAspect simpleMovementAspect)
    {
        simpleMovementAspect.GetRandom(randomComponent);
    }
}